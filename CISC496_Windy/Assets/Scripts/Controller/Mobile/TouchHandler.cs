using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Controller
{
    public enum FingerType
    {
        Press, End, Available
    }

    public abstract class Finger
    {
        public int FingerID { get; set; }
        public Vector2 StartPos { get; set; }
        public FingerType Type { get; set; }

        public virtual Vector2 DeltaPosition { get; set; }

        public Finger(int ID, Vector2 pos)
        {
            FingerID = ID;
            StartPos = pos;
            Type = FingerType.Available;
        }
    }

    public class CameraFinger : Finger
    {
        public float GetAxisX()
        {
            return DeltaPosition.x / Screen.width * 720.0f;
        }

        public float GetAxisY()
        {
            return DeltaPosition.y / Screen.height * 240.0f;
        }

        public CameraFinger(int ID, Vector2 pos) : base(ID, pos) { }
    }

    public class PlayerFinger : Finger
    {
        private Vector2 _moveToPos;

        public override Vector2 DeltaPosition
        {
            get
            {
                Vector2 direction = _moveToPos - StartPos;
                if (direction.magnitude < Screen.height / 4.0f)
                {
                    return direction / (Screen.height / 4.0f);
                }

                return direction.normalized;
            }
            set => _moveToPos = value;
        }

        public bool GetUpKey(out float degree)
        {
            degree = DeltaPosition.y;
            if (degree >= -Mathf.Epsilon) return true;
            return false;
        }

        public bool GetDownKey(out float degree)
        {
            degree = DeltaPosition.y * -1.0f;
            if (degree >= -Mathf.Epsilon) return true;
            return false;
        }

        public bool GetLeftKey(out float degree)
        {
            degree = DeltaPosition.x * -1.0f;
            if (degree >= -Mathf.Epsilon) return true;
            return false;
        }

        public bool GetRightKey(out float degree)
        {
            degree = DeltaPosition.x * 1.0f;
            if (degree >= -Mathf.Epsilon) return true;
            return false;
        }

        public PlayerFinger(int ID, Vector2 pos) : base(ID, pos) { }
    }

    public class TouchHandler : MonoBehaviour
    {
        static CameraFinger _cameraFinger;
        static PlayerFinger _playerFinger;

        public static CameraFinger CameraFinger { get => _cameraFinger; }
        public static PlayerFinger PlayerFinger { get => _playerFinger; }

        void ProcessTouches()
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:

                            Vector2 startPos = touch.position;
                            if (_cameraFinger.Type == FingerType.Available && startPos.x > Screen.width / 2.0f)
                            {
                                _cameraFinger.FingerID = touch.fingerId;
                                _cameraFinger.DeltaPosition = touch.deltaPosition;
                                _cameraFinger.Type = FingerType.Press;
                            }
                            else if (_playerFinger.Type == FingerType.Available && startPos.x <= Screen.width / 2.0f)
                            {
                                _playerFinger.FingerID = touch.fingerId;
                                _playerFinger.StartPos = touch.position;
                                _playerFinger.Type = FingerType.Press;
                            }
                            break;

                        case TouchPhase.Moved or TouchPhase.Stationary:
                            if (touch.fingerId == _cameraFinger.FingerID)
                            {
                                _cameraFinger.DeltaPosition = touch.deltaPosition;
                            }
                            else if (touch.fingerId == _playerFinger.FingerID)
                            {
                                _playerFinger.DeltaPosition = touch.position;
                            }
                            break;

                        case TouchPhase.Ended:
                            if (touch.fingerId == _cameraFinger.FingerID)
                            {
                                _cameraFinger.DeltaPosition = Vector2.zero;
                                StartCoroutine(ResetFinger(_cameraFinger));
                            }
                            else if (touch.fingerId == _playerFinger.FingerID)
                            {
                                _playerFinger.DeltaPosition = _playerFinger.StartPos;
                                StartCoroutine(ResetFinger(_playerFinger));
                            }
                            break;
                    }
                }
            }
        }
        IEnumerator ResetFinger(Finger f)
        {
            f.Type = FingerType.End;
            yield return null;
            f.Type = FingerType.Available;
            f.FingerID = -1;
        }

        void Update()
        {
            ProcessTouches();
        }

        void Start()
        {
            _playerFinger = new PlayerFinger(-1, Vector2.zero);
            _cameraFinger = new CameraFinger(-1, Vector2.zero);
        }


        public static float GetCameraAxisX()
        {
            if (_cameraFinger.Type == FingerType.Available) return 0.0f;
            return _cameraFinger.GetAxisX();
        }
        public static float GetCameraAxisY()
        {
            if (_cameraFinger.Type == FingerType.Available) return 0.0f;
            return _cameraFinger.GetAxisY();
        }

        public static bool GetUpKey(out float degree)
        {
            if (_playerFinger.Type == FingerType.Available)
            {
                degree = 0.0f;
                return false;
            }
            //else if(_playerFinger.Type == FingerType.End)
            //{
            //    degree = 0.0f;
            //    return true;
            //}
            return _playerFinger.GetUpKey(out degree);
        }
        public static bool GetDownKey(out float degree)
        {
            if (_playerFinger.Type == FingerType.Available)
            {
                degree = 0.0f;
                return false;
            }
            //else if (_playerFinger.Type == FingerType.End)
            //{
            //    degree = 0.0f;
            //    return true;
            //}
            return _playerFinger.GetDownKey(out degree);
        }
        public static bool GetLeftKey(out float degree)
        {
            if (_playerFinger.Type == FingerType.Available)
            {
                degree = 0.0f;
                return false;
            }
            //else if (_playerFinger.Type == FingerType.End)
            //{
            //    degree = 0.0f;
            //    return true;
            //}
            return _playerFinger.GetLeftKey(out degree);
        }
        public static bool GetRightKey(out float degree)
        {
            if (_playerFinger.Type == FingerType.Available)
            {
                degree = 0.0f;
                return false;
            }
            //else if (_playerFinger.Type == FingerType.End)
            //{
            //    degree = 0.0f;
            //    return true;
            //}
            return _playerFinger.GetRightKey(out degree);
        }
    }
}

