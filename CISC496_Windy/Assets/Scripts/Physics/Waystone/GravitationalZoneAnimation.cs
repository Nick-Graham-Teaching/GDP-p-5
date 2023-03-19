using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Waystone
{
    public class GravitationalZoneAnimation : MonoBehaviour
    {
        public float MaxRadius;
        public float MinRadius;

        public float radiusBias;
        private float _fractionDelta;
        public float ChangeRate;

        private void Start()
        {
            _fractionDelta = MinRadius;
            transform.localScale = new Vector3(MinRadius, MinRadius, MinRadius);
            StartCoroutine(Enlarge());


            //fractionDelta = MaxRadius;
            //transform.localScale = new Vector3(MaxRadius, MaxRadius, MaxRadius);
            //StartCoroutine(Shrink());
        }

        IEnumerator Enlarge()
        {
            yield return new WaitUntil(() =>
            {
                _fractionDelta = Mathf.Lerp(_fractionDelta, MaxRadius, ChangeRate * Time.deltaTime);
                transform.localScale = new Vector3(_fractionDelta, _fractionDelta, _fractionDelta);
                return _fractionDelta >= MaxRadius - radiusBias;
            });

            StartCoroutine(Shrink());
        }
        IEnumerator Shrink()
        {
            yield return new WaitUntil(() =>
            {
                _fractionDelta = Mathf.Lerp(_fractionDelta, MinRadius, ChangeRate * Time.deltaTime);
                transform.localScale = new Vector3(_fractionDelta, _fractionDelta, _fractionDelta);
                return _fractionDelta <= MinRadius + radiusBias;
            });

            StartCoroutine(Enlarge());
        }
    }
}

