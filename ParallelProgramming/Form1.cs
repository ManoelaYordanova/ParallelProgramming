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
            if (readEmails.Any((email) => !email.Processed)) {
                System.Windows.Forms.MessageBox.Show("Моля първо обработете вече генерираните имейли.");
                return;
            }

            GenerateEmails();
        
        }

        List<Emails> readEmails = new List<Emails>();
        public void GenerateEmails()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            readEmails.Clear();
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
            dataGridView1.Refresh();
            button2.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        { 

            ProcessEmails();
            
        }
        public static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            List<List<T>> result = source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            if(result.Count > 4)
            {
                result[3].AddRange(result[4]);
                result.Remove(result[4]);
            }

            return result;
        }
        public void ProcessEmails()
        {
            button2.Enabled = false;
            int chunkSize = (int)Math.Ceiling((decimal)readEmails.Count / 4);
            List<List<Emails>> chunkedEmails = ChunkBy(readEmails, chunkSize);

            Thread threadOne = new Thread(() => SendEmails(chunkedEmails[0], listBox1));
            Thread threadTwo = new Thread(() => SendEmails(chunkedEmails[1],listBox2));
            Thread threadThree = new Thread(() => SendEmails(chunkedEmails[2],listBox3));
            Thread threadFour = new Thread(() => SendEmails(chunkedEmails[3],listBox4));

            threadOne.Start();
            threadTwo.Start();
            threadThree.Start();
            threadFour.Start();
        }

        public void SendEmails(List<Emails> emails, ListBox box)
        {
            foreach (Emails email in emails)
            {
                this.Invoke(delegate () { box.Items.Add($"{email.Nummer} : {email.Email}"); } );
                int random = new Random().Next(1,5);
                Thread.Sleep(random * 1000);
                MailService send = new MailService();
                send.Send(email);
                lock (dataGridView1)
                {
                    this.Invoke(delegate () { dataGridView1.Refresh(); });
                }
                this.Invoke(delegate () { box.Items.Clear(); });
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}