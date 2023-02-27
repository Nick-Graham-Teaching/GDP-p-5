using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySys : Singleton<EnergySys>
{

    public float MaxEnergy;
    float _energyDelta;

    public float Energy => Mathf.Max(0.0f, _energyDelta);

    public float RefillEnergy
    {
        set {
            _energyDelta = Mathf.Min(Mathf.Max(_energyDelta, 0.0f) + value, MaxEnergy);
        }
    }

    public bool ConsumeEnergy
    {
        get {
            return _energyDelta-- - 1.0f > -Mathf.Epsilon;
        }
    }

    void TakeOff(int way) {
        
    }

    void Start()
    {
        PlayerMotionModeManager.Instance.Takeoff += TakeOff;
    }
}
