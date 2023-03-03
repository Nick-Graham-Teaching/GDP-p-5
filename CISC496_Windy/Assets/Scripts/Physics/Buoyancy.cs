using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public class Buoyancy : Singleton<Buoyancy>
{
    [Foldout("Dive Floating Accel", true)]
    float diveUpwardAccel;
    [SerializeField]
    private float MinDiveUpwardAccel;
    [SerializeField]
    private float MaxDiveUpwardAccel;

    [Foldout("Glide Floating Accel", true)]
    float glideUpwardAccel;
    [SerializeField]
    private float MinGlideUpwardAccel;
    [SerializeField]
    private float MaxGlideUpwardAccel;
    [SerializeField]
    private float PunishGlideUpwardAccel;
    [SerializeField]
    private float UpwardAccelSpeedUpRate;

    [Foldout("Glide Upforce", true)]
    float upForceDeltaTime;
    [SerializeField]
    private float UpForceMaxUtilityTime;
    [SerializeField]
    private float DeltaTimeRecoverRate;
    [SerializeField]
    private float PunishmentCD;

    bool glideFloatSupervisorOn;

    private float cloudUpwardAccel;
    public float CloudUpwardAccel { private get => cloudUpwardAccel; set => cloudUpwardAccel = value; }

    public float Force
    {
        get {
            return (PlayerMotionModeManager.Instance.MotionMode == PlayerMotionMode.DIVE ? diveUpwardAccel : glideUpwardAccel)
                + CloudUpwardAccel;
        }
    }


    IEnumerator GlideUpForceTimer()
    {
        yield return new WaitUntil(
                () => {
                    upForceDeltaTime += Time.deltaTime;
                    glideUpwardAccel = Mathf.Lerp(glideUpwardAccel, MaxGlideUpwardAccel, UpwardAccelSpeedUpRate * Time.deltaTime);
                    return !Input.GetKey(Keys.UpCode) || upForceDeltaTime >= UpForceMaxUtilityTime;
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
                    return Input.GetKeyDown(Keys.UpCode) || upForceDeltaTime < Mathf.Epsilon;
                }
            );
        }

        glideFloatSupervisorOn = false;
    }

    private void Update()
    {
        switch (PlayerMotionModeManager.Instance.MotionMode)
        {
            case PlayerMotionMode.GLIDE:
                if (!glideFloatSupervisorOn && KIH.Instance.GetKeyPress(Keys.UpCode) && upForceDeltaTime < UpForceMaxUtilityTime)
                {
                    StartCoroutine(GlideUpForceTimer());
                    glideFloatSupervisorOn = true;
                }
                break;
            case PlayerMotionMode.DIVE:
                diveUpwardAccel = KIH.Instance.GetKeyPress(Keys.UpCode) ? MaxDiveUpwardAccel : MinDiveUpwardAccel;
                break;
        }
    }

    private void Start()
    {
        glideUpwardAccel = MinGlideUpwardAccel;
    }
}
