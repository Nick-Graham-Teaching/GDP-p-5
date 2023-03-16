using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Windy
{
    public class Button : UnityEngine.UI.Selectable
    {
        [Serializable]
        public class ButtonPressEvent : UnityEvent { }

        [FormerlySerializedAs("onPress")]
        [SerializeField]
        private ButtonPressEvent _onPress;
        public ButtonPressEvent OnPress
        {
            get => _onPress;
            set => _onPress = value;
        }

        [Serializable]
        public class ButtonPressEndEvent : UnityEvent { }

        [FormerlySerializedAs("onPressEnd")]
        [SerializeField]
        private ButtonPressEndEvent _onPressEnd;
        public ButtonPressEndEvent OnPressEnd
        {
            get => _onPressEnd;
            set => _onPressEnd = value;
        }

        [Serializable]
        public class ButtonClickEvent : UnityEvent { }

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickEvent _onClick;
        public ButtonClickEvent OnClick
        {
            get => _onClick;
            set => _onClick = value;
        }

        [Serializable]
        public class ButtonClickEndEvent : UnityEvent { }

        [FormerlySerializedAs("onClickEnd")]
        [SerializeField]
        private ButtonClickEndEvent _onClickEnd;
        public ButtonClickEndEvent OnClickEnd
        {
            get => _onClickEnd;
            set => _onClickEnd = value;
        }


        private bool _isPointerDown;
        private bool _isPointerUp;
        private float _prePointerDownTime;
        private float PressCD;

        private bool _isPress;

        protected Button()
        {
            _isPointerDown = false;
            _isPointerUp = false;
            _isPress = false;
            _prePointerDownTime = float.NegativeInfinity;
            PressCD = 0.3f;

            OnPress = new ButtonPressEvent();
            OnPressEnd = new ButtonPressEndEvent();
            OnClick = new ButtonClickEvent();
            OnClickEnd = new ButtonClickEndEvent();
        }

        private void Update()
        {
            CheckLongPress();
        }

        void CheckLongPress()
        {
            if (_isPointerDown)
            {
                if (Time.time > (_prePointerDownTime + PressCD))
                {
                    _isPress = true;
                    OnPress?.Invoke();
                }
            }
            else if (_isPointerUp)
            {
                if (Time.time <= (_prePointerDownTime + PressCD))
                {
                    _isPress = false;
                    OnClick?.Invoke();
                    StartCoroutine(ClickEndMessage());
                }
                _isPointerUp = false;
            }
        }

        IEnumerator ClickEndMessage()
        {
            yield return null;
            OnClickEnd?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _isPointerDown = true;
            _prePointerDownTime = Time.time;
        }

        //public override void OnPointerUp(PointerEventData eventData)
        //{
        //    base.OnPointerUp(eventData);
        //    GameObject.Find("text1").GetComponent<UnityEngine.UI.Text>().text = "up";
        //    if (_isPress)
        //    {
        //        //Debug.Log("PressEnd");
        //        OnPressEnd?.Invoke();
        //        _isPress = false;
        //    }
        //    _isPointerDown = false;
        //    _isPointerUp = true;
        //}

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (_isPress)
            {
                OnPressEnd?.Invoke();
                _isPress = false;
            }
            _isPointerDown = false;
            _isPointerUp = true;
        }
    }
}
