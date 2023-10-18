using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ZipFiles();
        UnZipFiles();
    }

    // Update is called once per frame

    #region 压缩文件
    public void ZipFiles()
    {
        string path = Path.Combine(string.Format("{0}/",Application.dataPath), "../Data/jsToolhelper");
        string targetFile = Path.Combine(string.Format("{0}/",path), "../test.zip");
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            ZipHelper.ZipFiles(targetFile,files);
        }

    }

    public void UnZipFiles()
    {
        string path = Path.Combine(string.Format("{0}/",Application.dataPath), "../Data/jsToolhelper1");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string targetFile = Path.Combine(string.Format("{0}/",path), "../test.zip");

        ZipHelper.UpZipFiles(targetFile, path);

    }

    #endregion
    
    
}
