using UnityEngine;

namespace Windy.SpecialZone
{
    public class SpecialZone : MonoBehaviour
    {
        [SerializeField]
        protected float effectiveDistance;
        [SerializeField]
        protected float buoyancy;

        public virtual float EffectoveDistance { get; }
        public virtual float Buoyancy { get; }
    }

}
