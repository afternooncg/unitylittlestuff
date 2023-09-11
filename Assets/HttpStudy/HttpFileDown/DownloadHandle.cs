using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace WDframework
{
    /// <summary>
    /// 下载句柄
    /// 注意，如果unity的权限不是require的话，下载会失败
    /// </summary>
    public class DownloadHandle : DownloadHandlerScript
    {
        #region 私有字段
        /// <summary>
        /// 文件保存的路径
        /// </summary>
        private string Path;
        /// <summary>
        /// 写的文件流
        /// </summary>
        private FileStream FileStream;
        /// <summary>
        /// http请求
        /// </summary>
        private UnityWebRequest UnityWebRequest;
        /// <summary>
        /// 本地已经下载的文件的大小
        /// </summary>
        private long LocalFileSize = 0;
        /// <summary>
        /// 文件的总大小
        /// 无奈开启
        /// </summary>
        public long TotalFileSize = 0;
        /// <summary>
        /// 当前的文件大小
        /// </summary>
        private long CurFileSize = 0;
        /// <summary>
        /// 用作下载速度的时间统计
        /// </summary>
        private float LastTime = 0;
        /// <summary>
        /// 用来作为下载速度的大小统计
        /// </summary>
        private float LastDataSize = 0;
        /// <summary>
        /// 下载速度,单位:Byte/S
        /// </summary>
        private float DownloadSpeed = 0;
        #endregion

        /// <summary>
        /// 使用1MB的缓存,在补丁2017.2.1p1中对DownloadHandlerScript的优化中,目前最大传入数据量也仅仅是1024*1024,再多也没用
        /// </summary>
        /// <param name="path">文件保存的路径</param>
        /// <param name="request">UnityWebRequest对象,用来获文件大小,设置断点续传的请求头信息</param>
        public DownloadHandle(string path, UnityWebRequest request): base(new byte[1024 * 1024])
        {
            Path = path;
            FileStream = new FileStream(Path, FileMode.Append, FileAccess.Write);
            UnityWebRequest = request;
            this.LocalFileSize = File.Exists(path)? new System.IO.FileInfo(path).Length : 0;

            CurFileSize = LocalFileSize;
            // 发起请求
            request.SetRequestHeader("Range", "bytes=" + LocalFileSize + "-");
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~DownloadHandle()
        {
            Debug.LogError("~DownloadHandle");
            Clean();
        }
        /// <summary>
        /// 断点续传的时候是拿到多少就写入多少，不需要最后获取了
        /// </summary>
        /// <returns>null</returns>
        protected override byte[] GetData()
        {
            return null;
        }
        /// <summary>
        /// 断点续传的时候是拿到多少就写入多少，不需要最后获取了
        /// </summary>
        /// <returns>null</returns>
        protected override string GetText()
        {
            return null;
        }
        /// <summary>
        /// 在2017.3.0(包括该版本)以下的正式版本中存在一个性能上的问题
        /// 该回调方法有性能上的问题,每次传入的数据量最大不会超过65536(2^16)个字节,不论缓存区有多大
        /// 在下载速度中的体现,大约相当于每秒下载速度不会超过3.8MB/S
        /// 这个问题在 "补丁2017.2.1p1" 版本中被优化(2017.12.21发布)(https://unity3d.com/cn/unity/qa/patch-releases/2017.2.1p1)
        /// (965165) - Web: UnityWebRequest: improve performance for DownloadHandlerScript.
        /// 优化后,每次传入数据量最大不会超过1048576(2^20)个字节(1MB),基本满足下载使用
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="dataLength">字节长度</param>
        /// <returns>成功</returns>
        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || dataLength == 0)
            {
                return false;
            }
            Debug.LogError("ReceiveData=>"+dataLength);
            FileStream.Write(data, 0, dataLength);
            CurFileSize += dataLength;
            //统计下载速度
            if (UnityEngine.Time.time - LastTime >= 1.0f)
            {
                DownloadSpeed = (CurFileSize - LastDataSize) / (UnityEngine.Time.time - LastTime);
                LastTime = UnityEngine.Time.time;
                LastDataSize = CurFileSize;
            }
            return true;
        }
        /// <summary>
        /// 接受到文件头，需要从文件头里解释相应数据
        /// </summary>
        /// <param name="contentLength">内容</param>
        protected override void ReceiveContentLength(int contentLength)
        {
            string contentLengthStr = UnityWebRequest.GetResponseHeader("Content-Length");
            if (!string.IsNullOrEmpty(contentLengthStr))
            {
                try
                {
                    TotalFileSize = long.Parse(contentLengthStr);
                }
                catch (System.FormatException e)
                {
                    UnityEngine.Debug.Log("获取文件长度失败,contentLengthStr:" + contentLengthStr + "," + e.Message);
                    TotalFileSize = contentLength;
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log("获取文件长度失败,contentLengthStr:" + contentLengthStr + "," + e.Message);
                    TotalFileSize = contentLength;
                }
            }
            else
            {
                TotalFileSize = contentLength;
            }
            //这里拿到的下载大小是待下载的文件大小,需要加上本地已下载文件的大小才等于总大小
            TotalFileSize += LocalFileSize;
            LastTime = UnityEngine.Time.time;
            LastDataSize = CurFileSize;
        }

        protected override void CompleteContent()
        {
            base.CompleteContent();
        }
        /// <summary>
        /// 调用UnityWebRequest.downloadProgress属性时,将会调用该方法,用于返回下载进度
        /// </summary>
        /// <returns>进度</returns>
        protected override float GetProgress()
        {
            return TotalFileSize == 0 ? 0 : ((float)CurFileSize) / TotalFileSize;
        }
        #region 私有方法
        /// <summary>
        /// 关闭文件流
        /// </summary>
        public void Clean()
        {
            this.Dispose();
            DownloadSpeed = 0.0f;
            if (FileStream != null)
            {
                FileStream.Flush();
                FileStream.Dispose();
                FileStream = null;
            }
        }
        #endregion
    }
}