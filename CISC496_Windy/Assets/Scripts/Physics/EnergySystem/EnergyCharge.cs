using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.EnergySystem
{
    public class EnergyCharge : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EnergySys.Instance.RechargeEnergy();
            }
        }
    }
}

