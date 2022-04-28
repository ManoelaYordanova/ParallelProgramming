using RegularExpressionDataGenerator;

namespace ParallelProgramming
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GenerateEmails();
        }

        List<Emails> readEmails = new List<Emails>();
        public void GenerateEmails()
        {
            string emailPattern = @"[a-z]{1,9}@((gmail)|(outlook)|(hotmail))\.com";
            string textPattern = "\\w{10,90}";
            RegExpDataGenerator emailGenerator = new RegExpDataGenerator(emailPattern);
            RegExpDataGenerator textGenerator = new RegExpDataGenerator(textPattern);
            int random = new Random().Next(10, 100);
            for (int i = 0; i < random; i++)
            {
                readEmails.Add(new Emails(i.ToString(), emailGenerator.Next(), textGenerator.Next()));
            }
            dataGridView1.DataSource = readEmails;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}