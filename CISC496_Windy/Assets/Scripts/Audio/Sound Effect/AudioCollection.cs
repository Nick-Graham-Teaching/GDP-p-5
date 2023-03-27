using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Windy.Audio
{
    public class AudioCollection : Singleton<AudioCollection>
    {
        [System.Serializable]
        public class AudioDictionary : SerializableDictionary<AudioClip, Audio> { }

        public AudioDictionary Audios;

        public void Initialize()
        {
            foreach (AudioClip audio in Audios.Keys)
            {
                GameObject gameObj = new(audio.ToString());
                gameObj.transform.SetParent(transform);
                Audios[audio].AudioSource = gameObj.AddComponent<AudioSource>();
                Audios[audio].AudioSource.playOnAwake = false;
            }
        }
    }
}

