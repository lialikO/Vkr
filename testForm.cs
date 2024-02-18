using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        int nomerVop=1;
        int nomerTest;
        public void metVopr(int nomerV, int nomerT)
        {
            string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
            //Переменная соединения
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
                $"WHERE T_Test.idTestov = '{nomerT}' and T_Vopros.nomerVop = '{nomerV}";
            MySqlCommand comVop = new MySqlCommand(GetNameTest, conn);
            MySqlDataReader read = comVop.ExecuteReader();
            while (read.Read())
            {
                label4.Text = $"Вопрос {read.GetString(1)}) {read.GetString(0)}";

            }
            read.Close();

            conn.Close();
        }

        private  void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            nomerTest=comboBox1.SelectedIndex+1;
            string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
            //Переменная соединения
            MySqlConnection conn;
            //Инициализация подключения
            conn = new MySqlConnection(connStr);
            //Открываем соединения
            conn.Open();
            // string GetNameTest = $"SELECT " +
            //    $"T_Test.nameTest " +
            //    $"FROM T_Test " +
            //    $"WHERE T_Test.idTestov = '{nomerTest} '";
            string GetNameTest = $"SELECT " +
                $"T_Test.nameTest, " +
                $"T_Vopros.nomerVop, " +
                $"T_Vopros.vopros, " +
                $"T_Otv.Otv, " +
                $"T_Otv.verno " +
                $"FROM T_Test " +
                $"INNER JOIN T_Vopros ON  T_Test.idTestov = T_Vopros.idTest " +
                $"INNER JOIN T_Otv ON  T_Vopros.idVoprosa = T_Otv.idVop " +
                $"WHERE T_Test.idTestov = 1";

            // объект для выполнения SQL-запроса
            MySqlCommand commandGetDataUser = new MySqlCommand(GetNameTest, conn);
            // объект для чтения ответа сервера
            MySqlDataReader read = commandGetDataUser.ExecuteReader();
            // читаем результат
            while (read.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                label3.Text = read.GetString(0);
                

            }
            read.Close();
            
            conn.Close();
            metVopr(nomerVop, nomerTest);
            
            //label4.Text = b.ToString();


        }
    }
}
