using UnityEngine;
using System.Threading;
using System.Collections;

public class TouchPhaseExample : MonoBehaviour
{

    int num;

    ThreadStart threadDelegate;
    Thread childThread;

    void Increment()
    {
        while(num < 10)
        {
            num++;
            //Debug.Log("Child Thread: " + num);
            Thread.Sleep(1000);
        }
    }

    private void Start()
    {
        threadDelegate = new ThreadStart(Increment);
        childThread = new Thread(threadDelegate);

        Debug.Log("Main: Start Child Thread");
        childThread.Start();

        //StartCoroutine(MainReportNum());
    }

    IEnumerator MainReportNum()
    {
        while (true)
        {
            Debug.Log("Main: " + num);
            yield return new WaitForSeconds(0.0f);
        }
    }

    private void Update()
    {
        Debug.Log("Main: " + num);
    }
}