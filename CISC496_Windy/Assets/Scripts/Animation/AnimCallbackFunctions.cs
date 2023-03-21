using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Animation
{
    public class AnimCallbackFunctions : MonoBehaviour
    {
        #region Mobile Phone UI
        public void OnChooseCloudAnim(int currentCloud)
        {
            UI.MP_RandomCloudGenerator.Instance.OnCloudGeneration(currentCloud);
        }

        #endregion
    }
}

