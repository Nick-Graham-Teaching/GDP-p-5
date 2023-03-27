using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public class StaticSingleton<T> : MonoBehaviour where T : StaticSingleton<T>
    {
        protected static T Instance { get; set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
