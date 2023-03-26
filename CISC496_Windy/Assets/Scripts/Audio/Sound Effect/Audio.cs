using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Audio
{
    [System.Serializable]
    public class Audio
    {
        [SerializeField] UnityEngine.AudioClip[] clips;

        [SerializeField]
        [Range(0f, 5f)] float volume = 1.0f;
        [SerializeField]
        [Range(0f, 1f)] float pitch = 1.0f;
        [SerializeField]
        [Range(0f, 10f)] float playCD = 0f;
        float lastTimePlayed;

        private AudioSource _audioSource;
        public AudioSource AudioSource
        {
            set
            {
                if (_audioSource is null)
                {
                    _audioSource = value;
                }
            }
            get => _audioSource;
        }

        public UnityEngine.AudioClip PlayRandomly()
        {
            if (Time.time - lastTimePlayed < playCD) return null;

            if (clips.Length > 1)
            {
                AudioSource.clip = clips[Random.Range(0, clips.Length - 1)];
            }
            else if (clips.Length == 1) AudioSource.clip = clips[0];
            else throw new AudioException("Empty Clip Array, No AudioClip To Play");

            lastTimePlayed = Time.time;
            AudioSource.volume = volume;
            AudioSource.pitch = pitch;
            AudioSource.PlayOneShot(AudioSource.clip);

            return AudioSource.clip;
        }
    }
}
