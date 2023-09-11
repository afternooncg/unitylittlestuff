/****************************************************************************
 // @desc: HttpFileDown.cs  http大文件下载,将文件写入指定目录中，支持同步异步操作
 // @auther: yyj
 // @date:  2021/05/18
 ****************************************************************************/

using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace XFramework.Modular.Download
{
    public class HttpFileDown : CustomYieldInstruction
    {
        const int DEFAULT_DOWNLOAD_BUFFERLENGTH =  65536;  //1024; //

        private HttpWebRequest _request;
        private HttpWebResponse _reponse;

        private int _cancelled;
        private int _finished;
        private long _contentLength;
        //下载文件大小
        private long _fileSize;
        //已经接收的大小
        private long _bytesReceived;
        //下载进度
        private float _progressPercentage;
        //下载缓存大小
        private long _bufferLength;
        //缓存区
        private byte[] _innerBuffer;
        //写入操作
        private Stream _writeStream;
        //读取strem
        private Stream _readStream;
        //error信息
        private string _error;

        /// <summary>
        /// 需要下载的文件大小
        /// </summary>
        public long Size
        {
            get
            {
                return (_fileSize > 0?_fileSize:0);
            }
        }
        /// <summary>
        /// 已经接收的大小
        /// </summary>
        public long BytesReceived
        {
            get
            {
                return _bytesReceived;
            }
        }
        /// <summary>
        /// 下载进度
        /// </summary>
        public float Progress
        {
            get
            {
                return _progressPercentage;
            }
        }

        /// <summary>
        /// 异常报错
        /// </summary>
        public string Error
        {
            get
            {
                return _error;
            }
            protected set
            {
                _error = value;
            }
        }
        /// <summary>
        /// 取消下载
        /// </summary>
        public bool Cancelled
        {
            get
            {
                return Interlocked.CompareExchange(ref _cancelled, 0, 0) == 1;
            }
        }
        /// <summary>
        /// 下载是否完成
        /// </summary>
        public bool IsDone
        {
            get
            {
                return _progressPercentage == 1;
            }
        }
        /// <summary>
        /// 保持连接,下载未完成时
        /// </summary>
        public override bool keepWaiting
        {
            get
            {
                return Interlocked.CompareExchange(ref _finished, 0, 0) == 0;
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="url"> 下载地址</param>
        /// <param name="filePath"> 下载存入的文件路径</param>
        /// <param name="continueFromLast">若文件已经存在，是否从文件末尾继续下载，即是否需要断点续传</param>
        /// <param name="Async"> 同步下载还是异步下载</param>
        public HttpFileDown(string url, string filePath, bool continueFromLast = false,bool Async = true)
        {
            _error = string.Empty;
            _contentLength = 0;
            _progressPercentage = 0;
            _bytesReceived = 0;
            _fileSize = -1;
            _finished = 0;
            _cancelled = 0;

            _innerBuffer = new byte[DEFAULT_DOWNLOAD_BUFFERLENGTH];

            try
            {
                //创建文件或者从文件末尾开始写入（断点续传功能）
                if (continueFromLast && File.Exists(filePath))
                {
                    FileStream file = File.OpenWrite(filePath);
                    _bytesReceived = file.Length;
                    file.Seek(_bytesReceived, SeekOrigin.Current);

                    _writeStream = file;
                }
                else
                {
                    _writeStream = new FileStream(filePath, FileMode.Create);
                }

                //解决https的问题
               ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;
                _request = (HttpWebRequest)WebRequest.Create(new System.Uri(url));
                _request.KeepAlive = false;
                _request.Timeout = 5000;

                /*
                 //有网友说HttpWebRequest模式在某些机型异常，未碰到
                  var headRequest = UnityWebRequest.Head(adress);
                  yield return headRequest.SendWebRequest();
                  var totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));
                 */

                long totalfilesize = 0;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "HEAD";
                HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
                if (response1.StatusCode == HttpStatusCode.OK)
                {
                    totalfilesize = response1.ContentLength;
                }

                request = null;

                if (totalfilesize <= _bytesReceived)
                {
                    Debug.Log("Stop Request");
                    Finish();
                    return;
                }


                if (_bytesReceived > 0)
                {
                    //加入偏移量，断点续传
                    _request.AddRange((int)_bytesReceived);
                    // UnityWebRequet发起请求
                    //request.SetRequestHeader("Range", "bytes=" + LocalFileSize + "-");
                }


                if(Async)
                {
                    _request.BeginGetResponse(new AsyncCallback(_getResponseCallback), this);
                }
                else
                {
                    var response = _request.GetResponse();
                    _getResponseSync(response, this);
                }

            }
            catch (Exception e)
            {
                _error = e.Message;
                Finish();
            }
        }

        ~HttpFileDown()
        {
            Cancel();
        }

        public void Cancel()
        {
            if (Interlocked.CompareExchange(ref _cancelled, 1, 0) == 1)
            {
                return;
            }

            Interlocked.Exchange(ref _cancelled, 1);
            _error = "httpfile _cancelled";
            Finish();
        }



        /// <summary>
        /// 下载进度更新
        /// </summary>
        /// <param name="bytesRead"></param>
        private void _postProgressChanged(long bytesRead)
        {
            _bytesReceived += bytesRead;
            _progressPercentage = (_fileSize < 0 ? 0 : _fileSize == 0 ? 100 : (_bytesReceived / (float)_fileSize));
        }

        /// <summary>
        /// http完成
        /// </summary>
        public void Finish()
        {
            if (Interlocked.CompareExchange(ref _finished, 1, 0) == 1)
            {
                return;
            }

            Debug.Log("---- Finish");
            Interlocked.Exchange(ref _finished, 1);
            if (_request != null)
            {
                _request.Abort();
                _request = null;
            }
            if(_reponse != null)
            {
                _reponse.Close();
                _reponse = null;
            }

            if (_writeStream != null)
            {
                _writeStream.Close();
                _writeStream.Dispose();
                _writeStream = null;
            }
            if(_readStream != null)
            {
                _readStream.Close();
                _readStream.Dispose();
                _readStream = null;
            }
            ServicePointManager.ServerCertificateValidationCallback = null;
        }

        /// <summary>
        /// 同步异步请求数据
        /// </summary>
        /// <param name="respone"></param>
        /// <param name="http"></param>
        private static void _getResponseSync(WebResponse respone, HttpFileDown http)
        {
            if (respone == null || http == null || http.keepWaiting == false)
            {
                http.Error = "httpfile respone null";
                http.Finish();
                return;

            }
            try
            {
                http._reponse = respone as HttpWebResponse;

                if (http._reponse.StatusCode == HttpStatusCode.OK || http._reponse.StatusCode == HttpStatusCode.PartialContent)
                {
                    http._contentLength = http._reponse.ContentLength;
                    Debug.Log("_getResponseSync get succ " + http._contentLength);
                    http._fileSize = (http._bytesReceived + http._contentLength);

                    http._bufferLength = (http._contentLength > DEFAULT_DOWNLOAD_BUFFERLENGTH ? DEFAULT_DOWNLOAD_BUFFERLENGTH : http._contentLength);

                    http._readStream = http._reponse.GetResponseStream();
                    http._readStream.BeginRead(http._innerBuffer, 0, (int)http._bufferLength, _readCallback, http);

                }
                else
                {
                    http.Error = "httpfile response not ok";
                    http.Finish();
                }

            }
            catch (Exception e)
            {
                http._error = e.Message;
                http.Finish();
            }
        }
        private static void _getResponseCallback(IAsyncResult result)
        {
            HttpFileDown http = (HttpFileDown)result.AsyncState;
            try
            {
                var response = http._request.EndGetResponse(result);
                Debug.Log("_getResponseCallback get size: " + response.ContentLength);
                _getResponseSync(response, http);

            }
            catch (Exception e)
            {
                http._error = e.Message;
                http.Finish();
            }
        }
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="result"></param>
        private static void _readCallback(IAsyncResult result)
        {
            HttpFileDown http = (HttpFileDown)result.AsyncState;
            int bytesRead = 0;

            try
            {
                if (http._readStream != null && http._readStream != Stream.Null)
                {
                    bytesRead = http._readStream.EndRead(result);
                }

                Debug.Log("_readCallback " + bytesRead);
                if (bytesRead > 0)
                {
                    if (http._writeStream != null)
                    {
                        http._writeStream.Write(http._innerBuffer, 0, bytesRead);
                    }

                    http._postProgressChanged(bytesRead);
                }

                // WebRequest.Abort() 会调用WebResponse.Close()，导致跟ReadStream调用冲突，所以需要放在同一线程调用。
                if (http.Cancelled || bytesRead <= 0)
                {
                    if (http.IsDone == false && string.IsNullOrEmpty(http.Error))
                    {
                        http.Error = "bytesRead is null";
                    }

                    http.Finish();
                }

                else
                {
                    http._readStream.BeginRead(http._innerBuffer, 0, (int)http._bufferLength, _readCallback, http);
                }

            }
            catch (Exception e)
            {
                http._error = e.Message;
                http.Finish();
            }
        }
    }
}