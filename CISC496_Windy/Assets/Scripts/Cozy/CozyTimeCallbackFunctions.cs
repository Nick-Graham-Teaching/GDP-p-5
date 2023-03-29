using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Windy.Cozy
{
    public class CozyTimeCallbackFunctions : MonoBehaviour
    {
        [SerializeField] Light[] Lights;

        public void TurnOffLight()
        {
            foreach (Light light in Lights)
            {
                light.enabled = false;
            }
        }

        public void TurnOnLight()
        {
            foreach (Light light in Lights)
            {
                light.enabled = true;
            }
        }
    }
}
