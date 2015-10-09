using DownloadPackageManager.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadPackageManager
{
    public class DownloadManager : IDownloadPackageManager
    {
        private Thread _threaMain;
        // The stream of data retrieved from the web server
        private Stream strResponse;
        // The stream of data that we write to the harddrive
        private Stream strLocal;
        // The request to the web server for file information
        private HttpWebRequest webRequest;
        // The response from the web server containing information about the file
        private HttpWebResponse webResponse;
        /// <summary>
        /// On downloading file
        /// </summary>
        public event FinishProgressDownloading OnDownloading;
        /// <summary>
        /// 
        /// </summary>
        public event InitProgressDownload OnInitDownloading;
        public event FinishProgressDownloading OnFinishDownloading;
        public DownloadManager()
        {
            this.DownloadList = new List<DownloadInfo>();
            _threaMain = new Thread(ExecuteDownloadList);
        }
        public List<DownloadInfo> DownloadList { get; set; }

        public void Download(string url, string fileName)
        {
            DownloadList.Add(new DownloadInfo { Url = url, DestinationFileName = fileName });

            _threaMain.Start();
        }

        public void Download(List<DownloadInfo> listOfDownloads)
        {
            DownloadList = listOfDownloads;

            _threaMain.Start();
        }

        private void ExecuteDownloadList()
        {
            SetInitDownloading();
            foreach (var downloadInf in DownloadList)
            {
                using (WebClient wcDownload = new WebClient())
                {
                    try
                    {
                        webRequest = (HttpWebRequest)WebRequest.Create(downloadInf.Url);
                        webRequest.Credentials = CredentialCache.DefaultCredentials;
                        webResponse = (HttpWebResponse)webRequest.GetResponse();
                        Int64 fileSize = webResponse.ContentLength;

                        strResponse = wcDownload.OpenRead(downloadInf.Url);
                        strLocal = new FileStream(downloadInf.DestinationFileName, FileMode.Create, FileAccess.Write, FileShare.None);

                        int bytesSize = 0;
                        byte[] downBuffer = new byte[2048];

                        // Loop through the buffer until the buffer is empty
                        while ((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            // Write the data from the buffer to the local hard drive
                            strLocal.Write(downBuffer, 0, bytesSize);
                            // Invoke the method that updates the form's label and progress bar
                            downloadInf.DownloadPercent = Convert.ToInt32(strLocal.Length * 100 / (fileSize == 0 ? 1 : fileSize));
                        }
                    }
                    finally
                    {
                        // When the above code has ended, close the streams
                        strResponse.Close();
                        strLocal.Close();
                    }
                }
                SetOnDownloading(downloadInf);
            }
            SetFinishDownload();
        }

        private void SetOnDownloading(DownloadInfo downloadInf)
        {
            if (OnDownloading != null)
            {
                var index = DownloadList.IndexOf(downloadInf);
                var percent = index * 100 / (DownloadList.Count == 0 ? 1 : DownloadList.Count);
                OnDownloading(this, new DownloadProgressArgs() { Percent = percent });
            }
        }

        private void SetFinishDownload()
        {
            if (OnFinishDownloading != null)
                OnFinishDownloading(this, null);
        }

        private void SetInitDownloading()
        {
            if (OnInitDownloading != null)
                OnInitDownloading(this, null);
        }

        public void CancelDownload()
        {
            if (_threaMain != null)
                _threaMain.Abort();
        }
    }
}
