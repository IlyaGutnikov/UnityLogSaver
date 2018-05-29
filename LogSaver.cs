using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace IlyaGutnikov.LogSaver
{
    public class LogSaver : MonoBehaviour
    {
        [Header("Mark true to activate log on awake")]
        public bool EnableLoggingOnAwake;

        [Header("Mark true to activate log on DEVELOPMENT_BUILD")]
        public bool EnableLoggingOnDevBuild;
        
        private bool _isLoggingEnable;

        private LogsKeeper _logsKeeper;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            var isActiveOnDevBuild = Debug.isDebugBuild && EnableLoggingOnDevBuild;

            if (isActiveOnDevBuild || EnableLoggingOnAwake)
            {
                SetLoggingEnabled(true);   
            }
        }

        private void OnDestroy()
        {
            SetLoggingEnabled(false);
            WriteLogsToFile();
        }

        public void SetLoggingEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                EnableLogging();
            }
            else
            {
                DisableLogging();
            }
        }

        private void EnableLogging()
        {
            if (_isLoggingEnable)
            {
                Debug.Log("Logging is already enabled!");
                return;
            }

            Application.logMessageReceived += LogMessage;
            _logsKeeper = new LogsKeeper();
            _isLoggingEnable = true;

        }

        private void DisableLogging()
        {
            if (_isLoggingEnable == false)
            {
                Debug.Log("Logging is already disabled!");
                return;
            }
            
            Application.logMessageReceived -= LogMessage;
            _isLoggingEnable = false;
        }

        private void LogMessage(string condition, string stackTrace, LogType type)
        {
            var logInfo = new Log(condition, stackTrace, type, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));

            _logsKeeper.Logs.Add(logInfo);
        }

        public void WriteLogsToFile()
        {
            if (_logsKeeper == null || _logsKeeper.Logs == null || _logsKeeper.Logs.Count == 0)
            {
                Debug.LogWarning("Can't find logs to write!");
                return;
            }

            string jsonLogs = JsonUtility.ToJson(_logsKeeper, true);
            
            var path = Path.Combine(Application.persistentDataPath, "Logs");
            path = Path.Combine(path, "Log_" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz") + ".txt");

            var dirPath = Path.GetDirectoryName(path);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);   
            }
            
            File.WriteAllText(path, jsonLogs, Encoding.UTF8);
        }
    }
}