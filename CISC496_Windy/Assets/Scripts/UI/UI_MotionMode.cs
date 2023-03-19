using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.UI
{
    public class UI_MotionMode : MonoBehaviour
    {
        public float _characterStayTime;
        public float _diminishRate;

        public float _colorChangeRate;

        public Image Image_MotionModeCircle;
        public Animation Anim_MotionModeCircle;

        public Image Image_Walk;
        public Image Image_Glide;
        public Image Image_Dive;
        public Image Image_Takeoff;
        public Image Image_Trapped;

        public Image Character_Walk;
        public Image Character_Glide;
        public Image Character_Dive;
        public Image Character_Takeoff;
        public Image Character_Trapped;

        private Coroutine CharacterDiminishCoroutine;
        private Dictionary<Image, Image> MotionModeImages;

        IEnumerator CharacterImageFadeOut(Image character)
        {
            Util.ResetImageAlpha(character, 1.0f);
            // If not in game, then don't diminish, and keep the stay time unchanged
            yield return new WaitUntil(() => 
            {
                if (Game.GameProgressManager.Instance.GameState is null)
                {
                    return false;
                }
                return Game.GameProgressManager.Instance.GameState.IsInGame();
            });
            yield return new WaitForSeconds(_characterStayTime);
            // If not in game, then don't diminish
            yield return new WaitUntil(() => 
                Game.GameProgressManager.Instance.GameState.IsInGame() && Util.ImageFadeOut(character, _diminishRate)
            );
        }

        IEnumerator OnOutOfTrappedMode(Image i)
        {
            yield return new WaitUntil(() => Util.ImageColorLerp(i, _colorChangeRate, Color.white));
        }
        IEnumerator OnOutOfTrappedMode(IEnumerable<Image> images)
        {
            yield return new WaitUntil(() => Util.ImagesColorLerp(images, _colorChangeRate, Color.white));
        }

        private void Start()
        {

            MotionModeImages = new Dictionary<Image, Image>()
            {
                {Image_Walk,    Character_Walk },
                {Image_Glide,   Character_Glide },
                {Image_Dive,    Character_Dive },
                {Image_Takeoff, Character_Takeoff },
                {Image_Trapped, Character_Trapped }
            };

            Util.ResetImagesAlpha(MotionModeImages.Values, 0.0f);

            UIEvents.OnToWalkMode += () =>
            {
                //Util.ResetImagesAlpha(MotionModeImages.Values, 0.0f);
                foreach (Image image in MotionModeImages.Keys)
                {
                    if (image == Image_Walk)
                    {
                        image.enabled = true;
                        MotionModeImages[image].enabled = true;

                        if (CharacterDiminishCoroutine is not null) StopCoroutine(CharacterDiminishCoroutine);
                        CharacterDiminishCoroutine = StartCoroutine(CharacterImageFadeOut(MotionModeImages[image]));
                    }
                    else
                    {
                        image.enabled = false;
                        MotionModeImages[image].enabled = false;
                    }
                }
            };
            UIEvents.OnToGlideMode += () =>
            {
                //Util.ResetImagesAlpha(MotionModeImages.Values, 0.0f);
                foreach (Image image in MotionModeImages.Keys)
                {
                    if (image == Image_Glide)
                    {
                        image.enabled = true;
                        MotionModeImages[image].enabled = true;

                        if (CharacterDiminishCoroutine is not null) StopCoroutine(CharacterDiminishCoroutine);
                        CharacterDiminishCoroutine = StartCoroutine(CharacterImageFadeOut(MotionModeImages[image]));
                    }
                    else
                    {
                        image.enabled = false;
                        MotionModeImages[image].enabled = false;
                    }
                }
            };
            UIEvents.OnToDiveMode += () =>
            {
                //Util.ResetImagesAlpha(MotionModeImages.Values, 0.0f);
                foreach (Image image in MotionModeImages.Keys)
                {
                    if (image == Image_Dive)
                    {
                        image.enabled = true;
                        MotionModeImages[image].enabled = true;

                        if (CharacterDiminishCoroutine is not null) StopCoroutine(CharacterDiminishCoroutine);
                        CharacterDiminishCoroutine = StartCoroutine(CharacterImageFadeOut(MotionModeImages[image]));
                    }
                    else
                    {
                        image.enabled = false;
                        MotionModeImages[image].enabled = false;
                    }
                }
            };
            UIEvents.OnToTakeoffMode += () =>
            {
                //Util.ResetImagesAlpha(MotionModeImages.Values, 0.0f);
                foreach (Image image in MotionModeImages.Keys)
                {
                    if (image == Image_Takeoff)
                    {
                        image.enabled = true;
                        MotionModeImages[image].enabled = true;

                        if (CharacterDiminishCoroutine is not null) StopCoroutine(CharacterDiminishCoroutine);
                        CharacterDiminishCoroutine = StartCoroutine(CharacterImageFadeOut(MotionModeImages[image]));
                    }
                    else
                    {
                        image.enabled = false;
                        MotionModeImages[image].enabled = false;
                    }
                }
            };
            UIEvents.OnToTrappedMode += () =>
            {
                //Util.ResetImagesAlpha(MotionModeImages.Values, 0.0f);
                foreach (Image image in MotionModeImages.Keys)
                {
                    if (image == Image_Trapped)
                    {
                        image.enabled = true;
                        MotionModeImages[image].enabled = true;

                        Anim_MotionModeCircle.Play();
                        Util.ResetImageColor(Image_MotionModeCircle, Color.red);
                        Util.ResetImageColor(image, Color.red);
                        Util.ResetImageColor(MotionModeImages[image], Color.red);

                        if (CharacterDiminishCoroutine is not null) StopCoroutine(CharacterDiminishCoroutine);
                        CharacterDiminishCoroutine = StartCoroutine(CharacterImageFadeOut(MotionModeImages[image]));
                    }
                    else
                    {
                        image.enabled = false;
                        MotionModeImages[image].enabled = false;
                    }
                }
            };
            UIEvents.OnOutOfTrappedMode += () =>
            {
                Anim_MotionModeCircle.Stop();
                Util.ResetImagesColor(MotionModeImages.Keys, Color.red);
                Util.ResetImagesColor(MotionModeImages.Values, Color.red);
                Util.ResetImageColor(Image_MotionModeCircle, Color.red);
                StartCoroutine(OnOutOfTrappedMode(MotionModeImages.Keys));
                StartCoroutine(OnOutOfTrappedMode(MotionModeImages.Values));
                StartCoroutine(OnOutOfTrappedMode(Image_MotionModeCircle));
            };
        }
    }
}

