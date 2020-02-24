using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class FoodPlanner : Form
    {
        public FoodPlanner()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            standardMode.Visible = true;
            standardMode.BringToFront();
            panel1.BackColor = Color.Teal;
            label1.BackColor = Color.Teal;
            user1.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            modeFitness1.BringToFront();
            modeFitness1.Visible = true;
            panel1.BackColor = Color.Crimson;
            label1.BackColor = Color.Crimson;
            user1.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            modeKids1.BringToFront();
            modeKids1.Visible = true;
            panel1.BackColor = Color.DarkOrange;
            label1.BackColor = Color.DarkOrange;
            user1.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            user1.Visible = true;
            user1.BringToFront();
            /*standardMode.Visible = false;
            modeKids1.Visible = false;
            modeFitness1.Visible = false;*/
        }

        private void modeFitness1_Load(object sender, EventArgs e)
        {

        }

        private void modeKids1_Load(object sender, EventArgs e)
        {

        }
    }
}
