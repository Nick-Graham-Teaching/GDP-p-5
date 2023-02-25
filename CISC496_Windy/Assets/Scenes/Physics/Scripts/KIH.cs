using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KEYSTAT
{
    PRESS, TAP, IDLE, UP
}

public class Key 
{
    public KEYSTAT Value { get; set; }
    public Key() {
        Value = KEYSTAT.IDLE;
    }
}

public static class Keys 
{
    public static readonly KeyCode UpCode = KeyCode.W;   // Going forward
    public static readonly KeyCode LeftCode = KeyCode.A;   // Going left
    public static readonly KeyCode DownCode = KeyCode.S;   // Going backword
    public static readonly KeyCode RightCode = KeyCode.D;   // Going right
    public static readonly KeyCode JumpCode = KeyCode.Space;   // Jump

    public static readonly KeyCode[] keys = { UpCode, LeftCode, DownCode, RightCode, JumpCode };
}

// Stand for KeyInputHandler
public class KIH : Singleton<KIH>  
{

    // After receving getkeydown event, wait for keyBufferCD seconds to see if getkey events get recveived.
    [SerializeField]
    private float DirectionKeyColdDown;
    [SerializeField]
    private float JumpKeyColdDown;
    private bool keyUp;

    private Dictionary<KeyCode, Key> keyDic;
    private Dictionary<KeyCode, int> directionKeyPresseds;

    private void Start()
    {
        keyDic = new Dictionary<KeyCode, Key>();
        directionKeyPresseds = new Dictionary<KeyCode, int>();

        foreach (KeyCode keyCode in Keys.keys) {
            keyDic.Add(keyCode, new Key());
            StartCoroutine(KeyboardControl(keyCode));
        }
    }

    private IEnumerator KeyboardControl(KeyCode key) 
    {
        while (true)
        {
            if (keyDic[key].Value == KEYSTAT.UP)
            {
                keyUp = false;
                keyDic[key].Value = KEYSTAT.IDLE;
            }
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(KeyColdDownTimer(key));
            }
            if (keyDic[key].Value == KEYSTAT.TAP)
            {
                keyDic[key].Value = KEYSTAT.UP;
            }
            yield return null;
        }
    }
    private IEnumerator KeyColdDownTimer(KeyCode key)
    {
        yield return new WaitForSeconds(key == Keys.JumpCode ? JumpKeyColdDown : DirectionKeyColdDown);
        keyDic[key].Value = Input.GetKey(key) ? KEYSTAT.PRESS : KEYSTAT.TAP;
        if (keyDic[key].Value == KEYSTAT.PRESS)
        {
            directionKeyPresseds[key] = 0;
            StartCoroutine(WaitForKeyUp(key));
        }
    }
    private IEnumerator WaitForKeyUp(KeyCode key)
    {
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        keyDic[key].Value = KEYSTAT.UP;
        directionKeyPresseds.Remove(key);
        keyUp = true;
    }

    
    public bool GetKeyUp(KeyCode kc) 
    {
        return keyDic[kc].Value == KEYSTAT.UP;
    }
    public bool GetKeyTap(KeyCode kc)
    {
        return keyDic[kc].Value == KEYSTAT.TAP;
    }
    public bool GetKeyPress(KeyCode kc)
    {
        return keyDic[kc].Value == KEYSTAT.PRESS;
    }
    public bool LastKeyUpAfterPress() {
        return directionKeyPresseds.Keys.Count == 0 && keyUp;
    }
}
