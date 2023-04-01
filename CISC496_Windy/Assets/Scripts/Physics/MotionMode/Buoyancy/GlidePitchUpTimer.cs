using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Buoyancy
{
    public class GlidePitchUpTimer : StaticSingleton<GlidePitchUpTimer>
    {
        float upForceDeltaTime;
        [SerializeField] float UpForceMaxUtilityTime;

        [SerializeField] float DeltaTimeRecoverRate;
        [SerializeField] float _punishmentCD;
        public static float PunishmentCD => Instance._punishmentCD;

        public static bool IsBackToZero => Instance.upForceDeltaTime < Mathf.Epsilon;
        public static bool IsExceedingTimeLimit => Instance.upForceDeltaTime >= Instance.UpForceMaxUtilityTime;

        public static float UseTimeRatio => Mathf.Clamp01(1.0f - (Instance.upForceDeltaTime / Instance.UpForceMaxUtilityTime));

        public static void IncrementUseTime(float delta)
        {
            Instance.upForceDeltaTime += delta;
        }
        public static void DecrementUseTime()
        {
            Instance.upForceDeltaTime = Mathf.Lerp(Instance.upForceDeltaTime, 0.0f, Instance.DeltaTimeRecoverRate * Time.deltaTime);
        }
        public static void ResetUseTime()
        {
            Instance.upForceDeltaTime = 0.0f;
        }
    }
}

