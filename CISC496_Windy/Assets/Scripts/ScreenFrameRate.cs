using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFrameRate : Singleton<ScreenFrameRate>
{
    public int frameRate;
    
    private void Start()
    {
        Application.targetFrameRate = frameRate;
    }
    private void Update()
    {
        Application.targetFrameRate = frameRate;
    }
}
