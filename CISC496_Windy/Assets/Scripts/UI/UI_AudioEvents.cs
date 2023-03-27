using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Windy.UI
{
    public class UI_AudioEvents : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.UI_Button_Press);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.UI_Button_PointerEnter);
        }
    }
}

