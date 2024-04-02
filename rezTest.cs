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
using ClosedXML.Excel;
using System.IO;

namespace testvar
{
    public partial class rezTest : Form
    {
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

        public rezTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            table.Clear();
            //Переменная для запроса по ФИО
            string fioStud = textBox1.Text;
            //Объявление запроса
            string sqlQueryLoadUsers = "SELECT " +
                "T_Result.idTesta as 'Номер теста' , " +
                "T_Test.nameTest as 'Название теста' , " +
                "T_Result.fioRes as 'ФИО' , " +
                "T_Result.resilt as 'Результат' , " +
                "T_Result.`data` as 'Дата' " +
                "FROM T_Result INNER JOIN T_Test ON T_Result.idTesta = T_Test.idTestov " +
                $"WHERE T_Result.fioRes = '{fioStud}'";

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
            tablSize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            table.Clear();
            //Переменная для запроса по тесту
            int nTesta = Convert.ToInt32(comboBox1.SelectedValue);
            //Объявление запроса
            string sqlQueryLoadUsers = "SELECT " +
                "T_Result.idTesta as 'Номер теста' , " +
                "T_Test.nameTest as 'Название теста' , " +
                "T_Result.fioRes as 'ФИО' , " +
                "T_Result.resilt as 'Результат' , " +
                "T_Result.`data` as 'Дата' " +
                "FROM T_Result INNER JOIN T_Test ON T_Result.idTesta = T_Test.idTestov " +
                $"WHERE T_Result.idTesta = '{nTesta}'"; ;

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
            tablSize();
        }

        private void rezTest_Load(object sender, EventArgs e)
        {
            // строка подключения к БД
            string connStr = "server=pma.sdlik.ru;port=62002;user=st_29;database=is_29_EKZ;password=123456789;";
            //Инициализация подключения
            conn = new MySqlConnection(connStr);
            GetComboBoxList();





        }

        public void tablSize()
        {
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
        }

        public void GetComboBoxList()
        {
            //Формирование списка id тестов
            DataTable list_nomer_testa = new DataTable();
            MySqlCommand list_nTest_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_nomer_testa.Columns.Add(new DataColumn("idTestov", System.Type.GetType("System.Int32")));
            list_nomer_testa.Columns.Add(new DataColumn("nTestov", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_nomer_testa;
            comboBox1.DisplayMember = "nTestov";
            comboBox1.ValueMember = "idTestov";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_test = "SELECT T_Test.idTestov,T_Test.nameTest  FROM T_Test";
            list_nTest_command.CommandText = sql_list_test;
            list_nTest_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_test_reader;
            try
            {
                //Инициализируем ридер
                list_test_reader = list_nTest_command.ExecuteReader();
                while (list_test_reader.Read())
                {
                    DataRow rowToAdd = list_nomer_testa.NewRow();
                    rowToAdd["idTestov"] = Convert.ToInt32(list_test_reader[0]);
                    rowToAdd["nTestov"] = list_test_reader[1].ToString();
                    list_nomer_testa.Rows.Add(rowToAdd);
                }
                list_test_reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения списка тестов \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Введем переменную в который будет храниться путь к рабочему столу
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);                        
            //Созданим книгу
            XLWorkbook rez = new XLWorkbook();
            //Добавим страницу содержащуюю наши данные с таблицы
            rez.Worksheets.Add(table, "Rez"); 
            //Сделаем выравневание
            rez.Worksheet(1).Columns().AdjustToContents();
            //Сохраним файл на рабочем столе 
            rez.SaveAs(path+"result.xlsx");
            //Вывод сообщения куда сохранен файл
            MessageBox.Show("Фаил сохранен в "+path);
        }
    }
}
