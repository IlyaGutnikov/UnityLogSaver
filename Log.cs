using System;
using UnityEngine;

namespace IlyaGutnikov.LogSaver
{
    [Serializable]
    public class Log
    {
        public string Condition;
        public string StackTrace;
        public LogType LogType;

        public string DateTime;

        public Log(string condition, string stackTrace, LogType type, string dateTime)
        {
            Condition = condition;
            StackTrace = stackTrace;
            LogType = type;
            DateTime = dateTime;
        }
    }
}