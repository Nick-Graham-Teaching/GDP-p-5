using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Windy;

public class TouchPhaseExample : MonoBehaviour
{

    private void Start()
    {
        Windy.UI.UI_Puzzle.Instance.Input(Windy.UI.PuzzleAnswerLetters.Q);
        Windy.UI.UI_Puzzle.Instance.Input(Windy.UI.PuzzleAnswerLetters.A);
        Windy.UI.UI_Puzzle.Instance.Input(Windy.UI.PuzzleAnswerLetters.Y);
        Windy.UI.UI_Puzzle.Instance.Input(Windy.UI.PuzzleAnswerLetters.O);
        Windy.UI.UI_Puzzle.Instance.Input(Windy.UI.PuzzleAnswerLetters.N);
    }
}