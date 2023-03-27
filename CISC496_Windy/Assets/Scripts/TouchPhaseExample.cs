using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class TouchPhaseExample : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 0, 150, 30), "Reset"))
        {
            StartCoroutine(ShowWindow());
        }
    }

    IEnumerator ShowWindow()
    {
        Windy.UI.UI_PopupWindow.ConnectionWindowShowUp();

        yield return new WaitForSeconds(1.0f);

        Windy.UI.UI_PopupWindow.DisconnectionWindowShowUp();

        yield return new WaitForSeconds(1.0f);

        Windy.UI.UI_PopupWindow.ConnectionWindowShowUp();

        yield return new WaitForSeconds(1.2f);

        Windy.UI.UI_PopupWindow.DisconnectionWindowShowUp();

        yield return new WaitForSeconds(1.2f);

        Windy.UI.UI_PopupWindow.ConnectionWindowShowUp();

        yield return new WaitForSeconds(2.0f);

        Windy.UI.UI_PopupWindow.DisconnectionWindowShowUp();

        yield return new WaitForSeconds(2.0f);

        Windy.UI.UI_PopupWindow.ConnectionWindowShowUp();

    }
}