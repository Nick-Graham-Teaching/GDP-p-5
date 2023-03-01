using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyTutorial : MonoBehaviour
{
    public Image IMG_1823;
    public AudioSource audioSource;
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;

    private bool isShowing = false;

    void Start()
    {
        IMG_1823.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isShowing)
            {
                StartCoroutine(FadeIn());
                isShowing = true;
                Invoke("HideImage", 5f + fadeInTime);
            }
        }
    }

    IEnumerator FadeIn()
    {
        float alpha = 0.0f;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime / fadeInTime;
            IMG_1823.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float alpha = 1.0f;
        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime / fadeOutTime;
            IMG_1823.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
        IMG_1823.gameObject.SetActive(false);
        isShowing = false;
    }

    void HideImage()
    {
        StartCoroutine(FadeOut());
    }
}
