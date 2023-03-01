using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyBar : MonoBehaviour
{
    public float maxEnergy = 20;
    public float currentEnergy = 0;
    public RectTransform fill;

    
    void Start()
    {
        UpdateFillHeight();
        fill.anchorMin = new Vector2(0.5f, 0f);
        fill.anchorMax = new Vector2(0.5f, 0f);
        fill.pivot = new Vector2(0.5f, 0f);
    }

    public void EnergyUp()
    {
        currentEnergy += 0.5f;
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        UpdateFillHeight();
    }

    public void EnergyDown()
    {
        currentEnergy -= 0.5f;
        if (currentEnergy < 0)
            currentEnergy = 0;

        UpdateFillHeight();
    }

    void UpdateFillHeight()
    {
        float percentage = (float)currentEnergy / maxEnergy;
        fill.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height * percentage);
    }
}
