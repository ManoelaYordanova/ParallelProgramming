namespace ParallelProgramming
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random generator = new Random();
            int numberOfEmails = generator.Next(10, 19);
            generateEmails();
        }

        List<Emails> readEmails = new List<Emails>();
        public void generateEmails()
        {
            using (var streamReader = new StreamReader(@"C:\Users\USER\Desktop\C#\ParallelProgramming\ParallelProgramming\emails.csv"))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var elements = line.Split(new char[] {  ','  }, StringSplitOptions.RemoveEmptyEntries);
                    Emails email = new Emails(elements[0], elements[1], elements[2], false);
                    readEmails.Add(email);
                }
            }
            dataGridView1.DataSource = readEmails;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}