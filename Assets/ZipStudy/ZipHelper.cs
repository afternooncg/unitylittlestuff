using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

public class ZipHelper 
{
    /// <summary>
    /// 批量压缩
    /// </summary>
    /// <param name="targetZipFilePath"></param>
    /// <param name="filesPath"></param>
    public static void ZipFiles(string targetZipFilePath, string[] filesPath)
    {
        FileStream fs = File.Create(targetZipFilePath);
        ZipOutputStream zipStream = new ZipOutputStream(fs);

        int readSize = 0;
        byte[] buffer = new byte[1024];
        for (int i = 0; i < filesPath.Length; i++)
        {
            
            if (File.Exists(filesPath[i]))
            {
                FileStream filefs = new FileStream(filesPath[i], FileMode.Open);
                ZipEntry zipEntry = new ZipEntry(Path.GetFileName(filesPath[i]));
                zipStream.PutNextEntry(zipEntry);
                do
                {
                    readSize = filefs.Read(buffer, 0, buffer.Length);
                    if (readSize > 0)
                        zipStream.Write(buffer, 0, readSize);

                } while (readSize > 0);
                filefs.Close();

            }
        }
        //fs.Close();
        
        zipStream.Finish();
        zipStream.Close();
        
        
    }


    public static void UpZipFiles(string zipFilePath, string targetSavPath)
    {
        if (!File.Exists(zipFilePath))
            return;
        FileStream fs = File.OpenRead(zipFilePath);
        ZipInputStream zipStream = new ZipInputStream(fs);
        ZipEntry zipEntry = null;
        int readSize = 0;
        byte[] buffer = new byte[1024];
        do
        {
            zipEntry = zipStream.GetNextEntry();
            if(zipEntry==null)
                continue;

            Debug.Log("--0-" + zipEntry.Name);
            string savePath = string.Format("{0}/{1}", targetSavPath , zipEntry.Name);
            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            FileStream filefs = File.Create(savePath);

            do
            {
                readSize = zipStream.Read(buffer, 0, buffer.Length);
                if(readSize>0)
                    filefs.Write(buffer,0,readSize);

            } while (readSize>0);
            
            filefs.Flush();
            filefs.Close();

        } while (zipEntry!=null);
        
        zipStream.Close();
        
    }

   

}
