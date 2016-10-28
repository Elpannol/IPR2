using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    public class Log
    {
        private readonly List<string> _log;
        private readonly string _logName;

        public Log(string name)
        {
            _logName = name;
        }

        public void AddLogEntry(string text)
        {
            var src = DateTime.Now;
            string totalText = $"{src.Year}-{src.Month}-{src.Day} {src.Hour}:{src.Minute}:{src.Second} \t {text}";
            _log.Add(totalText);
        }

        public void DeleteLog()
        {
            _log.Clear();
        }

        public override string ToString()
        {
            string text = "";
            foreach(var s in _log)
            {
                text += s;
            }
            return text;
        }
    }
}
