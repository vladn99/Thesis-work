using System;
using System.Drawing;
using System.Windows.Forms;

namespace Дипломная_работа
{
    public partial class Form4 : Form
    {
        connection con = new connection("", @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + "\\database.mdf;Integrated Security=True;");
        public string us_system { get; set; }
        int id_sys;
        Panel pn;
        Label txt_name, txt_opis;
        Button btn;
        int schet = 0;

        public Form4()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
        }

        //ввод только буквенных значений
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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
                    con.smena_zaprosa("select * from statement where Id_statement = " + id + "");
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

        private void Form4_Load(object sender, EventArgs e)
        {
            button3.Visible = false;
            textBox3.Text = us_system;
            init_components();
            for_paint("select * from  statement");
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            clear_form();
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                int id = get_id();
                con.smena_zaprosa("insert into statement(Id_statement, dat_supplies, dat_accident, fio, phone, description_statement, Id_us) values (" + id + ", N'" + dateTimePicker1.Text + "', N'" + dateTimePicker2.Text + "', N'" + textBox1.Text + "', N'" + maskedTextBox1.Text + "', N'" + textBox2.Text + "', " + textBox3.Text.Remove(4) + ")");
                con.connection_open();
                con.run_qwery();
                con.connection_close();
                MessageBox.Show("Данные записаны");
                button3.Visible = true;
                id_sys = id;
                clear_form();
                init_components();
                for_paint("select * from  statement");
            }
            else
                MessageBox.Show("Заполните не заполненные поля", "Проверка заполнеиня", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void init_components()
        {
            panel1.Visible = true;
            pn = panel1;
            txt_name = label1;
            txt_opis = label2;
            btn = button1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 form5 = new Form5();
            form5.id_statement = (sender as Button).Name;
            form5.Show();
        }

        private void paint_form(int name, string txt_ops)
        {
            Panel nwpn = new Panel();
            nwpn.Name = "panel" + (schet + 2);
            nwpn.Height = pn.Height;
            nwpn.Width = pn.Width;
            if (schet == 0)
                nwpn.Location = new Point(pn.Location.X, pn.Location.Y);
            else
                nwpn.Location = new Point(pn.Location.X, pn.Location.Y + pn.Height + 20);
            nwpn.BorderStyle = pn.BorderStyle;
            nwpn.BackColor = pn.BackColor;
            nwpn.AutoScroll = pn.AutoScroll;
            tabPage1.Controls.Add(nwpn);
            pn = nwpn;
            Button newbtn = new Button();
            newbtn.Name = name.ToString();
            newbtn.Height = btn.Height;
            newbtn.Width = btn.Width;
            newbtn.Location = new Point(btn.Location.X, btn.Location.Y);
            newbtn.Text = btn.Text;
            newbtn.FlatStyle = btn.FlatStyle;
            newbtn.FlatAppearance.BorderColor = btn.FlatAppearance.BorderColor;
            newbtn.FlatAppearance.BorderSize = btn.FlatAppearance.BorderSize;
            newbtn.ForeColor = btn.ForeColor;
            newbtn.BackColor = Color.LightGray;
            newbtn.Font = btn.Font;
            newbtn.Click += new EventHandler(button1_Click);
            pn.Controls.Add(newbtn);
            btn = newbtn;
            Label newname = new Label();
            newname.Height = txt_name.Height;
            newname.Width = txt_name.Width;
            newname.Location = new Point(txt_name.Location.X, txt_name.Location.Y);
            newname.TextAlign = txt_name.TextAlign;
            newname.Font = txt_name.Font;
            newname.Name = "label_nm" + (schet + 2);
            newname.AutoSize = txt_name.AutoSize;
            newname.Text = name.ToString();
            pn.Controls.Add(newname);
            txt_name = newname;
            Label newopis = new Label();
            newopis.Height = txt_opis.Height;
            newopis.Width = txt_opis.Width;
            newopis.Location = new Point(txt_opis.Location.X, txt_opis.Location.Y);
            newopis.TextAlign = txt_opis.TextAlign;
            newopis.Font = txt_opis.Font;
            newopis.Name = "label_ops" + (schet + 2);
            newopis.AutoSize = txt_opis.AutoSize;
            newopis.Text =txt_ops;
            pn.Controls.Add(newopis);
            txt_opis = newopis;
            schet++;
        }

        private void for_paint(string qwery)
        {
            con.smena_zaprosa(qwery);
            con.connection_open();
            con.data_reader();
            if (con.reader.HasRows)
            {
                while (con.reader.Read())
                    paint_form(con.reader.GetInt32(0), con.reader.GetString(5));
            }
            else
            {
                MessageBox.Show("Нет данных в БД");
                panel1.Visible = false;
            }
            panel1.Visible = false;
            con.connection_close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            print print = new print();
            print.printer(id_sys, textBox1.Text, textBox2.Text, dateTimePicker1.Text, dateTimePicker2.Text, textBox3.Text.Remove(0, 5));
        }

        private void clear_form()
        {
            Panel panel;
            for (int i = 0; i < schet; i++)
            {
                panel = (Panel)tabPage1.Controls.Find("panel" + (i + 2), true)[0];
                tabPage1.Controls.Remove(panel);
                panel.Dispose();
            }
            schet = 0;
        }
    }
}
