using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Buoyancy
{
    public class B_Glide : Buoyancy
    {
        float glideUpwardAccel;
        protected internal float MinGlideUpwardAccel;
        protected internal float MaxGlideUpwardAccel;
        protected internal float PunishGlideUpwardAccel;
        protected internal float UpwardAccelSpeedUpRate;

        float upForceDeltaTime;
        protected internal float UpForceMaxUtilityTime;
        protected internal float DeltaTimeRecoverRate;
        protected internal float PunishmentCD;

        bool glideFloatSupervisorOn;

        public sealed override float Force => glideUpwardAccel + CloudUpwardAccel + PunishUpwardAccel;


        IEnumerator GlideUpForceTimer()
        {
            yield return new WaitUntil(
                    () => {
                        if (Game.GameProgressManager.Instance.GameState.IsInGame())
                        {
                            float degree;
                            if (!Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out degree))
                            {
                                return true;
                            }
                            if (degree < Mathf.Epsilon) return true;
                            GlidePitchUpTimer.IncrementUseTime(degree * Time.deltaTime);
                            glideUpwardAccel = Mathf.Lerp(glideUpwardAccel, MaxGlideUpwardAccel, degree * UpwardAccelSpeedUpRate * Time.deltaTime);
                        }
                        return !MM_Executor.Instance.MotionMode.IsGlide() || GlidePitchUpTimer.IsExceedingTimeLimit;
                    }
                );

            glideUpwardAccel = MinGlideUpwardAccel;

            if (GlidePitchUpTimer.IsExceedingTimeLimit)
            {
                UI.UI_GlidePitchUpTimer.ResetImageFillAmount();
                UI.UI_GameMessage.DisplayGlidePunishmentTutorialMessage();
                PunishUpwardAccel = PunishGlideUpwardAccel;
                float timer = 0;
                yield return new WaitUntil(() => {
                    if (Game.GameProgressManager.Instance.GameState.IsInGame()) timer += Time.deltaTime;
                    return timer > GlidePitchUpTimer.PunishmentCD;
                });
                PunishUpwardAccel = 0.0f;
                GlidePitchUpTimer.ResetUseTime();
            }
            else
            {
                yield return new WaitUntil(
                    () => {
                        if (Game.GameProgressManager.Instance.GameState.IsInGame()) GlidePitchUpTimer.DecrementUseTime();
                        return (MM_Executor.Instance.MotionMode.IsGlide() && Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out float _)) 
                        || GlidePitchUpTimer.IsBackToZero;
                    }
                );
            }

            glideFloatSupervisorOn = false;
        }


        public sealed override void Update()
        {
            base.Update();
            if (!glideFloatSupervisorOn && Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out float _) && !GlidePitchUpTimer.IsExceedingTimeLimit)
            {
                MM_Executor.Instance.StartCoroutine(GlideUpForceTimer());
                glideFloatSupervisorOn = true;
            }
        }
        public sealed override void Start()
        {
            glideUpwardAccel = MinGlideUpwardAccel;
        }

        public override string ToString() => "Buoyancy -- Glide";
    }
}
