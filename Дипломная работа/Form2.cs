using System;
using System.Windows.Forms;

namespace Дипломная_работа
{
    public partial class Form2 : Form
    {
        connection con = new connection("", @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + "\\database.mdf;Integrated Security=True;");

        public Form2()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
        }

        //ввод только буквенных значений
        private void vvod_bukw(KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (Char.IsDigit(number) && number != 8)
                e.Handled = true;
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
                    con.smena_zaprosa("select * from department where Id_department = " + id + "");
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            vvod_bukw(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text != "")
            {
                int id = get_id();
                con.smena_zaprosa("insert into department(Id_department, region, point, adres) values (" + id + ", " + comboBox1.Text.Remove(4) + ", N'" + textBox2.Text + "', N'" + textBox3.Text + "')");
                con.connection_open();
                con.run_qwery();
                con.connection_close();
                MessageBox.Show("Данные записаны");
            }
            else
                MessageBox.Show("Заполните не заполненные поля", "Проверка заполнеиня", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            con.smena_zaprosa("select * from item");
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                {
                    comboBox1.Items.Add(con.reader.GetInt32(0) + " " + con.reader.GetString(1));
                }
            }
            else
            {
                comboBox1.Items.Add("Нет данных");
            }
            con.connection_close();
        }
    }
}
