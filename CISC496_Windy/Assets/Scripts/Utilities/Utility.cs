using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


        public static void ResetImageAlpha(Image i, float alpha) 
        { 
            i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
        }
        public static void ResetImagesAlpha(Image[] images, float alpha) 
        {
            foreach (Image i in images) {
                ResetImageAlpha(i, alpha);
            }
        }

        public static bool ImageFadeIn(Image i, float rate, float alpha = 1.0f, float threshold = 0.005f) 
        {
            i.color = Color.Lerp(i.color, new Color(i.color.r, i.color.g, i.color.b, alpha), rate * Time.deltaTime);
            return i.color.a > alpha - threshold;
        }             
        public static bool ImageFadeOut(Image i, float rate, float alpha = 0.0f, float threshold = 0.005f)
        {
            i.color = Color.Lerp(i.color, new Color(i.color.r, i.color.g, i.color.b, alpha), rate * Time.deltaTime);
            return i.color.a < alpha + threshold;
        }
        public static bool ImagesFadeIn(Image[] images, float rate, float alpha = 1.0f, float threshold = 0.005f) 
        {
            float a = 0.0f;
            foreach (Image i in images) {
                ImageFadeIn(i, rate, alpha);
                a = i.color.a;
            }
            return a > alpha - threshold;
        }
        public static bool ImagesFadeOut(Image[] images, float rate, float alpha = 0.0f, float threshold = 0.005f)
        {
            float a = 0.0f;
            foreach (Image i in images)
            {
                ImageFadeOut(i, rate, alpha);
                a = i.color.a;
            }
            return a < alpha + threshold;
        }
    }

    public class TakeOffException : Exception
    {
        public TakeOffException(string message) : base(message) { }
    }
}
