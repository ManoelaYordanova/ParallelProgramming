using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Net.Mail;
using POI;

namespace ParallelProgramming
{
    internal class Program
    {
        // uncomment if in memory db is required
        //private static SqliteConnection connection;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
            //connection = new SqliteConnection(ConfigurationManager.ConnectionStrings["InMemory"].ConnectionString);
            //connection.Open();
            //using (SqliteCommand command = new SqliteCommand(SQLiteCommandFactory.CreateTable<MailMessage>("Mails"), connection))
            //{
            //    command.ExecuteNonQuery();
            //}
        }
    }
}