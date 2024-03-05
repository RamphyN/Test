using Oracle.ManagedDataAccess.Client;
using System;
using System.IO;
using System.Data;

namespace MyFirstOracleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string conString = "User Id=SYS;Password=ramphynunez;Data Source=//localhost:1521/xe;DBA Privilege=SYSDBA";

            using (OracleConnection connection = new OracleConnection(conString))
            {
                try
                {
                    connection.Open();
                    bool exit = false;
                    while (!exit)
                    {
                        Console.WriteLine("\nMenu:");
                        Console.WriteLine("1. Create");
                        Console.WriteLine("2. Read");
                        Console.WriteLine("3. Update");
                        Console.WriteLine("4. Delete");
                        Console.WriteLine("5. Export to CSV");
                        Console.WriteLine("6. Export Schema and Data");
                        Console.WriteLine("7. Exit");
                        Console.Write("Enter your choice: ");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                CreateStudent(connection);
                                break;
                            case "2":
                                ReadStudents(connection);
                                break;
                            case "3":
                                UpdateStudent(connection);
                                break;
                            case "4":
                                DeleteStudent(connection);
                                break;
                            case "5":
                                ExportToCSV(connection);
                                break;
                            case "6":
                                ExportSchemaAndData(connection);
                                break;
                            case "7":
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    LogError(ex.Message);
                }
            }
        }

        static void CreateStudent(OracleConnection connection)
        {
            Console.WriteLine("\nCreating a new student...");
            Console.Write("Enter ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Mobile: ");
            string mobile = Console.ReadLine();
            Console.Write("Enter Course: ");
            string course = Console.ReadLine();

            OracleCommand insertCommand = new OracleCommand("INSERT INTO Student (ID, FirstName, LastName, EmailId, Mobile, Course) VALUES (:id, :firstName, :lastName, :email, :mobile, :course)", connection);
            insertCommand.Parameters.Add(":id", OracleDbType.Int32).Value = id;
            insertCommand.Parameters.Add(":firstName", OracleDbType.Varchar2).Value = firstName;
            insertCommand.Parameters.Add(":lastName", OracleDbType.Varchar2).Value = lastName;
            insertCommand.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
            insertCommand.Parameters.Add(":mobile", OracleDbType.Varchar2).Value = mobile;
            insertCommand.Parameters.Add(":course", OracleDbType.Varchar2).Value = course;
            int rowsInserted = insertCommand.ExecuteNonQuery();
            Console.WriteLine($"{rowsInserted} row(s) inserted.");
        }

        static void ReadStudents(OracleConnection connection)
        {
            Console.WriteLine("\nReading all students...");
            OracleCommand selectCommand = new OracleCommand("SELECT ID, FirstName, LastName, EmailId, Mobile, Course FROM Student", connection);
            OracleDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader.GetInt32(0)}, First Name: {reader.GetString(1)}, Last Name: {reader.GetString(2)}, Email: {reader.GetString(3)}, Mobile: {reader.GetString(4)}, Course: {reader.GetString(5)}");
            }
            reader.Close();
        }

        static void UpdateStudent(OracleConnection connection)
        {
            Console.WriteLine("\nUpdating a student...");
            Console.Write("Enter Student ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            // Get the current student details
            OracleCommand selectCommand = new OracleCommand("SELECT FirstName, LastName, EmailId, Mobile, Course FROM Student WHERE ID = :id", connection);
            selectCommand.Parameters.Add(":id", OracleDbType.Int32).Value = studentId;
            OracleDataReader reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                string currentFirstName = reader.GetString(0);
                string currentLastName = reader.GetString(1);
                string currentEmail = reader.GetString(2);
                string currentMobile = reader.GetString(3);
                string currentCourse = reader.GetString(4);

                Console.Write($"Current First Name ({currentFirstName}): ");
                string newFirstName = Console.ReadLine();
                newFirstName = string.IsNullOrWhiteSpace(newFirstName) ? currentFirstName : newFirstName;

                Console.Write($"Current Last Name ({currentLastName}): ");
                string newLastName = Console.ReadLine();
                newLastName = string.IsNullOrWhiteSpace(newLastName) ? currentLastName : newLastName;

                Console.Write($"Current Email ({currentEmail}): ");
                string newEmail = Console.ReadLine();
                newEmail = string.IsNullOrWhiteSpace(newEmail) ? currentEmail : newEmail;

                Console.Write($"Current Mobile ({currentMobile}): ");
                string newMobile = Console.ReadLine();
                newMobile = string.IsNullOrWhiteSpace(newMobile) ? currentMobile : newMobile;

                Console.Write($"Current Course ({currentCourse}): ");
                string newCourse = Console.ReadLine();
                newCourse = string.IsNullOrWhiteSpace(newCourse) ? currentCourse : newCourse;

                reader.Close();

                OracleCommand updateCommand = new OracleCommand("UPDATE Student SET FirstName = :firstName, LastName = :lastName, EmailId = :email, Mobile = :mobile, Course = :course WHERE ID = :id", connection);
                updateCommand.Parameters.Add(":firstName", OracleDbType.Varchar2).Value = newFirstName;
                updateCommand.Parameters.Add(":lastName", OracleDbType.Varchar2).Value = newLastName;
                updateCommand.Parameters.Add(":email", OracleDbType.Varchar2).Value = newEmail;
                updateCommand.Parameters.Add(":mobile", OracleDbType.Varchar2).Value = newMobile;
                updateCommand.Parameters.Add(":course", OracleDbType.Varchar2).Value = newCourse;
                updateCommand.Parameters.Add(":id", OracleDbType.Int32).Value = studentId;
                int rowsUpdated = updateCommand.ExecuteNonQuery();
                Console.WriteLine($"{rowsUpdated} row(s) updated.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        static void DeleteStudent(OracleConnection connection)
        {
            Console.WriteLine("\nDeleting a student...");
            Console.Write("Enter Student ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int deleteId))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            OracleCommand deleteCommand = new OracleCommand("DELETE FROM Student WHERE ID = :id", connection);
            deleteCommand.Parameters.Add(":id", OracleDbType.Int32).Value = deleteId;
            int rowsDeleted = deleteCommand.ExecuteNonQuery();
            Console.WriteLine($"{rowsDeleted} row(s) deleted.");
        }

        static void ExportToCSV(OracleConnection connection)
        {
            string fileName = "students.csv";
            string delimiter = ",";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("ID,First Name,Last Name,Email,Mobile,Course");

                OracleCommand selectCommand = new OracleCommand("SELECT ID, FirstName, LastName, EmailId, Mobile, Course FROM Student", connection);
                OracleDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    string line = $"{reader.GetInt32(0)}{delimiter}{reader.GetString(1)}{delimiter}{reader.GetString(2)}{delimiter}{reader.GetString(3)}{delimiter}{reader.GetString(4)}{delimiter}{reader.GetString(5)}";
                    writer.WriteLine(line);
                }
                reader.Close();

                Console.WriteLine($"Data exported to {fileName}");
            }
        }

        static void ExportSchemaAndData(OracleConnection connection)
        {
            Console.Write("Enter the table name to export its schema and data: ");
            string tableName = Console.ReadLine().ToUpper();

            string fileName = $"{tableName}_schema_and_data.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                string schemaQuery = $@"SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH 
                                         FROM ALL_TAB_COLUMNS 
                                         WHERE TABLE_NAME = '{tableName}' 
                                         ORDER BY COLUMN_ID";
                using (OracleCommand schemaCommand = new OracleCommand(schemaQuery, connection))
                {
                    using (OracleDataReader reader = schemaCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"No schema information found for the table: {tableName}");
                            return;
                        }

                        writer.WriteLine("Schema:");
                        writer.WriteLine("Column Name, Data Type, Data Length");
                        while (reader.Read())
                        {
                            string columnName = reader.GetString(0);
                            string dataType = reader.GetString(1);
                            int dataLength = reader.GetInt32(2);

                            writer.WriteLine($"{columnName}, {dataType}, {dataLength}");
                        }
                        writer.WriteLine();
                    }
                }

                string dataQuery = $"SELECT * FROM {tableName}";
                using (OracleCommand dataCommand = new OracleCommand(dataQuery, connection))
                {
                    using (OracleDataReader reader = dataCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            writer.WriteLine("No data found in the table.");
                            return;
                        }

                        writer.WriteLine("Data:");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            writer.Write(reader.GetName(i) + (i < reader.FieldCount - 1 ? ", " : "\n"));
                        }

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                writer.Write(reader.GetValue(i).ToString() + (i < reader.FieldCount - 1 ? ", " : "\n"));
                            }
                        }
                    }
                }
                Console.WriteLine($"Table schema and data exported to {fileName}");
            }
        }

        static void LogError(string errorMessage)
        {
            string logFileName = "error.log";
            string logFilePath = Path.Combine(Environment.CurrentDirectory, logFileName);
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {errorMessage}");
                }
                Console.WriteLine($"Error logged to {logFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while logging: {ex.Message}");
            }
        }
    }
}
