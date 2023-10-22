using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class TestThreadPool : MonoBehaviour
{
    private int threadNum = 15;
    string path = "E:/workprj/En/en_client/Assets/Res";

    private List<string> namelist1 = new List<string>();

    static object locker = new object();
    private List<string> namelist2 = new List<string>();
    private List<string> namelist3 = new List<string>();
    private int lockNum  = 0;
    private float beginTime = 0;
    // Start is called before the first frame update
    void Start()
    {




    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("ScanFolderSingleThread");
            float t = Time.realtimeSinceStartup;
            namelist1.Clear();
            ScanFolderSingleThread(path);
            Debug.Log(namelist1.Count);
            Debug.Log("ScanFolderSingleThread end "+ (Time.realtimeSinceStartup - t));
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            Debug.Log("ScanFolder");
            beginTime = Time.realtimeSinceStartup;
            namelist2.Clear();
            namelist3.Clear();
            ScanFolder(path);
            lockNum = namelist2.Count;
            Debug.Log(namelist2.Count);
            ThreadPool.SetMaxThreads(threadNum, threadNum);
            for (int i = 0; i < namelist2.Count; i++)
            {
                ThreadPool.QueueUserWorkItem(ScanFolderByThread, namelist2[i]);

            }

        }

        if (beginTime > 0)
        {
            if (lockNum <= 0)
            {
                Debug.Log("ScanFolder end "+ namelist3.Count +" " + (Time.realtimeSinceStartup - beginTime));
                beginTime = 0;
            }

        }


    }

    void ScanFolderSingleThread(string folder)
    {
        if (!Directory.Exists(folder))
            return;
        string[] files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {
            namelist1.Add(files[i]);
            //Test(files[i]);
        }


        string[] paths = Directory.GetDirectories(folder);
        for (int i = 0; i < paths.Length; i++)
             ScanFolderSingleThread(paths[i]);
    }

    void ScanFolder(string folder)
    {
        if (!Directory.Exists(folder))
            return;

        string[] paths = Directory.GetDirectories(folder);
        namelist2.AddRange(paths);
        for (int i = 0; i < paths.Length; i++)
            ScanFolder(paths[i]);
    }

    void ScanFolderByThread(object folder1)
    {
        string folder = folder1.ToString();
        //Debug.Log("--" + folder);
        if (!Directory.Exists(folder))
        {
            Debug.Log("no " + folder);
            return;
        }

        string[] files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly);
        lock (locker)
        {
            //Debug.Log("--" + folder);
            for (int i = 0; i < files.Length; i++)
            {
                namelist1.Add(files[i]);
            }

            lockNum--;
            //Debug.Log("--" + lockNum);
        }
        //for (int i = 0; i < files.Length; i++)
         //   Test(files[i]);

    }

    void Test(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            str.Insert(i,"a");
        }
    }
}
