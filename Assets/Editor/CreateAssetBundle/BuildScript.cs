using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XFramework.Modular.Update;
using XFramework.Runtime.Util;


class BuildScript
{
    public static void BuildFilesToAssetBundle(string[] files, string outPath, string assetbundleName,
        bool bForceBuild = false)
    {
        BuildAssetBundleOptions option = BuildAssetBundleOptions.None;
        option = BuildAssetBundleOptions.ChunkBasedCompression;


        //配置打包平台
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

        option = option | BuildAssetBundleOptions.DeterministicAssetBundle;
        //在pc上， 以ab包模式运行不能使用DisableWriteTypeTree，否则加载Tmp的fontAsset字体会报错
        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
            option = option | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
        }
        else
        {
            option = option | BuildAssetBundleOptions.DisableWriteTypeTree;
        }

        if (bForceBuild == true)
        {
            option = option | BuildAssetBundleOptions.ForceRebuildAssetBundle;
        }

        string outBuildPath = UpdateConfig.StreamingAssetsPath.Combine(target.ToString());
        if (!string.IsNullOrEmpty(outPath))
        {
            outBuildPath = outBuildPath.Combine("../../../xxData/" + outPath);
        }

        if (!Directory.Exists(outBuildPath))
        {
            Directory.CreateDirectory(outBuildPath);
        }

        Debug.Log("outBuildPath " + outBuildPath);
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = assetbundleName;
        build.assetNames = files;
        build.assetBundleVariant = string.Empty;
        BuildPipeline.BuildAssetBundles(outBuildPath, new AssetBundleBuild[1] {build}, option, target);

    }

    public static void BuildAssetBundles(AssetBundleBuild[] builds, BuildTarget target,
        BuildAssetBundleOptions options = BuildAssetBundleOptions.None)
    {
        string outputPath = UpdateConfig.StreamingAssetsPath.Combine(target.ToString());
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // for (int i = 0; i < builds.Length; i++)
        // {
        //     if (builds[i].assetBundleName.IndexOf("/") > 0)
        //     {
        //         FileHelper.EnsuerFolder(UpdateConfig.StreamingAssetsPath.Combine(FileHelper.GetParentDir(builds[i].assetBundleName)));
        //     }
        // }
        //开启加密
        /* BuildPipeline.SetAssetBundleEncryptKey("0123456789abcdef");*/
        string streamBundlePath = outputPath.Combine("streamingassets.bundle");
        BuildPipeline.BuildAssetBundles(outputPath, builds, options, target);

        string genBundlePath = outputPath.Combine(target.ToString());
        if (!File.Exists(genBundlePath))
        {
            return;
        }

        if (File.Exists(streamBundlePath))
        {
            File.Delete(streamBundlePath);
        }

        File.Move(genBundlePath, streamBundlePath);
    }


    public static void CopyAssetBundlesTo(string outputPath, BuildTarget target)
    {
        #region AB包资源

        try
        {

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            // 设置对应平台的assetbundles的源目录
            var source = UpdateConfig.StreamingAssetsPath.Combine(target.ToString());
            var output = Application.streamingAssetsPath.Combine(target.ToString());
            //Directory.CreateDirectory(output);
            if (!Directory.Exists(source))
            {
                Debug.Log("No assetBundle output folder, try to build the assetBundles first. - " + source);
                return;
            }

            if (Directory.Exists(output))
            {
                // 清理streaming assets目录
                FileUtil.DeleteFileOrDirectory(output);
            }

            FileUtil.CopyFileOrDirectory(source, output);

            // 清理manifest文件
            string[] manifests = Directory.GetFiles(outputPath, "*.manifest", SearchOption.AllDirectories);
            for (int i = 0; i < manifests.Length; i++)
            {
                FileUtil.DeleteFileOrDirectory(manifests[i]);
            }

            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

        #endregion

    }
}