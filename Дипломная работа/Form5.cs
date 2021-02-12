using System;
using System.Windows.Forms;

namespace Дипломная_работа
{
    public partial class Form5 : Form
    {
        connection con = new connection("", @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + "\\database.mdf;Integrated Security=True;");
        public string id_statement { get; set; }
        int id_sys;

        public Form5()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.us_system = id_sys.ToString() + " " + textBox3.Text;
            form4.Show();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            int us_sys = 0;
            con.smena_zaprosa("select * from statement where Id_statement = " + id_statement + "");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                {
                    id_sys = con.reader.GetInt32(0);
                    textBox6.Text = con.reader.GetString(1);
                    textBox5.Text = con.reader.GetString(2);
                    textBox1.Text = con.reader.GetString(3);
                    textBox4.Text = con.reader.GetString(4);
                    textBox2.Text = con.reader.GetString(5);
                    us_sys = con.reader.GetInt32(6);
                }
            }
            con.connection_close();
            con.smena_zaprosa("select * from user_system  where Id_us = " + us_sys + "");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                    textBox3.Text = con.reader.GetString(1);
            }
            con.connection_close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            print print = new print();
            print.printer(id_sys, textBox1.Text, textBox2.Text, textBox6.Text, textBox5.Text, textBox3.Text);
        }
    }
}
