using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aetos.Messaging.Common.Logging
{
    public static class Log
    {
        public static void Debug(string message, params object[] args)
        {
            WriteEntry(Level.Debug, message, args);
        }

        public static void Info(string message, params object[] args)
        {
            WriteEntry(Level.Info, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            WriteEntry(Level.Error, message, args);
        }

        public static void Fatal(string message, params object[] args)
        {
            WriteEntry(Level.Fatal, message, args);
        }

        private static void WriteEntry(Level level, string message, params object[] args)
        {

            try
            {
                var entry = new LogEntry
                {
                    RecordedAtUtc = DateTime.UtcNow,
                    Host = Dns.GetHostName(),
                    Level = level,
                    Message = string.Format(message, args)
                };

                switch (level)
                {
                    case Level.Debug:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case Level.Info:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case Level.Error:
                    case Level.Fatal:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
                Console.WriteLine("          --------------------              ");
                Console.WriteLine(entry.ToString());
                Console.WriteLine("          --------------------              ");
                Console.ResetColor();

                //var logToMongo = ConfigurationManager.AppSettings["logToMongo"];
                //if (logToMongo != null && bool.Parse(logToMongo))
                //{
                //    var db = Db.GetDatabase();
                //    var entries = db.GetCollection<LogEntry>("logEntries");
                //    entries.Save(entry);
                //}
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error writing log entry: {0}; {1}", message, ex.Message);
                Console.ResetColor();
            }
        }
    }
}
