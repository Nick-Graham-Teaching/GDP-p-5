using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class PuzzleHintTriggerCallbackFunction : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UI_GameMessage.DisplayPuzzleHintTutorialMessage();
            }
        }
    }
}

