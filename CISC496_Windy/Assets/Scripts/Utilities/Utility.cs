using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyUtility
{
    public static class Util
    {
        public static IEnumerator Timer(float CD, Action action)
        {
            yield return new WaitForSeconds(CD);
            action.Invoke();
        }
    }

    public class TakeOffException : Exception
    {
        public TakeOffException() { }
        public TakeOffException(string message) : base(message) { }
    }
}
