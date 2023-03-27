using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Audio
{
    public class AudioPlayer : Singleton<AudioPlayer>
    {

        private static AudioCollection _audioCIns;
        private static AudioCollection AudioCIns
        {
            get
            {
                if (_audioCIns is null && AudioCollection.Instance is not null)
                {
                    _audioCIns = AudioCollection.Instance;
                }
                return _audioCIns;
            }
        }

        private void Start()
        {
            AudioCIns.Initialize();

            PlayLoopRandomly(AudioClip.Music_Hangdrum);
        }

        IEnumerator PlayLoop(AudioClip clip)
        {
            Audio audio = AudioCIns.Audios[clip];
            while (Application.isPlaying)
            {
                UnityEngine.AudioClip audioClip =  audio.PlayRandomly();
                yield return new WaitForSeconds(audioClip.length + 0.5f);
            }
        }
        public static void PlayLoopRandomly(AudioClip clip)
        {
            Instance.StartCoroutine(Instance.PlayLoop(clip));
        }

        public static void PlaydOneTimeRandomly(AudioClip clip)
        {
            if (AudioCIns.Audios.ContainsKey(clip))
            {
                AudioCIns.Audios[clip].PlayRandomly();
            }
            else
                throw new AudioException(string.Format("{0} Not Found in Audios Dictionary", clip.ToString()));
        }

    }
}
