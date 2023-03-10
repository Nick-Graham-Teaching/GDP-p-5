using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFrameRate : Singleton<ScreenFrameRate>
{
    [SerializeField]
    private int _frameRate;

    public int FrameRate { 
        set 
        {
            _frameRate = value;
            Application.targetFrameRate = value;
        } 
    }
    
    private void Start()
    {
        Application.targetFrameRate = _frameRate;
    }
}
