using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Windy.UI
{
    public class UI_TutorialCallbackFunctions : MonoBehaviour, IPointerDownHandler
    {

        public void OnPointerDown(PointerEventData eventData)
        {
            Game.GameTutorialManager.TurnOffTut();
        }
    }
}

