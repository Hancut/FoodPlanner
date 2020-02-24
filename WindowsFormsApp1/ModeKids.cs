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
    public partial class ModeKids : UserControl
    {
        string connection = "Data Source=DESKTOP-H7KJBCU;Initial Catalog=FoodPlanner;Integrated Security=True";

        public ModeKids()
        {
            InitializeComponent();
            connectionSql("Kids", "");
        }

        private void snacksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectionSql("Kids", "");
        }

        private void breakfastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List.Controls.Clear();
            connectionSql("Kids", "Breakfast");
        }

        private void lunchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List.Controls.Clear();
            connectionSql("Kids", "Lunch");
        }

        private void dinnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List.Controls.Clear();
            connectionSql("Kids", "Dinner");
        }

        private void connectionSql(string type, string time)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                var command = new SqlCommand("selectFood", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@type", SqlDbType.Char);
                command.Parameters["@type"].Value = type;
                command.Parameters.Add("@time", SqlDbType.Char);
                command.Parameters["@time"].Value = time;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0).Trim();
                    PictureBox picFood = new PictureBox();
                    Label nameFood = new Label();

                    // Set basic 
                    string imageName = @"C:\Users\ancad\source\repos\WindowsFormsApp1\WindowsFormsApp1\Resources\" + name.Trim() + ".png";
                    nameFood.Text = name;
                    nameFood.Click += new EventHandler(label_Click);
                    nameFood.Tag = name;
                    picFood.BackgroundImageLayout = ImageLayout.Stretch;
                    picFood.Image = Image.FromFile(imageName);
                    picFood.SizeMode = PictureBoxSizeMode.StretchImage;
                    picFood.Click += new EventHandler(pb_Click);
                    picFood.Tag = name;

                    // Set details
                    picFood.Size = new System.Drawing.Size(300, 300);
                    nameFood.Font = new Font(System.Drawing.FontFamily.GenericSerif, 22, FontStyle.Bold);
                    nameFood.AutoSize = true;
                    nameFood.MaximumSize = new Size(400, 1000);

                    this.List.Controls.Add(picFood);
                    this.List.Controls.Add(nameFood);

                }
            }
        }

        void pb_Click(object sender, EventArgs e)
        {
            PictureBox picture = sender as PictureBox;
            PictureBox pictureBox = new PictureBox();
            Label label = new Label();

            // save corect label for the picture (name of the food)
            pictureBox = picture;
            foreach (Control item in this.List.Controls)
            {
                if ((item.GetType() == typeof(Label)) && item.Tag.Equals(picture.Tag))
                {
                    label = (Label)item;
                }
            }

            // eliminate rest of the food
            this.List.Controls.Clear();
            this.List.Controls.Add(pictureBox);
            this.List.Controls.Add(label);


            // set connection
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                // call function for calculate how much calories has *foodName*
                string query = @"SELECT [dbo].[CalculateCalories](@foodName) AS calories;";
                var command = new SqlCommand(query, con);

                command.Parameters.Add("@foodName", SqlDbType.Char);
                command.Parameters["@foodName"].Value = pictureBox.Tag;
                command.ExecuteNonQuery();

                // get result
                Int32 finalCalories = (Int32)command.ExecuteScalar();

                // print it
                label.Text = label.Tag.ToString().Trim() + Environment.NewLine + Environment.NewLine + finalCalories.ToString() + " calories";

                // get ingredients
                command = new SqlCommand("selectIngredients", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@foodName", SqlDbType.Char);
                command.Parameters["@foodName"].Value = pictureBox.Tag;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                // get list of ingredients
                string ingr = reader.GetString(0);
                label.Text += Environment.NewLine + Environment.NewLine + "Ingredients:" + Environment.NewLine + ingr;

                // add as eaten
                command = new SqlCommand("addEatenFood", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                reader.Close();

                var rand = new Random();
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = rand.Next() % 100;
                command.Parameters.Add("@name", SqlDbType.Char);
                command.Parameters["@name"].Value = pictureBox.Tag.ToString().Trim();
                command.ExecuteNonQuery();
            }


        }

        void label_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            Label cpyLabel = new Label();
            PictureBox pictureBox = new PictureBox();

            cpyLabel = label;
            foreach (Control item in this.List.Controls)
            {
                if ((item.GetType() == typeof(PictureBox)) && item.Tag.Equals(label.Tag))
                {
                    pictureBox = (PictureBox)item;
                }
            }
            this.List.Controls.Clear();
            this.List.Controls.Add(pictureBox);
            this.List.Controls.Add(cpyLabel);

            // set connection
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                // call function for calculate how much calories has *foodName*
                string query = @"SELECT [dbo].[CalculateCalories](@foodName) AS calories;";
                var command = new SqlCommand(query, con);

                command.Parameters.Add("@foodName", SqlDbType.Char);
                command.Parameters["@foodName"].Value = pictureBox.Tag;
                command.ExecuteNonQuery();

                // get result
                Int32 finalCalories = (Int32)command.ExecuteScalar();

                // print it
                label.Text = label.Tag.ToString().Trim() + Environment.NewLine + Environment.NewLine + finalCalories.ToString() + " calories";

                // get ingredients
                command = new SqlCommand("selectIngredients", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@foodName", SqlDbType.Char);
                command.Parameters["@foodName"].Value = pictureBox.Tag;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                // get list of ingredients
                string ingr = reader.GetString(0);
                label.Text += Environment.NewLine + Environment.NewLine + "Ingredients:" + Environment.NewLine + ingr;
            }

        }


    }
}
