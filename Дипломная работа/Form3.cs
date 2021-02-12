using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Дипломная_работа
{
    public partial class Form3 : Form
    {
        connection con = new connection("", @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + "\\database.mdf;Integrated Security=True;");

        public Form3()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
        }

        //ввод только буквенных значений
        private void vvod_bukw(KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        //формирование уникального id
        private int get_id()
        {
            int id = 0000;
            bool true_or_false = false;
            Random random = new Random();
            try
            {
                while (true_or_false == false)
                {
                    id = random.Next(1000, 9999);
                    con.connection_open();
                    con.smena_zaprosa("select * from user_system where Id_us = " + id + "");
                    con.data_reader();
                    if (!con.reader.HasRows)
                        true_or_false = true;
                    con.connection_close();
                }
            }
            catch
            {

            }
            return id;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            vvod_bukw(e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            vvod_bukw(e);
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            con.smena_zaprosa("select * from statys");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                {
                    if (con.reader.GetString(1) == "admin")
                        continue;
                    comboBox2.Items.Add("" + con.reader.GetInt32(0).ToString() + " " + con.reader.GetString(1));
                }
            }
            con.connection_close();

            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            con.smena_zaprosa("select * from department");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                {
                    comboBox1.Items.Add("" + con.reader.GetInt32(0).ToString() + " " + con.reader.GetInt32(1) + " " + con.reader.GetString(2) + " " + con.reader.GetString(3));
                    list.Add(con.reader.GetInt32(1).ToString());
                }
            }
            con.connection_close();
            foreach (var item in list)
            {
                con.smena_zaprosa("select nm_item from item where id = " + item + "");
                con.connection_open();
                con.data_reader();
                if (con.reader.HasRows)
                {
                    while (con.reader.Read())
                    {
                        list2.Add(item + " " + con.reader.GetString(0));
                    }
                }
                con.connection_close();
            }
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                comboBox1.Items[i] = comboBox1.Items[i].ToString().Replace(list2[i].Remove(4), list2[i].Remove(0, 5));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && comboBox2.Text != "" && textBox4.Text != "" && comboBox1.Text != "")
            {
                int id = get_id();
                con.smena_zaprosa("insert into user_system(Id_us, fio, statys, phone, login_us, password_us, id_police) values (" + id + ", N'" + textBox1.Text + "', " + comboBox2.Text.Remove(4) + ", N'" + maskedTextBox1.Text + "', N'" + textBox4.Text + "', N'" + textBox2.Text + "', " + comboBox1.Text.Remove(4) + ")");
                con.connection_open();
                con.run_qwery();
                con.connection_close();
                MessageBox.Show("Данные записаны");
            }
            else
                MessageBox.Show("Заполните не заполненные поля", "Проверка заполнеиня", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
