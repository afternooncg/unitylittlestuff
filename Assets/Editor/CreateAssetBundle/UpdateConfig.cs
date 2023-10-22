/****************************************************************************
 // @desc: CheckUpdateEvent.cs,  检查更新的相关事件
 // @auther: yangyj
 // @date:  2021/05/18
 ****************************************************************************/

using System;
using System.IO;
using UnityEngine;
using XFramework.Runtime.Util;

namespace XFramework.Modular.Update
{
    public class UpdateConfig
    {
        public static readonly string UPDATE_PLATFORM_BASE =
#if UNITY_ANDROID
             "Android";
#elif UNITY_IOS
            "iOS";
#else
            "StandaloneWindows";
#endif
        //基本模块信息
        public static readonly string APK_BASE_MODULE_NAME = "base";
        //语言模块名称
        public static readonly string RES_PACK_LOCALIZE_MODULE_NAME = "localize";

        //assetbundle后缀名
        public static readonly string ASSETBUNDLE_SUFFIX = ".bundle";
        //资源映射文件路径
        public static readonly string ASSET_BUNDLE_MAP_PATH = "Assets/DataConfig/resBundle.txt";

        //模块文件生成目录
        public static readonly string MODULE_INFO_FILE_PATH = "Assets/DataConfig/ModuleInfoFile";

        //APK包预装ab包信息文件
        public static readonly string APK_ASSTBUNDLE_INFO_FILE = "apk_assetbundlesInfo";
        public static readonly string APK_ASSETBUNDLE_INFO_BUNDLE = "config/apk_assetbundlesinfo.bundle";

        public static string RES_VERSION_KEY = "Update_Res_Ver";
        public static string RES_VERSION_CHECK_KEY = "CSVersion";   //用于新包第1次运行时检测，是否是新包，如果是要清除旧热更资源

        public static string ZIP_PASSWORD = "igg@en_2021";
#if UNITY_EDITOR
        //应用版本文件路径
        public static string applicationConfigPath = "Assets/DataConfig/applicationConfig.txt";
        //预装模块信息路径
        public static string apkInitModuleConfigPath = "Assets/DataConfig/Editor/apkInitModules.asset";
        //预装资源assetbundle信息文件路径
        public static string apkAssetBundleInfoPath = "Assets/DataConfig/apk_assetbundlesInfo.txt";

        //热更新相关版本信息文件发布目录
        public static string apkUpdateVersionInfosPath = Application.dataPath + "../Data/Update/Now";
        //热更新相关版本信息文件上次发布目录，便于回退
        public static string apkUpdateVersionInfosOldPath = Application.dataPath + "../Data/Update/Old";
        //CSVersion路径
        //public static string versionCSPath = "Assets/Scripts/XFramework/Runtime/Service/Update/UpdateGenParamInfo.cs";

        //public static string versionCSPath = "Assets/Scripts/Client/GameApp/GenCore/UpdateGenParamInfo.cs";

        //public static string packConfigPath = "Assets/Scripts/Client/GameApp/GenCore/PackGenParamInfo.cs";

#endif
        //资源映射清单文件
        public static readonly string ASSET_BUNDLE_MAP_FILE = "resBundle";
        //资源映射的assetbundlename
        public static readonly string ASSET_BUNDLE_MAP = "resbundle.bundle";

        public static readonly string ASSET_BUNDLE_MAP_FULL_NAME = "config/resbundle.bundle";

        //应用版本文件
        public static string applicationConfigName = "applicationConfig.txt";
        public static string APPLICATIONCONNFIG_BUNDLE_NAME = "applicationConfig.bundle";
        public static string APPLICATIONCONFIG_BUNDLE_PATH = "config/applicationConfig.bundle";

        //整包APK下载目录地址
        public static readonly string APK_DOWNLOAD_PATH = Application.persistentDataPath.Combine("downApk.apk");

        /// <summary>
        /// 获取运行的资源目录路径
        /// </summary>
        public static string StreamingAssetsPath
        {
            get
            {
                if(Application.isEditor)
                {
                    return Environment.CurrentDirectory.Replace("\\", "/") + "/Data";
                }
                else
                {
                    return Application.streamingAssetsPath;
                }
            }
        }

        //资源在APK中目录
        public static readonly string STREAMING_ASSET_PATH = StreamingAssetsPath.Combine(UPDATE_PLATFORM_BASE);

        //资源在热更新目录
        public static readonly string UPDATE_ASSET_PATH = Application.persistentDataPath.Combine("update");

        public static string GetResPath(string path)
        {
            var filePath = UPDATE_ASSET_PATH.Combine(path);
            if (!File.Exists(filePath))
            {
                return STREAMING_ASSET_PATH.Combine(path);
            }
            return filePath;
        }

    }
}