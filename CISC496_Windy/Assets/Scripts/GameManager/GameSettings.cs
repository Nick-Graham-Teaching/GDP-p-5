using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [SerializeField]
    private int _inputDevice = 0;  // 0 == Keyboard; 1 == Mobile

    [SerializeField]
    private int  _frameRate = 1;    // 0 == 30; 1 == 60; 2 == 120

    [SerializeField]
    private float _sensitivityX = 0; // 0 -> 1
    [SerializeField]
    private float _sensitivityY = 0; // 0 -> 1


    public int InputDevice { 
        get => _inputDevice; 
        set {
            _inputDevice = value;
            GameEvents.OnInputDeviceChange?.Invoke(value);
        } 
    }
    public int FrameRate { 
        get => _frameRate; 
        set
        {
            _frameRate = value;
            GameEvents.OnFrameRateChange?.Invoke(30 * Mathf.RoundToInt(Mathf.Pow(2, value)));
        } 
    }
    public float SensitivityX {
        get => _sensitivityX;
        set
        {
            _sensitivityX = value;
            GameEvents.OnSentivityXChange?.Invoke(value);
        }
    }
    public float SensitivityY { 
        get => _sensitivityY;
        set
        {
            _sensitivityY = value;
            GameEvents.OnSentivityYChange?.Invoke(value);
        }
    }
}
