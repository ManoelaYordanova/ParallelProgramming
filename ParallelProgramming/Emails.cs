namespace ParallelProgramming
{
    public class Emails
    {
        public Emails(string nummer, string email, string text)
        {
            this.Nummer = nummer;
            this.Email = email;
            this.Text = text;
        }
        public string Nummer { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public bool Processed { get; set; }

   }
}
