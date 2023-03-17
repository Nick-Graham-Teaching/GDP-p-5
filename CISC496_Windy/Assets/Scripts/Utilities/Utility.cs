using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Windy
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

    public class Matrix3
    {
        /**
         * {
         *      values[0] values[1] values[2]
         *      values[3] values[4] values[5]
         *      values[6] values[7] values[8]
         * }
         */
        private float[] values;

        public Vector3 Row1 
        {
            get
            {
                return new Vector3(values[0], values[1], values[2]);
            } 
        }
        public Vector3 Row2
        {
            get
            {
                return new Vector3(values[3], values[4], values[5]);
            }
        }
        public Vector3 Row3
        {
            get
            {
                return new Vector3(values[6], values[7], values[8]);
            }
        }
        public Vector3 Column1
        {
            get
            {
                return new Vector3(values[0], values[3], values[6]);
            }
        }
        public Vector3 Column2
        {
            get
            {
                return new Vector3(values[1], values[4], values[7]);
            }
        }
        public Vector3 Column3
        {
            get
            {
                return new Vector3(values[2], values[5], values[8]);
            }
        }
        public static Matrix3 Identity
        {
            get
            {
                return new Matrix3(1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f);
            }
        }

        public Matrix3()
        {
            values = new float[9];
        }
        public Matrix3(Vector3 v1, Vector3 v2, Vector3 v3, bool row = true)
        {
            values = new float[9];
            if (row)
            {
                values[0] = v1.x;
                values[1] = v1.y;
                values[2] = v1.z;

                values[3] = v2.x;
                values[4] = v2.y;
                values[5] = v2.z;

                values[6] = v3.x;
                values[7] = v3.y;
                values[8] = v3.z;
            }
            else
            {
                values[0] = v1.x;
                values[3] = v1.y;
                values[6] = v1.z;

                values[1] = v2.x;
                values[4] = v2.y;
                values[7] = v2.z;

                values[2] = v3.x;
                values[5] = v3.y;
                values[8] = v3.z;
            }
        }
        public Matrix3(params float[] values)
        {
            if (values.Length != 9)
            {
                throw new ArgumentOutOfRangeException("Matrix3 Constructor: Parameters Out of Range");
            }
            this.values = new float[9];
            for (int i = 0; i < 9; i++)
            {
                this.values[i] = values[i];
            }
        }
        public Matrix3(Matrix3 m3)
        {
            if (m3 is null)
            {
                throw new NullReferenceException("Matrix3 Constructor: Null Pointer");
            }
            values = new float[9];
            for (int i = 0; i < 9; i++)
            {
                values[i] = m3.values[i];
            }
        }
        public override string ToString()
        {
            return string.Format(

                "({0:f2}, {1:f2}, {2:f2}), ({3:f2}, {4:f2}, {5:f2}), ({6:f2}, {7:f2}, {8:f2})",
                values[0], values[1], values[2],
                values[3], values[4], values[5],
                values[6], values[7], values[8]

                );
        }

        public static Vector3 operator* (Matrix3 m, Vector3 v)
        {
            return new Vector3 (
                    
                    Vector3.Dot(m.Row1, v),
                    Vector3.Dot(m.Row2, v),
                    Vector3.Dot(m.Row3, v)

                );
        }
        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            return new Matrix3(
                
                    m1 * m2.Column1,
                    m1 * m2.Column2,
                    m1 * m2.Column3,
                    false

                );
        }
    }

    public class TakeOffException : Exception
    {
        public TakeOffException(string message) : base(message) { }
    }

    public class UnknownKeyException : Exception
    {
        public UnknownKeyException(string message) : base(message) { }
    }
}
