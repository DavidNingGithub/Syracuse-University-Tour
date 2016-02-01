using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FinalProject1.Models
{
    public class Session
    {
        static object synch = new object();              // lock object
        static Dictionary<string, FileStream> streams;   // session storage
        static int id;                                   // session identifier
        static DateTime time;                            // lease time

        static Session()
        {
            streams = new Dictionary<string, FileStream>();
            id = -1;
        }

        public string incrSessionId()
        {
            lock (synch) { return ((++id).ToString()); }
        }

        public string getSessionId()
        {
            lock (synch)
            {
                return id.ToString();
            }
        }
        public void saveStream(FileStream stream, string sessionId)
        {
            lock (synch)
            {
                DateTime lease = time.AddDays(1);
                if (lease < DateTime.Now)
                {
                    streams = new Dictionary<string, FileStream>();  // prune old streams
                    time = DateTime.Now.AddDays(1);
                }
                streams[sessionId] = stream;
            }
        }
        public FileStream getStream(string sessionId)
        {
            lock (synch)
            {
                return streams[sessionId];
            }
        }
    }
}