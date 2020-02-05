using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityOutputLogSystem
{
    public class LogMonoBehaviour : MonoBehaviour
    {
        private string fileName = "test.txt";
        private Test   _test;
        private string _foldePath;
        private int    _exceptionIndex;

        [SerializeField] private List<ExceptionData> exceptionDatas; 

        private void Start()
        {
            exceptionDatas                 =  new List<ExceptionData>();
            _foldePath                     =  Path.Combine(Application.dataPath, "Log");
            Application.logMessageReceived -= ApplicationOnlogMessageReceived;
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
            _test                          =  new Test();
            _test.Init();
            SetExceptionIndex();
            TryToError();
        }

        private void SetExceptionIndex()
        {
            // _exceptionIndex = 
            var text       = FileUtility.ReadFile(_foldePath , fileName);
            var exceptions = text.Split(new []{"Exception Index"} , StringSplitOptions.RemoveEmptyEntries);
            _exceptionIndex = exceptions.Length;
        }


        private void ApplicationOnlogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception)
                return;
            var firstLine = stacktrace.Split('\n')[0];
            var words = firstLine.Split(new[]
                                        {
                                            "Stacktrace : " ," " ,"(at","Assets/Scripts/",":" , "()" , ")"
                                        } , StringSplitOptions.RemoveEmptyEntries);
            var lineNumber         = int.Parse(words[2]);
            var methodAndClassName = words[0].Split('.');
            var methodName         = methodAndClassName[0];
            var className          = methodAndClassName[1];
            if(CheckContains(methodName , className , lineNumber ))  return; // 已存在此行不輸出
            _exceptionIndex++;
            string text = $"Exception Index : {_exceptionIndex}\n" + 
                          $"Data and time : {DateTime.Now}\n"      +
                          $"Condition : {condition}\n"             +
                          $"Type : {type}\n"                       +
                          $"Stacktrace : {stacktrace}";
            FileUtility.CaptureScreenShot(Path.Combine(_foldePath, "ScreenShot Folder") , _exceptionIndex);
            FileUtility.WriteFile(_foldePath, text, fileName);
        }

        private bool CheckContains(string methodName , string ClassName , int lineNumber)
        {
            var exceptionData = exceptionDatas.Find(data => data.ClassName           == ClassName && data.MethodName == methodName &&
                                                            data.ExceptionLineNumber == lineNumber);
            return exceptionData != null;
        }

        public void TryToError()
        {
            _test.TT();
        }
    }
    public class Test
    {
        private int[] _ints;

        public void TT()
        {
            ++_ints[1];
        }

        public void Init()
        {
            _ints = new int[1]{ 1 };
        }
    }

    public class ExceptionData
    {
        public string ClassName;
        public string MethodName;
        public int    ExceptionLineNumber;
    }
}