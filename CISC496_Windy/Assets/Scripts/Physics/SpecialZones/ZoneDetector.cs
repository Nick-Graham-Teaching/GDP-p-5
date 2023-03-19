using UnityEngine;

namespace Windy.Obsolete.SpecialZone
{
    public class ZoneDetector : MonoBehaviour
    {
        public Transform Player;

        public int specialZoneLayerMask;

        private float Coefficient(float distance, float effectiveDistance)
        {
            float coefficient = Mathf.Pow(Mathf.Cos((Mathf.PI / (2.0f * effectiveDistance)) * distance), 10.1f);
            if (coefficient is float.NaN) coefficient = 0.0f;
            return coefficient;
        }

        private void LateUpdate()
        {
            if (UnityEngine.Physics.Raycast(Player.position, Vector3.down, out RaycastHit hitInfo, float.PositiveInfinity, specialZoneLayerMask))
            {
                float maxDistance = hitInfo.collider.GetComponent<Windy.SpecialZone.SpecialZone>().EffectoveDistance;
                Buoyancy.Buoyancy.Instance.CloudUpwardAccel =
                    Coefficient(Mathf.Clamp(hitInfo.distance, 1.0f, maxDistance), maxDistance) *
                    (hitInfo.collider.GetComponent<Windy.SpecialZone.SpecialZone>().Buoyancy + Mathf.Abs(Player.GetComponent<Rigidbody>().velocity.y));

            }
            else Buoyancy.Buoyancy.Instance.CloudUpwardAccel = 0.0f;
        }
    }
}
