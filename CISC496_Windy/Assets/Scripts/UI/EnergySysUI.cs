using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySysUI : MonoBehaviour
{

    Image FillImage;
    float fillAmount;
    public float decreaseRate;


    private void Update()
    {
        FillImage.fillAmount = Mathf.Lerp(FillImage.fillAmount, fillAmount, decreaseRate * Time.deltaTime);
    }

    private void Start()
    {
        FillImage = GetComponent<Image>();
        fillAmount = FillImage.fillAmount;

        EnergySys.Instance.EnergyChanged += (a) => fillAmount = a;
        GameEvents.OnToStartPage += ResetStatus;
    }
    void ResetStatus() => FillImage.fillAmount = 1.0f;
}
