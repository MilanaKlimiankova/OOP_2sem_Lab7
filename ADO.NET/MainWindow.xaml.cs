using Glimpse.Ado.Tab;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADO.NET
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public class Image
        {
            public Image(int id, string filename, string title, byte[] data)
            {
                Id = id;
                FileName = filename;
                Title = title;
                Data = data;
            }
            public int Id { get; private set; }
            public string FileName { get; private set; }
            public string Title { get; private set; }
            public byte[] Data { get; private set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlExpression = " SELECT COUNT(*) FROM Users";
            string sqlExpression2 = "INSERT INTO Users (Name, Age) VALUES ('Tamara', 22)";
            string sqlExpression3 = "UPDATE Users SET Age=200 WHERE Name='Polina'";
            string sqlExpression4 = "DELETE FROM Users WHERE Name='Tamara'";
            string sqlExpression5 = "SELECT * FROM Users";
            string sqlExpression6 = "SELECT MIN(Age) FROM Users";
            string sqlExpression7 = "INSERT INTO Users (Name, Age) VALUES (@name1, @age1)";
            string sqlExpression8 = "INSERT INTO Users (Name, Age) VALUES (@name, @age);SET @id=SCOPE_IDENTITY()";
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                object n = command.ExecuteScalar();
                n = (int)n;
                Emount.Text = n.ToString();
                //////// выполняет sql-выражение и возвращает одно скалярное значение, например, число. Подходит для sql-выражения SELECT в паре с одной из встроенных функций SQL, как например, Min, Max, Sum, Count.
                SqlCommand command2 = new SqlCommand(sqlExpression2, connection);
                command2.ExecuteNonQuery();
                /////просто выполняет sql-выражение и возвращает количество измененных записей. Подходит для sql-выражений INSERT, UPDATE, DELETE.
                SqlCommand command3 = new SqlCommand(sqlExpression3, connection);
                command3.ExecuteNonQuery();
                //////
                SqlCommand command4 = new SqlCommand(sqlExpression4, connection);
                command4.ExecuteNonQuery();
                ////////
                SqlCommand command5 = new SqlCommand(sqlExpression5, connection);
                SqlDataReader reader = command5.ExecuteReader();
                /////ExecuteReader: выполняет sql-выражение и возвращает строки из таблицы. Подходит для sql-выражения SELECT.
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object id11 = reader.GetValue(0);
                        object name11 = reader.GetValue(1);
                        object age11 = reader.GetValue(2);
                        Select.Text += id11.ToString() + name11.ToString() + age11.ToString();
                    }
                }
                reader.Close();
                /////
                SqlCommand command6 = new SqlCommand(sqlExpression6, connection);
                MinAge.Text = command6.ExecuteScalar().ToString();
                ////
                SqlCommand command7 = new SqlCommand(sqlExpression7, connection);
                string name1 = "Margo";
                int age1 = 25;
                SqlParameter nameParam = new SqlParameter("@name1", name1);
                command7.Parameters.Add(nameParam);
                SqlParameter ageParam = new SqlParameter("@age1", age1);
                command7.Parameters.Add(ageParam);
                command7.ExecuteNonQuery();
                //
                //
                //
                string name2 = "Leon";
                int age2 = 34;
                SqlCommand command8 = new SqlCommand(sqlExpression8, connection);
                SqlParameter nameParam2 = new SqlParameter("@name", name2);
                command8.Parameters.Add(nameParam2);
                SqlParameter ageParam2 = new SqlParameter("@age", age2);
                command8.Parameters.Add(ageParam2);
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output // параметр выходной
                };
                command8.Parameters.Add(idParam);
                command8.ExecuteNonQuery();
                idParam1.Text=idParam.Value.ToString();

                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command9 = connection.CreateCommand();
                command.Transaction = transaction;

                try
                {
                    // выполняем две отдельные команды
                    command.CommandText = "INSERT INTO Users (Name, Age) VALUES('Tim', 34)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Users (Name, Age) VALUES('Kat', 31)";
                    command.ExecuteNonQuery();
                    // подтверждаем транзакцию
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }

                SqlCommand command10 = new SqlCommand();
                command10.Connection = connection;
                command10.CommandText = @"INSERT INTO Images VALUES (@FileName, @Title, @ImageData)";
                command10.Parameters.Add("@FileName", SqlDbType.NVarChar, 50);
                command10.Parameters.Add("@Title", SqlDbType.NVarChar, 50);
                command10.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                // путь к файлу для загрузки
                string filename = @"D:\car.jpg";
                // заголовок файла
                string title = "Кот";
                // получаем короткое имя файла для сохранения в бд
                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1); // cats.jpg
                                                                                           // массив для хранения бинарных данных файла
                byte[] imageData;
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                // передаем данные в команду через параметры
                command10.Parameters["@FileName"].Value = shortFileName;
                command10.Parameters["@Title"].Value = title;
                command10.Parameters["@ImageData"].Value = imageData;
                command10.ExecuteNonQuery();
                ////
                ////
                

            }
            ReadFileFromDatabase();
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            ////
            string sql = "SELECT * FROM Users";
            string connectionString2 = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
            using (SqlConnection connection2 = new SqlConnection(connectionString2))
            {
                connection2.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection2);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                dataGridView1.ItemsSource = ds.Tables[0].DefaultView;
            }

        }
        private static void ReadFileFromDatabase()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
            List<Image> images = new List<Image>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Images";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string filename = reader.GetString(1);
                    string title = reader.GetString(2);
                    byte[] data = (byte[])reader.GetValue(3);

                    Image image = new Image(id, filename, title, data);
                    images.Add(image);
                }
            }
            // сохраним первый файл из списка
            if (images.Count > 0)
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(images[0].FileName, FileMode.OpenOrCreate))
                {
                    fs.Write(images[0].Data, 0, images[0].Data.Length);
                    Console.WriteLine("Изображение '{0}' сохранено", images[0].Title);
                }
            }
        }
        private static void GetAgeRange(string name)
        {
            string sqlExpression = "sp_GetAgeRange";
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                // определяем первый выходной параметр
                SqlParameter minAgeParam = new SqlParameter
                {
                    ParameterName = "@minAge",
                    SqlDbType = SqlDbType.Int // тип параметра
                };
                // указываем, что параметр будет выходным
                minAgeParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(minAgeParam);

                // определяем второй выходной параметр
                SqlParameter maxAgeParam = new SqlParameter
                {
                    ParameterName = "@maxAge",
                    SqlDbType = SqlDbType.Int
                };
                maxAgeParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(maxAgeParam);

                command.ExecuteNonQuery();

                MessageBox.Show("Минимальный возраст:" + command.Parameters["@minAge"].Value);
                MessageBox.Show("Максимальный возраст" + command.Parameters["@maxAge"].Value);
            }
        }
        private static void AddUser(string name, int age)
        {
            // название процедуры
            string sqlExpression = "sp_InsertUser";
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                // добавляем параметр
                command.Parameters.Add(nameParam);
                // параметр для ввода возраста
                SqlParameter ageParam = new SqlParameter
                {
                    ParameterName = "@age",
                    Value = age
                };
                command.Parameters.Add(ageParam);

                var result = command.ExecuteScalar();
                // если нам не надо возвращать id
                //var result = command.ExecuteNonQuery();

                Console.WriteLine("Id добавленного объекта: {0}", result);
            }
        }
        private static void GetUsers()
        {
            // название процедуры
            string sqlExpression = "sp_GetUsers";
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int age = reader.GetInt32(2);
                        Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                    }
                }
                reader.Close();
            }


        }
    }

}
