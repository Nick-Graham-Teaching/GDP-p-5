using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class MP_PanelSelfAdaption : Singleton<MP_PanelSelfAdaption>
    {
        public RectTransform panelTransform;

        private new void Awake()
        {
            if (panelTransform is null)
            {
                panelTransform = GetComponent<RectTransform>();
            }

            panelTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
    }
}

