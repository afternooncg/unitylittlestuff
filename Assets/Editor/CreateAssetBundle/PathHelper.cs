using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

namespace XFramework.Runtime.Util
{
    public static class PathHelper
    {
        public static string ReplaceESC(this string path)
        {
            return path.Replace("\\", "/");
        }
        /// <summary>
        /// 连接路径
        /// </summary>
        /// <param name="path1">路径1</param>
        /// <param name="path2">路径2</param>
        /// <returns></returns>
        public static string Combine(this string path, params string[] paths)
        {
            int capcity = path.Length;
            for (int i = 0; i < paths.Length; ++i)
            {
                paths[i] = paths[i].ReplaceESC();
                capcity += paths[i].Length;
            }
            capcity = capcity + paths.Length + 1;
            StringBuilder sb = new StringBuilder(capcity);
            sb.Append(path);
            for (int i = 0; i < paths.Length; ++i)
            {
                sb.Append("/");
                sb.Append(paths[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            path = path.Replace("\\", "/");
            string assetPath = path;
            int index = path.IndexOf("Assets/");
            if (index != -1)
            {
                assetPath = path.Substring(index + "Assets/".Length);
            }
            return Path.GetFullPath(Application.dataPath.Combine(assetPath)).ReplaceESC();
        }

        /// <summary>
        /// 获取资源在Asset下的相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAssetPath(string path)
        {
            if (path.StartsWith(Application.dataPath.Replace("\\", "/")))
            {
                path = path.Substring(Application.dataPath.Length + 1);
            }
            return string.Format("Assets/{0}", path);
        }


        /// <summary>
        /// 判断文件夹是否存在， 不存在则创建
        /// </summary>
        /// <param name="systemPath"></param>
        public static void EnsuerFolder(string systemPath)
        {
            if (!Directory.Exists(systemPath))
            {
                Directory.CreateDirectory(systemPath);
            }
        }
        /// <summary>
        /// 获取父目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetParentDir(string path)
        {
            path = path.Replace("\\", "/");
            int pos = path.LastIndexOf('/');
            if (pos == -1)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                return dirInfo.Parent.ToString();
            }
            return path.Substring(0, pos).Trim();
        }

        /// <summary>
        /// 获取主干名（不带后缀的名字）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}

