using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using MySql.Data.MySqlClient;
using WinFormsApp1;

namespace Vkr
{
    public partial class testForm : Form
    {
        public testForm()
        {
            InitializeComponent();
        }
        Otveti[] vopOtv;
        int kolVop;
        int nomerVop = 1;
        int nomerTest;
        int i; //Переменная для массивов
        string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
        //Переменная соединения
        public class Otveti
        {
            public string vopros;
            public string prOtv;
            public string otvSt;
            public Otveti(string vopros)
            {
                this.vopros = vopros;

            }


        };
        public void metVopr(int nomerV, int nomerT)
        {

            MySqlConnection conn;
            //Инициализация подключения
            conn = new MySqlConnection(connStr);
            //Открываем соединения
            conn.Open();
            string GetNameTest = $"SELECT " +
                $"T_Vopros.vopros,  " +
                $"T_Vopros.nomerVop " +
                $"FROM T_Vopros " +
                $"INNER JOIN T_Test ON  T_Vopros.idTest = T_Test.idTestov " +
                $"WHERE T_Test.idTestov = '{nomerT}' and T_Vopros.nomerVop = '{nomerV}'";
            MySqlCommand comVop = new MySqlCommand(GetNameTest, conn);
            MySqlDataReader read = comVop.ExecuteReader();
            i = nomerVop - 1;
            while (read.Read())
            {

                //label4.Text = $"Вопрос {read.GetString(1)}) {read.GetString(0)}";
                Otveti vop1 = new Otveti(read.GetString(0));
                vopOtv[i] = vop1;
                label4.Text = $"Вопрос {read.GetString(1)}) {vopOtv[i].vopros}";


            }

            read.Close();
            GetNameTest = $"SELECT " +
                $"T_Otv.Otv, " +
                $"T_Otv.verno " +
                $"FROM T_Vopros " +
                $"INNER JOIN T_Otv ON  T_Vopros.idVoprosa = T_Otv.idVop " +
                $"WHERE T_Vopros.idTest = '{nomerT}' AND T_Vopros.nomerVop = '{nomerV}'";

            MySqlCommand comOtv = new MySqlCommand(GetNameTest, conn);
            read = comOtv.ExecuteReader();
            string[] otv = new string[4];
            int idmas = 0;
            int ver = 0;
            while (read.Read())
            {
                otv[idmas] = $"{read.GetString(0)}";
                ver = read.GetInt32(1);
                if (ver > 0)
                {
                    vopOtv[i].prOtv = read.GetString(0);
                }
                //MessageBox.Show(vopOtv[i].prOtv);
                idmas++;
            }
            button1.Text = otv[0];
            button2.Text = otv[1];
            button3.Text = otv[2];
            button4.Text = otv[3];

            conn.Close();
        }

        public void GetComboBoxList()
        {
            MySqlConnection conn;
            conn = new MySqlConnection(connStr);
            //Формирование списка статусов
            DataTable list_stud_table = new DataTable();
            MySqlCommand list_stud_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_stud_table.Columns.Add(new DataColumn("idTestov", System.Type.GetType("System.Int32")));
            list_stud_table.Columns.Add(new DataColumn("nameTest", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_stud_table;
            comboBox1.DisplayMember = "nameTest";
            comboBox1.ValueMember = "idTestov";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_users = "SELECT T_Test.idTestov,  T_Test.nameTest FROM T_Test";
            list_stud_command.CommandText = sql_list_users;
            list_stud_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_stud_reader;
            try
            {
                //Инициализируем ридер
                list_stud_reader = list_stud_command.ExecuteReader();
                while (list_stud_reader.Read())
                {
                    DataRow rowToAdd = list_stud_table.NewRow();
                    rowToAdd["idTestov"] = Convert.ToInt32(list_stud_reader[0]);
                    rowToAdd["nameTest"] = list_stud_reader[1].ToString();
                    list_stud_table.Rows.Add(rowToAdd);
                }
                list_stud_reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения списка ролей \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            nomerTest = comboBox1.SelectedIndex + 1;
            //string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
            //Переменная соединения
            MySqlConnection conn;
            //Инициализация подключения
            conn = new MySqlConnection(connStr);
            //Открываем соединения
            conn.Open();
            string GetNameTest = $"SELECT " +
                $"T_Test.nameTest, " +
                $"T_Test.kolVop " +
                $"FROM " +
                $"T_Test " +
                $"WHERE T_Test.idTestov = '{nomerTest} '";



            // объект для выполнения SQL-запроса
            MySqlCommand commandGetDataUser = new MySqlCommand(GetNameTest, conn);
            // объект для чтения ответа сервера
            MySqlDataReader read = commandGetDataUser.ExecuteReader();
            // читаем результат
            while (read.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                label3.Text = read.GetString(0);
                kolVop = read.GetInt32(1);
                string a = kolVop.ToString();

            }
            read.Close();

            conn.Close();
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            vopOtv = new Otveti[kolVop];
            metVopr(nomerVop, nomerTest);




        }
        public void prov(int i)
        {
            if (vopOtv[i].otvSt == vopOtv[i].prOtv)
            {
                MessageBox.Show($"Ваш ответ - {vopOtv[i].otvSt}\n Правильный!");
            }
            else
            {
                MessageBox.Show($"Ваш ответ - {vopOtv[i].otvSt}\n Неправильный!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vopOtv[i].otvSt = button1.Text;
            prov(i);
            if (nomerVop < kolVop) { nomerVop++; metVopr(nomerVop, nomerTest); }
            else
            {
                button4.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                button6.Visible = true;
            };

        }

        private void button2_Click(object sender, EventArgs e)
        {
            vopOtv[i].otvSt = button2.Text;
            prov(i);
            if (nomerVop < kolVop) { nomerVop++; metVopr(nomerVop, nomerTest); }
            else
            {
                button4.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                button6.Visible = true;
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vopOtv[i].otvSt = button3.Text;
            prov(i);
            if (nomerVop < kolVop) { nomerVop++; metVopr(nomerVop, nomerTest); }
            else
            {
                button4.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                button6.Visible = true;
            };
        }

        private void button4_Click(object sender, EventArgs e)
        {
            vopOtv[i].otvSt = button4.Text;
            prov(i);
            if (nomerVop < kolVop) { nomerVop++; metVopr(nomerVop, nomerTest); }
            else
            {
                button4.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                button6.Visible = true;

            };
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int kolPrOtv = 0;
            for (int a = 0; a < kolVop; a++)
            {
                if (vopOtv[a].otvSt == vopOtv[a].prOtv)
                {
                    kolPrOtv++;
                }

            }
            string res = $"{kolPrOtv}/{kolVop}";
            MessageBox.Show($"Ваш результат {res}");
            MySqlConnection conn;
            //Инициализация подключения
            conn = new MySqlConnection(connStr);
            //Открываем соединения
            //conn.Open();
            string fioRes = textBox1.Text;
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);

            string GetResTest = $"INSERT INTO T_Result (fioRes, idTesta, resilt,data) " +
                $"VALUES ('{fioRes}','{nomerTest}','{res}','{date}' )";
            try
            {
                conn.Open();
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(GetResTest, conn);
                // выполняем запрос
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Добавление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления пользователя\n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
            AutClass.vhod = true;
            mainForm mainForm = new mainForm();
            mainForm.Visible = true;
        }

        private void testForm_Load(object sender, EventArgs e)
        {
            GetComboBoxList();
        }
    }
}
