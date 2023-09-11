using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Modular.Download;

public class TestHttpFileDown : MonoBehaviour
{
    private HttpFileDown httpfiledown = null;

    private bool isCheck = true;
    // Start is called before the first frame update
    void Start()
    {
        httpfiledown = new HttpFileDown("http://10.0.16.124/android-33.rar", "F:/xx/android-33.rar",true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheck)
        {
            if (httpfiledown.IsDone)
            {
                Debug.Log("--down succ");
                isCheck = false;
            }
        }

    }

    private void OnDestroy()
    {
        httpfiledown.Finish();
    }
}
