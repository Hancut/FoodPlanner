using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class User : UserControl
    {
        string connection = "Data Source=DESKTOP-H7KJBCU;Initial Catalog=FoodPlanner;Integrated Security=True";
        double totalCalories = 0;
        public User()
        {
            InitializeComponent();
        }

        private void User_VisibleChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                var command = new SqlCommand("printCal", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@username", SqlDbType.Char);
                command.Parameters["@username"].Value = "ana";
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                double cal = reader.GetDouble(0);
                reader.Close();

                totalCalories += cal;
                calories.Text = cal.ToString();

                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Food", con);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);

                dataGridView1.DataSource = dtbl;

                command = new SqlCommand("calcRC", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                int maxim;
                Int32.TryParse(textBox2.Text, out maxim);
                command.Parameters.Add("@maxim", SqlDbType.Int);
                command.Parameters["@maxim"].Value = maxim;
                command.ExecuteNonQuery();

                reader = command.ExecuteReader();
                reader.Read();
                Int32 rcal = reader.GetInt32(0);
                rcalories.Text = rcal.ToString();
                reader.Close();

                command = new SqlCommand("youcaneat", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                Int32.TryParse(textBox2.Text, out maxim);
                command.Parameters.Add("@maxim", SqlDbType.Int);
                command.Parameters["@maxim"].Value = rcal;
                command.ExecuteNonQuery();

                label6.Text = "You can eat:" + Environment.NewLine;
                reader = command.ExecuteReader();
                while(reader.Read())
                {
                    string food = reader.GetString(0);
                    label6.Text += Environment.NewLine + food;
                }
                rcalories.Text = rcal.ToString();
                reader.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Food", con);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);

                dataGridView1.DataSource = dtbl;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                var command = new SqlCommand("calcRC", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                int maxim;
                Int32.TryParse(textBox2.Text, out maxim);
                command.Parameters.Add("@maxim", SqlDbType.Int);
                command.Parameters["@maxim"].Value = maxim;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Int32 rcal = reader.GetInt32(0);
                rcalories.Text = rcal.ToString();
                reader.Close();
            }
        }
    }
}
