using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace EStilesA2_6SQL
{

    public partial class frmMain : Form
    {
        static SQLiteConnection sqlite_conn;
        static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "movies.db");
        String Info;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            sqlite_conn = CreateConnection();
        }

        static SQLiteConnection CreateConnection()
        {

            sqlite_conn = new SQLiteConnection("Data Source=" + path);
            //open conn
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sqlite_conn;
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd;
            cmd = sqlite_conn.CreateCommand();
            //drop it if its there
            cmd.CommandText = "DROP TABLE IF EXISTS movies";
            cmd.ExecuteNonQuery();
            //create table with 3 columns, primary key name id
            cmd.CommandText = @"Create Table movies(id INTEGER PRIMARY KEY,
                title TEXT, main_character TEXT, year INT)";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Table Created");
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd;
            cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Demon Slayer: Kimetsu no Yaiba the Movie: Mugen Train', 'Tanjiro Kamado', '2020')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('My Hero Academia: Heroes Rising', 'Izuku Midoriya', '2019')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('My Hero Academia: Two Heroes', 'Izuku Midoriya', '2018')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Spirited Away', 'Chihiro Ogino', '2002')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Princess Mononoke', 'Ashitaka', '1997')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Kikis Delivery Service', 'Kiki', '1989')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Howls Moving Castle', 'Sophie Hatter', '2004')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Wolf Children', 'Ame & Yuki', '2013')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('A Whisker Away', 'Miyo Sasaki', '2020')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO movies(title, main_character, year) VALUES ('Castle In The Sky', 'Sheeta', '1986')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Rows Created");
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            SQLiteDataReader dr;
            SQLiteCommand cmd;
            cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "SELECT * from movies";
            //use reader to get data from table
            dr = cmd.ExecuteReader();
            lstMovies.Items.Add("ID\t\tTitle\t\tMain Character\t\tYear Created");
            while (dr.Read())
            {
                Info = dr.GetInt32(0) + "\t" + dr.GetString(1) + "\t" + dr.GetString(2) + "\t" + dr.GetInt32(3);
                lstMovies.Items.Add(Info);
            }
            //sqlite_conn.Close();
        }

        private StringBuilder GenerateReport()
        {
            //actual formatting for the report
            StringBuilder html = new StringBuilder();
            //lets you style it
            StringBuilder css = new StringBuilder();
            //www.w3schools.com/css/css_syntax.asp
            css.Append("<style>");
            css.Append("td {padding:5px;text-align:center;font-weight:bold;text-align:center;}");
            css.Append("h1{color: green;}");
            css.Append("</style>");

            html.Append("<html>");
            html.Append($"<head>{css}<title>{"Movie List"}</title></head>");
            html.Append("<body>");
            html.Append($"<h1>{"Movie List"}</h1>");

            //Create Table data
            SQLiteDataReader dr;
            SQLiteCommand cmd;
            cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "SELECT * from movies";
            //use reader to get data from table
            dr = cmd.ExecuteReader();
            html.Append("<table>");
            html.Append("<th>MovieID</th>");
            html.Append("<th>Movie Title</th>");
            html.Append("<th>Main Character</th>");
            html.Append("<th>Release Year</th>");
            html.Append("<tr><td colspan=3><hr/></td></tr>");
            html.Append("<tr>");

            while (dr.Read())
            {
                Info = dr.GetInt32(0) + "\t" + dr.GetString(1) + "\t" + dr.GetString(2) + "\t" + dr.GetInt32(3);
                html.Append($"<td>{dr.GetInt32(0)}</td>");
                html.Append($"<td>{dr.GetString(1)}</td>");
                html.Append($"<td>{dr.GetString(2)}</td>");
                html.Append($"<td>{dr.GetInt32(3)}</td>");
                html.Append("</tr>");
            }
            html.Append("<tr><td colspan=4><hr/></td></tr>");
            html.Append("</table>");
            html.Append("</body></html>");
            return html;
        }

        private void PrintReport(StringBuilder html)
        {
            //write to hard drive using the name report.html
            try
            {
                //using statement will automatically close a file after opening it
                using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Report.html"))
                {
                    wr.WriteLine(html);
                }
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Report.html");
            }
            catch (Exception)
            {
                MessageBox.Show("You don't have write permissions", "Error System Permissions", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            //DateTime today = DateTime.Now;
            //using (StreamWriter wr = new StreamWriter($"{today.ToString("yyyy-MM-dd-HHmmss")} - Report.html"))
            //{
            //    wr.WriteLine(html);
            //}
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            PrintReport(GenerateReport());
        }


        public Image ExitHoverImage;
        //public Image ExitUnHoverImage = Image.FromFile(@"ExitUnHover.png");

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            //pictureBox1.Image = ExitHoverImage;
            pictureBox1.Load("https://i.imgur.com/6swA1NF.png");
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Load("https://i.imgur.com/Ia3C7lU.jpg");
           // pictureBox1.Image = ExitUnHoverImage;
        }
    }
}
