using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class UIEvents
{
    public static Action OnStartPressed;
    public static Action OnToWalkMode;
    public static Action OnToGlideMode;
    public static Action OnToDiveMode;
}

public class UIEventsHandler : Singleton<UIEventsHandler>
{

    public GameObject StartPageUI;
    public GameObject InGameUI;
    public float UIFadeOutRate;
    public float UIFadeInRate;

    Image[] StartPageUIImages;
    Button[] StartPageUIButtons;
    Image[] InGameUIImages;

    public Image TutorialImage;
    public float tutorialStayTime;

    public GameObject WalkMode;
    public GameObject GlideMode;
    public GameObject DiveMode;

    IEnumerator StartPageUIFadeOut()
    {
        // Disable all buttons
        foreach (Button b in StartPageUIButtons)
        {
            b.enabled = false;
        }
        yield return new WaitUntil(() =>
        {
            // Decrease alpha of all images
            foreach (Image i in StartPageUIImages)
            {
                Color c = i.color;
                i.color = Color.Lerp(c, new Color(c.r, c.g, c.b, 0.0f), UIFadeOutRate * Time.deltaTime);
            }
            // While doing the camera animation
            return GameProgressManager.Instance.CameraAnimationTransition();
        }
        );
        // Enable all player and camera control files
        GameEvent.OnStartPressed?.Invoke();
        StartCoroutine(InGameUIFadeIn());
    }

    public IEnumerator InGameUIFadeIn() {
        float alpha = 0.0f;
        yield return new WaitUntil(() =>
        {
            foreach (Image i in InGameUIImages)
            {
                Color c = i.color;
                alpha = Mathf.Lerp(alpha, 1.0f, UIFadeInRate * Time.deltaTime);
                i.color = new Color(c.r, c.g, c.b, alpha);
            }
            return alpha >= 1.0f - 0.005f;
        });
        yield return new WaitForSeconds(tutorialStayTime);
        alpha = 1.0f;
        yield return new WaitUntil(() => {
            Color c = TutorialImage.color;

            alpha = Mathf.Lerp(alpha, 0.0f, UIFadeOutRate * Time.deltaTime);
            TutorialImage.color = new Color(c.r, c.g, c.b, alpha);
            return alpha <= 0.0f + 0.005f;
        });
        TutorialImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartPageUIImages = StartPageUI.GetComponentsInChildren<Image>();
        StartPageUIButtons = StartPageUI.GetComponentsInChildren<Button>();
        InGameUIImages = InGameUI.GetComponentsInChildren<Image>();

        UIEvents.OnStartPressed += () => { 
            StartCoroutine(StartPageUIFadeOut()); 
        };

        UIEvents.OnToWalkMode  += () => {
            WalkMode.GetComponent<Image>().enabled = true;
            GlideMode.GetComponent<Image>().enabled = false;
            DiveMode.GetComponent<Image>().enabled = false;
        };
        UIEvents.OnToGlideMode += () => {
            GlideMode.GetComponent<Image>().enabled = true;
            WalkMode.GetComponent<Image>().enabled = false;
            DiveMode.GetComponent<Image>().enabled = false;
        };
        UIEvents.OnToDiveMode  += () => {
            DiveMode.GetComponent<Image>().enabled = true;
            GlideMode.GetComponent<Image>().enabled = false;
            WalkMode.GetComponent<Image>().enabled = false;
        };

        UIEvents.OnToWalkMode?.Invoke();
    }
}
