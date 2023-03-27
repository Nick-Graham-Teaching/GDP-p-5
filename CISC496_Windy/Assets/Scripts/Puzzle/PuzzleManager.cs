using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.Puzzle
{
    public enum PuzzleAnswerLetters
    {
        A,E,I,N,O,Q,S,T,U,Y,Reset,Delete
    }

    public class PuzzleManager : Singleton<PuzzleManager>
    {
        private string filePath = "Puzzle Letters/{0}";

        private Sprite letter_A;
        private Sprite letter_E;
        private Sprite letter_I;
        private Sprite letter_N;
        private Sprite letter_O;
        private Sprite letter_Q;
        private Sprite letter_S;
        private Sprite letter_T;
        private Sprite letter_U;
        private Sprite letter_Y;
        private Sprite Blank;

        public Image Letter1;
        public Image Letter2;
        public Image Letter3;
        public Image Letter4;
        public Image Letter5;
        public Image Letter6;

        public float WrongAnswerWarningColorChangeRate;

        private Image[] UI_Letters = new Image[6];
        private PuzzleAnswerLetters[] _playerAnswer = new PuzzleAnswerLetters[6];

        private readonly PuzzleAnswerLetters[] CorrectAnswer = 
        { 
            PuzzleAnswerLetters.Q,
            PuzzleAnswerLetters.U, 
            PuzzleAnswerLetters.E,
            PuzzleAnswerLetters.E,
            PuzzleAnswerLetters.N,
            PuzzleAnswerLetters.S
        };

        // Points to the lastest empty position
        private int _pointer;
        public int Pointer 
        {
            get
            {
                return _pointer;
            }
            set
            {
                _pointer = Mathf.Clamp(value, 0, 6);
            } 
        }

        public void ClearInput()
        {
            Pointer = 0;
            foreach (Image i in UI_Letters)
            {
                i.sprite = Blank;
            }
        }

        IEnumerator WrongAnswerWarnAnimation()
        {
            yield return new WaitUntil(() => Util.ImagesColorLerp(UI_Letters, WrongAnswerWarningColorChangeRate, Color.red));
            yield return new WaitUntil(() => Util.ImagesColorLerp(UI_Letters, WrongAnswerWarningColorChangeRate, Color.white));
            yield return new WaitUntil(() => Util.ImagesColorLerp(UI_Letters, WrongAnswerWarningColorChangeRate, Color.red));
            yield return new WaitUntil(() => Util.ImagesColorLerp(UI_Letters, WrongAnswerWarningColorChangeRate, Color.white));
            ClearInput();
        }

        public void DeleteOneInput()
        {
            UI_Letters[Pointer--].sprite = Blank;
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Delete);
        }

        public void Input(PuzzleAnswerLetters letter)
        {
            if (Pointer == 6)
            {
                return;
            }

            switch (letter)
            {
                case PuzzleAnswerLetters.A:
                    UI_Letters[Pointer].sprite = letter_A;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.A;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;
                case PuzzleAnswerLetters.E:
                    UI_Letters[Pointer].sprite = letter_E;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.E;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.I:
                    UI_Letters[Pointer].sprite = letter_I;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.I;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.N:
                    UI_Letters[Pointer].sprite = letter_N;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.N;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.O:
                    UI_Letters[Pointer].sprite = letter_O;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.O;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.Q:
                    UI_Letters[Pointer].sprite = letter_Q;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.Q;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.S:
                    UI_Letters[Pointer].sprite = letter_S;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.S;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.T:
                    UI_Letters[Pointer].sprite = letter_T;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.T;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.U:
                    UI_Letters[Pointer].sprite = letter_U;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.U;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;               
                case PuzzleAnswerLetters.Y:
                    UI_Letters[Pointer].sprite = letter_Y;
                    _playerAnswer[Pointer++] = PuzzleAnswerLetters.Y;
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Input);
                    break;
                case PuzzleAnswerLetters.Reset:
                    ClearInput();
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_Clear);
                    break;
            }

            if (Pointer == 6)
            {
                CheckPuzzleInput();
            }
        }

        void CheckPuzzleInput()
        {
            for (int i = 0; i < 6; i++)
            {
                if (_playerAnswer[i] != CorrectAnswer[i])
                {
                    StartCoroutine(WrongAnswerWarnAnimation());
                    Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_WrongAnswer);
                    return;
                }
            }

            // Win the game;
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Puzzle_CorrectAnswer);
            ClearInput();
            UI.UI_GameMessage.DisplayPuzzleSolvedMessage();
        }

        private new void Awake()
        {
            base.Awake();

            letter_A = Resources.Load<Sprite>(string.Format(filePath, "A"));
            letter_E = Resources.Load<Sprite>(string.Format(filePath, "E"));
            letter_I = Resources.Load<Sprite>(string.Format(filePath, "I"));
            letter_N = Resources.Load<Sprite>(string.Format(filePath, "N"));
            letter_O = Resources.Load<Sprite>(string.Format(filePath, "O"));
            letter_Q = Resources.Load<Sprite>(string.Format(filePath, "Q"));
            letter_S = Resources.Load<Sprite>(string.Format(filePath, "S"));
            letter_T = Resources.Load<Sprite>(string.Format(filePath, "T"));
            letter_U = Resources.Load<Sprite>(string.Format(filePath, "U"));
            letter_Y = Resources.Load<Sprite>(string.Format(filePath, "Y"));

            Blank    = Resources.Load<Sprite>(string.Format(filePath, "Blank"));

            UI_Letters[0] = Letter1;
            UI_Letters[1] = Letter2;
            UI_Letters[2] = Letter3;
            UI_Letters[3] = Letter4;
            UI_Letters[4] = Letter5;
            UI_Letters[5] = Letter6;
        }
    }
}
