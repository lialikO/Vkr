using Microsoft.VisualBasic.Logging;
using MySql.Data.MySqlClient;
using testvar;
using Vkr;
namespace WinFormsApp1
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            if (AutClass.vhod == false)
            {
                this.Hide();
                autForm autForm = new autForm();
                autForm.ShowDialog();
                if (!AutClass.IsAuthenticated)
                {
                    Application.Exit();
                }
            }
               
                toolStripStatusLabel1.Text = $"Здравствуйте {AutClass.aut_fio}! Вы вошли как  {AutClass.aut_role_title.ToString()}";
            switch (AutClass.aut_role)
            {
                case 3:
                    button1.Enabled = true;

                    break;
                case 2:
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button2.Visible = true;
                    button3.Enabled = true;
                    button3.Visible = true;
                    break;
                case 1:
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button2.Visible = true;
                    button3.Enabled = true;
                    button3.Visible = true;
                    button4.Enabled = true;
                    button4.Visible = true;

                    break;
                default:
                    MessageBox.Show("А вы кто?Уходите!");
                    Application.Exit();
                    break;
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            testForm testForm = new testForm();
            testForm.ShowDialog();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            user_adm user_Adm = new user_adm();
            user_Adm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            redTest redTest = new redTest();
            redTest.ShowDialog();
        }
    }
}
