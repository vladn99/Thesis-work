using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Дипломная_работа
{
    class connection
    {
        private static string ConnectionString;
        public SqlConnection constr;
        private string sqlExpression = "";
        public SqlCommand command;
        public SqlDataReader reader;

        public connection(string sqlExpression, string Connection_String)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath.Replace(@"\bin\Debug", ""));
            ConnectionString = Connection_String;
            smena_zaprosa(sqlExpression);
        }

        public void connection_open()
        {
            constr = new SqlConnection(ConnectionString);
            constr.Open();
        }

        public void connection_close()
        {
            constr.Close();
        }

        public void run_qwery()
        {
            command = new SqlCommand(sqlExpression, constr);
            command.ExecuteNonQuery();
        }

        public void data_reader()
        {
            command = new SqlCommand(sqlExpression, constr);
            reader = command.ExecuteReader();
        }

        public void smena_zaprosa(string sqlExpression)
        {
            this.sqlExpression = sqlExpression;
        }
    }
}
