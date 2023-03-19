using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Buoyancy
{
    public abstract class Buoyancy : IBehaviour
    {

        protected internal Transform PlayerTransform;

        protected internal int SpecialZoneLayerMask;

        protected float CloudUpwardAccel { get; set; }

        public abstract float Force { get; }

        float Coefficient(float distance, float effectiveDistance)
        {
            float coefficient = Mathf.Pow(Mathf.Cos((Mathf.PI / (2.0f * effectiveDistance)) * distance), 10.1f);
            if (coefficient is float.NaN) coefficient = 0.0f;
            return coefficient;
        }

        public virtual void Update()
        {
            if (Physics.Raycast(PlayerTransform.position, Vector3.down, out RaycastHit hitInfo, float.PositiveInfinity, SpecialZoneLayerMask))
            {
                float maxDistance = hitInfo.collider.GetComponent<SpecialZone.SpecialZone>().EffectoveDistance;
                CloudUpwardAccel =
                    Coefficient(Mathf.Clamp(hitInfo.distance, 1.0f, maxDistance), maxDistance) *
                    (hitInfo.collider.GetComponent<SpecialZone.SpecialZone>().Buoyancy + Mathf.Abs(PlayerTransform.GetComponent<Rigidbody>().velocity.y));

            }
            else CloudUpwardAccel = 0.0f;
        }

        public virtual void Start() { }
        public virtual void Quit() { }

        public override string ToString() => "Buoyancy Class";
    }
}
