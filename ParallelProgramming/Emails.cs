using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class Emails
    {
        public Emails(String nummer, String email, String text, Boolean processed)
        {
            this.Nummer = nummer;
            this.Email = email;
            this.Text = text;
            this.Processed = processed;
        }
        public String Nummer { get; set; }
        public String Email { get; set; }
        public String Text { get; set; }
        public Boolean Processed { get; set; }

   }
}
