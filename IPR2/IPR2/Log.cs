﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IPR2
{
    public class Log
    {
        public List<string> _log { get; set; }
        public string _logName;

        public Log(string name)
        {
            _log =  new List<string>();
            _logName = name;
        }

        [JsonConstructor]
        public Log(List<string> log, string logName)
        {
            _log = log;
            _logName = logName;
        }

        public void AddLogEntry(string text)
        {
            var src = DateTime.Now;
            string totalText = $"{src.Year}-{src.Month}-{src.Day} {src.Hour}:{src.Minute}:{src.Second} \t {text}";
            _log.Add(totalText);
            Console.WriteLine(totalText);
        }

        public void DeleteLog()
        {
            _log.Clear();
        }

        public override string ToString()
        {
            string text = $"{_logName}\n";
            foreach(var s in _log)
            {
                text += $"{s}\n";
            }
            return text;
        }
    }
}
