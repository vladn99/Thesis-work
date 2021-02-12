using System;
using System.Windows.Forms;

namespace Дипломная_работа
{
    public partial class Form6 : Form
    {
        connection con = new connection("", @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + "\\database.mdf;Integrated Security=True;");
        int id_polic;
        int id_stat;


        public Form6()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            con.smena_zaprosa("select Id_us, fio from user_system where v_shtate = 'false'");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                    comboBox1.Items.Add("" + con.reader.GetInt32(0).ToString() + " " + con.reader.GetString(1));
            }
            con.connection_close();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            int item_id = 0;
            con.smena_zaprosa("select * from user_system where Id_us = " + comboBox1.Text.Remove(4) + "");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                {
                    id_polic = con.reader.GetInt32(6);
                    id_stat = con.reader.GetInt32(2);
                    textBox3.Text = con.reader.GetString(3);
                }
            }
            con.connection_close();
            con.smena_zaprosa("select * from department where Id_department = " + id_polic + "");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                {
                    textBox4.Text = "" + con.reader.GetInt32(0) + " " + con.reader.GetInt32(1) + " " + con.reader.GetString(2) + " " + con.reader.GetString(3);
                    item_id = con.reader.GetInt32(1);
                }
            }
            con.connection_close();
            con.smena_zaprosa("select nm_item from item where id = " + item_id + "");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                    textBox4.Text = textBox4.Text.Replace(item_id.ToString(), con.reader.GetString(0).ToString());
            }
            con.connection_close();
            con.smena_zaprosa("select * from statys where id = " + id_stat + "");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                    textBox2.Text = "" + con.reader.GetInt32(0) + " " + con.reader.GetString(1);
            }
            con.connection_close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                con.smena_zaprosa("update user_system set v_shtate = 'true' where Id_us = " + comboBox1.Text.Remove(4) + "");
                con.connection_open();
                con.run_qwery();
                con.connection_close();
                MessageBox.Show("Пользователь получил доступ к системе");
            }
            else
                MessageBox.Show("Заполните не заполненные поля", "Проверка заполнеиня", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}
