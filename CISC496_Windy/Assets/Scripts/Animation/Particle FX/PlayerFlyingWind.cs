using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Animation
{
    using Controller;

    public class PlayerFlyingWind : MonoBehaviour
    {
        [SerializeField] ParticleSystem Wind;

        ParticleSystem.EmissionModule Emission;
        ParticleSystem.MinMaxCurve EmissionRate;

        [SerializeField] Rigidbody PlayerBody;
        [Tooltip("Speed Limit For Displaying Wind Particles")]
        [SerializeField] float SpeedLimitMin;
        [SerializeField] float SpeedLimitMax;
        [SerializeField] float EmissionRateMin;
        [SerializeField] float EmissionRateMax;

        float SpeedLimitDelta;
        float EmissionRateDelta;

        private void Start()
        {
            Emission = Wind.emission;
            EmissionRate = new();

            SpeedLimitDelta = SpeedLimitMax - SpeedLimitMin;
            EmissionRateDelta = EmissionRateMax - EmissionRateMin;
        }
        private void LateUpdate()
        {
            if (!MM_Executor.Instance.MotionMode.IsGlide() && Controller.ControlDevice.GetAllDirectionKeysIdle())
            {
                Emission.enabled = false;
                return;
            }

            float verticleSpeed = Mathf.Abs(Vector3.Dot(PlayerBody.velocity, Vector3.up));

            if (verticleSpeed > SpeedLimitMin)
            {
                Emission.enabled = true;
                float speedRatio = Mathf.Min((verticleSpeed - SpeedLimitMin) / SpeedLimitDelta, 1.0f);
                Emission.rateOverTime = (EmissionRate.constant = EmissionRateMin + EmissionRateDelta * speedRatio);
            }
            else Emission.enabled = false;
        }
    }
}

