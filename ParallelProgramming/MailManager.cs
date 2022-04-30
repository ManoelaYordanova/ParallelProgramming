using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class MailManager
    {
        public static string Office365 { get => ConfigurationManager.AppSettings["office365Smtp"]?.ToString(); }
        public static string Gmail { get => ConfigurationManager.AppSettings["gmailSmtp"]?.ToString(); }
        public static string Hotmail { get => ConfigurationManager.AppSettings["hotmailSmtp"]?.ToString(); }
    }
}
