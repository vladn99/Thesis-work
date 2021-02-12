using System;
using System.Windows.Forms;

namespace Дипломная_работа
{
    public partial class Form1 : Form
    {
        connection con = new connection("", @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + "\\database.mdf;Integrated Security=True;");

        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                string us_sys = "";
                int stat = 0;
                string v_shtate = "";
                con.smena_zaprosa("select * from user_system where login_us = N'" + textBox1.Text + "' and password_us = N'" + textBox2.Text + "'");
                con.connection_open();
                con.data_reader();
                if (con.reader.HasRows)
                {
                    while (con.reader.Read())
                    {
                        us_sys = "" + con.reader.GetInt32(0).ToString() + " " + con.reader.GetString(1);
                        stat = con.reader.GetInt32(2);
                        v_shtate = con.reader.GetString(7);
                    }
                }
                else
                    MessageBox.Show("Данного пользователя не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.connection_close();
                if (us_sys != "")
                {
                    string statys = "";
                    con.smena_zaprosa("select * from statys where id = " + stat + "");
                    con.connection_open();
                    con.data_reader();
                    if (con.reader.HasRows)
                    {
                        while (con.reader.Read())
                            statys = con.reader.GetString(1);
                    }
                    con.connection_close();
                    if (statys != "admin")
                    {
                        if (v_shtate == "true")
                        {
                            this.Hide();
                            Form4 form4 = new Form4();
                            form4.us_system = us_sys;
                            form4.Show();
                        }
                        else
                            MessageBox.Show("У данного пользователя нет права доступа. Обратитесь к администратору", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        this.Hide();
                        Form6 form6 = new Form6();
                        form6.Show();
                    }
                }
            }
            else
                MessageBox.Show("Заполните не заполненные поля", "Проверка заполнеиня", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
