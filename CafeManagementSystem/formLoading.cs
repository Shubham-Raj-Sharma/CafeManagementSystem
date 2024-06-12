using System;
using System.Threading;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    public partial class formLoading : Form
    {
        public formLoading()
        {
            InitializeComponent();
        }

        private void formLoading_Load(object sender, EventArgs e)
        {
            //Get Cafe Name and Location
            labelCafeName.Text = "";
            labelLocation.Text = "";
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            panelSlider.Width += 2;

            if (panelSlider.Width >= panel1.Width)
            {
                timer1.Enabled = false;
                formLogin f1 = new formLogin();
                f1.Show();
                this.Hide();
            }
        }
    }
}
