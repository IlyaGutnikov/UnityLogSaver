using System;
using System.Collections.Generic;

namespace IlyaGutnikov.LogSaver
{
    [Serializable]
    public class LogsKeeper
    {
        public List<Log> Logs;

        public LogsKeeper()
        {
            Logs = new List<Log>();
        }
    }
}
