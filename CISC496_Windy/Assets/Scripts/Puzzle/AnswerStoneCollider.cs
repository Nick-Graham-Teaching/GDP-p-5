using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Puzzle
{
    public class AnswerStoneCollider : MonoBehaviour
    {
        public PuzzleAnswerLetters letter;

        private float InputCD = 1.5f;
        private bool flag = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (flag && collision.gameObject.CompareTag("Player"))
            {
                PuzzleManager.Instance.Input(letter);
                flag = false;
                StartCoroutine(Util.Timer(InputCD, () => flag = true));
            }
        }
    }
}

