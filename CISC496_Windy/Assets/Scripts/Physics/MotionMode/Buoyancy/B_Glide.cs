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

        public sealed override float Force => glideUpwardAccel + CloudUpwardAccel;


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
                            upForceDeltaTime += (degree * Time.deltaTime);
                            glideUpwardAccel = Mathf.Lerp(glideUpwardAccel, MaxGlideUpwardAccel, degree * UpwardAccelSpeedUpRate * Time.deltaTime);
                        }
                        return upForceDeltaTime >= UpForceMaxUtilityTime;
                    }
                );

            if (upForceDeltaTime >= UpForceMaxUtilityTime)
            {
                glideUpwardAccel = PunishGlideUpwardAccel;
                yield return new WaitForSeconds(PunishmentCD);
                glideUpwardAccel = MinGlideUpwardAccel;
                upForceDeltaTime = 0.0f;
            }
            else
            {
                glideUpwardAccel = MinGlideUpwardAccel;
                yield return new WaitUntil(
                    () => {
                        upForceDeltaTime = Mathf.Lerp(upForceDeltaTime, 0.0f, DeltaTimeRecoverRate * Time.deltaTime);
                        return Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out float _) || upForceDeltaTime < Mathf.Epsilon;
                    }
                );
            }

            glideFloatSupervisorOn = false;
        }


        public sealed override void Update()
        {
            base.Update();
            if (!glideFloatSupervisorOn && Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out float _) && upForceDeltaTime < UpForceMaxUtilityTime)
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
