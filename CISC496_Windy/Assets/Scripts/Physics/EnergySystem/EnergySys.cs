using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySys : Singleton<EnergySys>
{
    float _energyDelta;
    [SerializeField]
    float MaxEnergy;
    [SerializeField]
    float rechargeSpeed;

    [SerializeField]
    float toDiveConsumption;
    [SerializeField]
    float toGlideConsumption;

    public event Action<float> EnergyChanged;

    public float Energy 
    {
        set 
        {
            _energyDelta = Mathf.Min(value, MaxEnergy);
            EnergyChanged?.Invoke(_energyDelta / MaxEnergy);
        }
        get
        {
            return _energyDelta;
        }
    }

    public void RechargeEnergy(float rate = 1.0f) {
        Energy += rate * rechargeSpeed;
    }

    public bool ConsumeEnergy(float consumption = 1.0f) {
        consumption = Mathf.Clamp(consumption, 0.0f, 1.0f);
        if (Energy - consumption > -Mathf.Epsilon)
        {
            Energy -= consumption;
            return true;
        }
        Debug.LogError(new MyUtility.TakeOffException("Energy is not enough!"));
        return false;
    }

    void onTakeOff(int way)
    {
        switch (way)
        {
            case 0b001:
                if (!ConsumeEnergy(toDiveConsumption)) 
                {
                    throw new MyUtility.TakeOffException("Energy is not enough!");
                }
                break;
            case 0b100:
                if (!ConsumeEnergy(toGlideConsumption))
                {
                    throw new MyUtility.TakeOffException("Energy is not enough!");
                }
                break;
        }
    }

    void Start()
    {
        PlayerMotionModeManager.Instance.Takeoff += onTakeOff;

        EnergyChanged += (a) => { Debug.Log("Energy left: " + a); };
        Energy = MaxEnergy;
    }
}


