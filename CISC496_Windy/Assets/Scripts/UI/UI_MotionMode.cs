using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.UI
{
    public class UI_MotionMode : MonoBehaviour
    {
        [SerializeField] Rigidbody PlayerBody;

        [SerializeField] float _characterStayTime;
        [SerializeField] float _diminishRate;

        [SerializeField] float _colorChangeRate;

        [SerializeField] Image Image_MotionModeCircle;
        [SerializeField] UnityEngine.Animation Anim_MotionModeCircle;
        AnimationState MotionModeCircle_CirclingAnimationState;

        [SerializeField] float MotionModeCircle_CirclingAnimationMinSpeed;
        [SerializeField] float MotionModeCircle_CirclingAnimationMaxSpeed;

        [SerializeField] Image Image_Walk;
        [SerializeField] Image Image_Glide;
        [SerializeField] Image Image_Dive;
        [SerializeField] Image Image_Takeoff;
        [SerializeField] Image Image_Trapped;

        [SerializeField] Image Character_Walk;
        [SerializeField] Image Character_Glide;
        [SerializeField] Image Character_Dive;
        [SerializeField] Image Character_Takeoff;
        [SerializeField] Image Character_Trapped;

        Coroutine CharacterDiminishCoroutine;
        Dictionary<Image, Image> MotionModeImages;

        private void Update()
        {
            if (Game.GameProgressManager.Instance.GameState.IsInGame())
            {
                MotionModeCircle_CirclingAnimationState.speed = Mathf.Clamp(Mathf.Abs(PlayerBody.velocity.y) / 20.0f, 
                    MotionModeCircle_CirclingAnimationMinSpeed, 
                    MotionModeCircle_CirclingAnimationMaxSpeed);
            }
        }

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

            MotionModeCircle_CirclingAnimationState = Anim_MotionModeCircle["Anim - MotionModeCircle - Rotation"];

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

                        Anim_MotionModeCircle.Play("Anim - MotionModeCircle - Twitch");
                        StopAllCoroutines();
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
                Anim_MotionModeCircle.Play("Anim - MotionModeCircle - Rotation");
                Util.ResetImagesColor(MotionModeImages.Keys, Color.red);
                Util.ResetImagesColor(MotionModeImages.Values, Color.red);
                Util.ResetImageColor(Image_MotionModeCircle, Color.red);
                StopAllCoroutines();
                StartCoroutine(OnOutOfTrappedMode(MotionModeImages.Keys));
                StartCoroutine(OnOutOfTrappedMode(MotionModeImages.Values));
                StartCoroutine(OnOutOfTrappedMode(Image_MotionModeCircle));
            };
        }
    }
}

