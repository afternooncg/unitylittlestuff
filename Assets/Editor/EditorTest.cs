using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;
using File = System.IO.File;

public class EditorTest
{
   [MenuItem("EditorTool/CreateAb_onefile")]
    public static void CreateAb()
    {
        string filepath =  "Assets/Res/ModuleInfoFile/base.txt";
        string bundlepath = "Assets/Res/ModuleInfoFile/modules.bundle";
        Debug.Log(filepath);
        float t = Time.realtimeSinceStartup;
        BuildScript.BuildFilesToAssetBundle(new string[1]{filepath},"config",bundlepath,true);
        Debug.Log("time:" + (Time.realtimeSinceStartup - t));
    }

    [MenuItem("EditorTool/CreateAb_bat")]
    public static void CreateAbbat()
    {

        string[] filepaths = System.IO.Directory.GetFiles(Application.dataPath + "/Res/ModuleInfoFile","*.txt", SearchOption.TopDirectoryOnly);
        foreach (var file in filepaths)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            string filepath =  "Assets/Res/ModuleInfoFile/" + name + ".txt";
             string bundlepath = "../../Data/Assets/Res/ModuleInfoFile/"+ name +".bundle";
             Debug.Log(filepath);
             float t = Time.realtimeSinceStartup;
             BuildScript.BuildFilesToAssetBundle(new string[1]{filepath},"config",bundlepath,true);
             Debug.Log("time:" + (Time.realtimeSinceStartup - t));
        }
    }
}
