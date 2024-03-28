using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class modTest : Form
    {
        //Переменная соединения
        MySqlConnection conn;
        //DataAdapter представляет собой объект Command , получающий данные из источника данных.
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
        private BindingSource bSource = new BindingSource();
        //DataSet - расположенное в оперативной памяти представление данных, обеспечивающее согласованную реляционную программную 
        //модель независимо от источника данных.DataSet представляет полный набор данных, включая таблицы, содержащие, упорядочивающие 
        //и ограничивающие данные, а также связи между таблицами.
        private DataSet ds = new DataSet();
        //Представляет одну таблицу данных в памяти.
        private DataTable table = new DataTable();
        //Переменная для ID записи в БД, выбранной в гриде. Пока она не содердит значения, лучше его инициализировать с 0
        //что бы в БД не отправлялся null
        string id_selected_rows = "0";

        public modTest()
        {
            InitializeComponent();
        }
        //Метод обновления DataGreed
        public void reload_list()
        {
            //Чистим виртуальную таблицу
            table.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListUsers();
        }
        public void GetListUsers()
        {
            //Объявление запроса
            string sqlQueryLoadUsers = "SELECT " +
                "T_Test.nameTest as 'Название теста', " +
                "T_Vopros.nomerVop 'Номер вопроса', " +
                "T_Vopros.vopros 'Вопрос', " +
                "T_Otv.Otv  'Ответ'," +
                "T_Otv.verno 'Верный?'," +
                "FROM T_Test " +
                "INNER JOIN T_Vopros ON T_Test.idTestov = T_Vopros.idTest " +
                "INNER JOIN T_Otv ON  T_Vopros.idVoprosa = T_Otv.idVop "; //+
                //"WHERE T_Test.idTestov = 1"

            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(sqlQueryLoadUsers, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;
            //Закрываем соединение
            conn.Close();

            //Отражаем количество записей в ДатаГриде
            int count_rows = dataGridView1.RowCount;
            toolStripStatusLabel2.Text = (count_rows).ToString();
        }
        public void DeleteUser()
        {
            //Формируем строку запроса на добавление строк
            string sql_delete_user = "DELETE FROM T_Users WHERE idUsers='" + id_selected_rows + "'";
            //Посылаем запрос на обновление данных
            MySqlCommand delete_user = new MySqlCommand(sql_delete_user, conn);
            try
            {
                conn.Open();
                delete_user.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                //Вызов метода обновления ДатаГрида
                reload_list();
            }
        }

        private void user_adm_Load(object sender, EventArgs e)
        {
            string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
            //Переменная соединения

            //Инициализация подключения
            conn = new MySqlConnection(connStr);


            GetListUsers();
            GetComboBoxList();
            GetComboBoxList2();

            //Видимость полей в гриде 
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[4].Visible = true;

            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[3].FillWeight = 15;
            dataGridView1.Columns[4].FillWeight = 15;

            //Режим для полей "Только для чтения"
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;

            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true; 
        }
        public void GetComboBoxList()
        {
            //Формирование списка статусов
            DataTable list_test_table = new DataTable();
            MySqlCommand list_test_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_test_table.Columns.Add(new DataColumn("idTestov", System.Type.GetType("System.Int32")));
            list_test_table.Columns.Add(new DataColumn("nameTest", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_test_table;
            comboBox1.DisplayMember = "nameTest";
            comboBox1.ValueMember = "idTestov";
            //Формируем строку запроса на отображение списка тестов
            string sql_list_users = "SELECT T_Test.idTestov, T_Test.nameTest FROM T_Test";
            list_test_command.CommandText = sql_list_users;
            list_test_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_test_reader;
            try
            {
                //Инициализируем ридер
                list_test_reader = list_test_command.ExecuteReader();
                while (list_test_reader.Read())
                {
                    DataRow rowToAdd = list_test_table.NewRow();
                    rowToAdd["idTestov"] = Convert.ToInt32(list_test_reader[0]);
                    rowToAdd["nameTest"] = list_test_reader[1].ToString();
                    list_test_table.Rows.Add(rowToAdd);
                }
                list_test_reader.Close();
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

        public void GetComboBoxList2()
        {
            //Формирование списка статусов
            DataTable list_vop_table = new DataTable();
            MySqlCommand list_vop_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_vop_table.Columns.Add(new DataColumn("nomerVop", System.Type.GetType("System.Int32")));            
            //Настройка видимости полей комбобокса
            comboBox2.DataSource = list_vop_table;
            comboBox2.DisplayMember = "nomerVop";
            comboBox2.ValueMember = "nomerVop";
            int nT = comboBox1.Items.Count;
            //Формируем строку запроса на отображение списка вопросов
            string sql_list_vop = $"SELECT T_Vopros.nomerVop FROM T_Vopros WHERE T_Vopros.idTest = {nT}";
            list_vop_command.CommandText = sql_list_vop;
            list_vop_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_vop_reader;
            try
            {
                //Инициализируем ридер
                list_vop_reader = list_vop_command.ExecuteReader();
                while (list_vop_reader.Read())
                {
                    DataRow rowToAdd = list_vop_table.NewRow();
                    rowToAdd["nomerVop"] = Convert.ToInt32(list_vop_reader[0]);                   
                    list_vop_table.Rows.Add(rowToAdd);
                }
                list_vop_reader.Close();
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

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }
        //Метод получения ID выделенной строки, для последующего вызова его в нужных методах
        public void GetSelectedIDString()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_rows;
            //Индекс выбранной строки
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
            //Указываем ID выделенной строки в метке
            toolStripStatusLabel4.Text = id_selected_rows;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Магические строки
            dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            dataGridView1.CurrentRow.Selected = true;
            //Метод получения ID выделенной строки в глобальную переменную
            GetSelectedIDString();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                //dataGridView1.CurrentRow.Selected = true;
                dataGridView1.CurrentCell.Selected = true;
                //Метод получения ID выделенной строки в глобальную переменную
                GetSelectedIDString();
            }
        }
        public void ChangeStatusEmploy(string new_state)
        {
            //Получаем ID изменяемого студента
            string redact_id = id_selected_rows;
            // запрос обновления данных
            string query2 = $"UPDATE T_Users SET enabledUsers='{new_state}' WHERE (idUsers='{id_selected_rows}')";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(query2, conn);


            try
            {
                // устанавливаем соединение с БД
                conn.Open();
                // выполняем запрос
                command.ExecuteNonQuery();
                MessageBox.Show("Изменение статуса прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                // закрываем подключение к БД
                conn.Close();
                //Обновляем DataGrid
                reload_list();
            }
        }
     
        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button1.Visible = false;
            button3.Visible = true;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT * FROM T_Users WHERE idUsers={id_selected_rows}";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                textBox1.Text = reader[1].ToString();
                textBox2.Text = reader[2].ToString();
                textBox3.Text = reader[5].ToString();
                textBox4.Text = reader[3].ToString();
                comboBox1.SelectedValue = reader[4].ToString();

            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user_login = textBox1.Text;
            string password = textBox2.Text;
            string fio = textBox3.Text;
            string status = textBox4.Text;
            int role = Convert.ToInt32(comboBox1.SelectedValue);
            //int role = Convert.ToInt32(comboBox1.SelectedIndex + 1);

            string sql = $"INSERT INTO T_Users (loginUsers, passUsers, enabledUsers, roleUsers, fioUsers) " +
                $"VALUES ('{user_login}', '{password}', {status}, {role.ToString()}, '{fio}')";

            try
            {
                conn.Open();
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(sql, conn);
                // выполняем запрос
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Добавление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reload_list();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления пользователя\n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Но в любом случае, нужно закрыть соединение
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user_login = textBox1.Text;
            string password = textBox2.Text;
            string fio = textBox3.Text;
            string status = textBox4.Text;
            int role = Convert.ToInt32(comboBox1.SelectedValue);
            //int role = Convert.ToInt32(comboBox1.SelectedIndex+1);
            //Получаем ID изменяемого студента
            string redact_id = id_selected_rows;
            // запрос обновления данных
            string query2 = $"UPDATE T_Users " +
                $"SET " +
                $"loginUsers = '{user_login}', " +
                $"passUsers = '{password}', " +
                $"enabledUsers = {status}, " +
                $"roleUsers = {role}, " +
                $"fioUsers = '{fio}' " +
                $"WHERE " +
                $"idUsers = {id_selected_rows}";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(query2, conn);


            try
            {
                // устанавливаем соединение с БД
                conn.Open();
                // выполняем запрос
                command.ExecuteNonQuery();
                MessageBox.Show("Изменение прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                // закрываем подключение к БД
                conn.Close();
                //Обновляем DataGrid
                reload_list();
            }
        }
    }
}
