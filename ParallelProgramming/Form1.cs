using RegularExpressionDataGenerator;

namespace ParallelProgramming
{
    public partial class Form1 : Form
    {
        private MailService mailService = new MailService();
        private List<List<Emails>> chunkedEmails;
        private TimeSpan timespan;
        private int ticks = 0;

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
            InitTimer();

            ClearData();

            string emailPattern = @"[a-z]{1,9}@((gmail)|(outlook)|(hotmail))\.com";
            string textPattern = "\\w{10,90}";

            RegExpDataGenerator emailGenerator = new RegExpDataGenerator(emailPattern);
            RegExpDataGenerator textGenerator = new RegExpDataGenerator(textPattern);

            int random = new Random().Next(20, 100);
            for (int i = 0; i < random; i++)
            {
                readEmails.Add(new Emails(i.ToString(), emailGenerator.Next(), textGenerator.Next()));
            }

            readEmails.Add(new Emails(random.ToString(), "stefkakracunova18@gmail.com", textGenerator.Next()));

            UpdateData(readEmails);
            
        }
        private void ClearData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            readEmails.Clear();
        }

        private void InitTimer()
        {
            ticks = 0;
            timespan = TimeSpan.FromSeconds(ticks);
            toolStripTextBox1.Text = $"{timespan.Minutes}:" + "0" + timespan.Seconds.ToString();
        }

        private void UpdateData(List<Emails> readEmails)
        {
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

           // ProcessOneThreadEmails();
            ///ProcessTwoThreadEmails();
           // ProcessThreeThreadEmails();
            //ProessFiveThreadEmails();
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
            timer1.Start();
            button2.Enabled = false;

            int chunkSize = (int)Math.Ceiling((decimal)readEmails.Count / 4);
            chunkedEmails = ChunkBy(readEmails, chunkSize);

            Thread threadOne = new Thread(() => SendEmails(chunkedEmails[0], thread1));
            Thread threadTwo = new Thread(() => SendEmails(chunkedEmails[1],thread2));
            Thread threadThree = new Thread(() => SendEmails(chunkedEmails[2],thread3));
            Thread threadFour = new Thread(() => SendEmails(chunkedEmails[3],thread4));

            threadOne.Start();
            threadTwo.Start();
            threadThree.Start();
            threadFour.Start();
        }

        public void ProcessOneThreadEmails()
        {
            timer1.Start();
            button2.Enabled = false;
            chunkedEmails = new List<List<Emails>>() { readEmails };
            Thread threadOne = new Thread(() => SendEmails(readEmails, thread1));
            threadOne.Start();
        }
        public void ProcessTwoThreadEmails()
        {
            timer1.Start();
            button2.Enabled = false;

            int chunkSize = (int)Math.Ceiling((decimal)readEmails.Count / 2);
             chunkedEmails = ChunkBy(readEmails, chunkSize);

            Thread threadOne = new Thread(() => SendEmails(chunkedEmails[0], thread1));
            Thread threadTwo = new Thread(() => SendEmails(chunkedEmails[1], thread2));

            threadOne.Start();
            threadTwo.Start();
        }

        public void ProcessThreeThreadEmails()
        {
            timer1.Start();
            button2.Enabled = false;

            int chunkSize = (int)Math.Ceiling((decimal)readEmails.Count / 3);
            chunkedEmails = ChunkBy(readEmails, chunkSize);

            Thread threadOne = new Thread(() => SendEmails(chunkedEmails[0], thread1));
            Thread threadTwo = new Thread(() => SendEmails(chunkedEmails[1], thread2));
            Thread threadThree = new Thread(() => SendEmails(chunkedEmails[2], thread3));

            threadOne.Start();
            threadTwo.Start();
            threadThree.Start();
        }

        public void ProessFiveThreadEmails()
        {
            timer1.Start();
            button2.Enabled = false;

            int chunkSize = (int)Math.Ceiling((decimal)readEmails.Count / 5);
            chunkedEmails = ChunkBy(readEmails, chunkSize);

            Thread threadOne = new Thread(() => SendEmails(chunkedEmails[0], thread1));
            Thread threadTwo = new Thread(() => SendEmails(chunkedEmails[1], thread2));
            Thread threadThree = new Thread(() => SendEmails(chunkedEmails[2], thread3));
            Thread threadFour = new Thread(() => SendEmails(chunkedEmails[3], thread4));
            Thread threadFive = new Thread(() => SendEmails(chunkedEmails[3], thread4));

            threadOne.Start();
            threadTwo.Start();
            threadThree.Start();
            threadFour.Start();
            threadFive.Start();
        }



        public void SendEmails(List<Emails> emails, ListBox box)
        {
            foreach (Emails email in emails)
            {
                this.Invoke(delegate () { box.Items.Add($"{email.Nummer} : {email.Email}"); } );
                mailService.Send(email);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!chunkedEmails.Any((emails) => emails.Any(email => !email.Processed)))
            {
                timer1.Stop();
                return;
            }

            ticks++;
            timespan = TimeSpan.FromSeconds(ticks);

            if (timespan.Seconds < 10)
            {
              toolStripTextBox1.Text = $"{timespan.Minutes}:" + "0" + timespan.Seconds.ToString();
            } else
            {
              toolStripTextBox1.Text = $"{timespan.Minutes}:{timespan.Seconds}";
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
    }
}