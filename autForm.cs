using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WinFormsApp1
{
    public partial class autForm : Form
    {
        public autForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string log = textBox1.Text;
            string pass = textBox2.Text;

            string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
            //Переменная соединения
            MySqlConnection conn;
            //Инициализация подключения
            conn = new MySqlConnection(connStr);
            //Открываем соединения
            conn.Open();
            //Составляем запрос
            string querySql = $"SELECT COUNT(*) FROM T_Users WHERE loginUsers='{log}' and passUsers='{pass}' and enabledUsers=1";
            //Формируем комманду
            MySqlCommand authCom = new MySqlCommand(querySql, conn);
            //Выполняем команду
            string result = authCom.ExecuteScalar().ToString();

            if (Convert.ToInt32(result) > 0)
            {

                string queryGetDataUser = $"SELECT " +
                    $"T_Users.idUsers, " +
                    $"T_Users.loginUsers, " +
                    $"T_Users.passUsers, " +
                    $"T_Users.fioUsers, 	" +
                    $"T_Users.enabledUsers, 	" +
                    $"T_Role.titleRole, 	" +
                    $"T_Users.roleUsers " +
                    $"FROM	T_Role	" +
                    $"INNER JOIN 	T_Users	ON 		T_Role.idRole = T_Users.roleUsers " +
                    $"WHERE T_Users.loginUsers='{log}' and T_Users.passUsers='{pass}' and T_Users.enabledUsers=1";
                // объект для выполнения SQL-запроса
                MySqlCommand commandGetDataUser = new MySqlCommand(queryGetDataUser, conn);
                // объект для чтения ответа сервера
                MySqlDataReader reader = commandGetDataUser.ExecuteReader();
                // читаем результат
                while (reader.Read())
                {
                    // элементы массива [] - это значения столбцов из запроса SELECT
                    AutClass.aut_id = Convert.ToInt32(reader[0].ToString());
                    AutClass.aut_fio = reader[3].ToString();
                    AutClass.aut_role = Convert.ToInt32(reader[6].ToString());
                    AutClass.aut_role_title = reader[5].ToString();

                }
                reader.Close(); // закрываем reader

                MessageBox.Show($"Авторизация пользователя {log} успешна.");
                AutClass.IsAuthenticated = true;
                AutClass.vhod = true;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Авторизация пользователя {log} не удалась.");
                AutClass.IsAuthenticated = false;
                textBox1.Clear();
                textBox2.Clear();
            }
            conn.Close();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
