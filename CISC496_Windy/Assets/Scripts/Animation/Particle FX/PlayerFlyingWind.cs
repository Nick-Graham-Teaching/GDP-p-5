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

            float verticleSpeed = Vector3.Dot(PlayerBody.velocity, Vector3.up);
            float absVerticleSpeed = Mathf.Abs(verticleSpeed);

            if (absVerticleSpeed > SpeedLimitMin)
            {
                Emission.enabled = true;
                float speedRatio = Mathf.Min((absVerticleSpeed - SpeedLimitMin) / SpeedLimitDelta, 1.0f);
                Emission.rateOverTime = (EmissionRate.constant = EmissionRateMin + EmissionRateDelta * speedRatio);

                if (verticleSpeed > 0.0f)
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Flying_Ascent);
                else
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Flying_Descent);
            }
            else Emission.enabled = false;
        }
    }
}

