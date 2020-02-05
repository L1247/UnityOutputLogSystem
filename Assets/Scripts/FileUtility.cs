using System;
using System.IO;
using UnityEngine;

namespace UnityOutputLogSystem
{
    public static class FileUtility
    {
        public static string ReadFile(string foldPath , string fileName)
        {
            CheckFoldPath(foldPath);
            StreamReader streamReader = new StreamReader(Path.Combine(foldPath, fileName), true);
            var          content      = streamReader.ReadToEnd();
            streamReader.Close();
            return content;
        }
    
        public static void WriteFile(string foldPath, string text, string fileName)
        {
            CheckFoldPath(foldPath);
            StreamWriter streamWriter = new StreamWriter(Path.Combine(foldPath, fileName), true);
            streamWriter.WriteLine(text);
            streamWriter.Close();
        }

        public static void CaptureScreenShot(string foldPath , int exceptionIndex)
        {
            CheckFoldPath(foldPath);
            var fileInfo = $"{exceptionIndex}-" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            var filename = Path.Combine(foldPath, fileInfo);
            ScreenCapture.CaptureScreenshot(filename);
        }

        private static void CheckFoldPath(string foldPath)
        {
            if (Directory.Exists(foldPath))
                return;
            Directory.CreateDirectory(foldPath);
        }
    }
}