using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestAutoResetEvent : MonoBehaviour
{
    private AutoResetEvent _checkResetEvent = new AutoResetEvent(false);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(CallFun));
            thread.Start(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            int num = Random.Range(1, 5);
            Debug.Log("num:" + num);
            for(int i=0;i<num;i++)
                _checkResetEvent.Set();
        }
    }

    public void CallFun(object index)
    {
        int id = (int)index;
        while (true)
        {
            Debug.Log("id:unlock " + id);
            Debug.Log("id:lock " + id );
            _checkResetEvent.Reset();
            _checkResetEvent.WaitOne();

        }
    }
}
