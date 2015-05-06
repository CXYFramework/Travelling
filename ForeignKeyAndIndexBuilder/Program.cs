using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
namespace ForeignKeyBuilder
{
    class Program
    {
        static string sqlTable = "SELECT name as Name FROM [dbo].[sysobjects] where OBJECTPROPERTY(id, N'IsUserTable') = 1";
        static string sqlColumnFormat = "select name as Name from sys.all_columns where object_id=OBJECT_ID('{0}')";
        static string fkAddFormat = "alter table {0} add constraint {1} foreign key ({2}) references {3}({4})";
        static string fkDropFormat = "ALTER TABLE [dbo].[{0}] DROP CONSTRAINT [{1}]";

        static string indexFormat="CREATE INDEX {0} ON {1} ({2}); ";

        static string deleteCascade = " on delete cascade";
        static string updateCascade = " on update cascade";

        static string sqlCount = "select count(1) from {0}";
        static string sqlDelete = "delete from {0}";

        static int IsAdd = 1;

        static void Main(string[] args)
        {


            using (var connection = Program.GetOpenConnection())
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsCascadeDelete"]))
                    fkAddFormat += deleteCascade;


                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsCascadeUpdate"]))
                    fkAddFormat += updateCascade;

                IsAdd = Convert.ToInt32(ConfigurationManager.AppSettings["IsAddFK"]);

                var tables = connection.Query(sqlTable);

                foreach (var item in tables)
                {
                    string tableName = item.Name;

                    var columns = connection.Query(string.Format(sqlColumnFormat, tableName));

                    foreach (var column in columns)
                    {
                        string columnName = column.Name;

                        string parentTableName = columnName.Match(@"\b\w+(?=_Id|_ID\b)|\b\w+(?=Id|ID\b)");

                        if (!string.IsNullOrWhiteSpace(parentTableName))
                        {
                            try
                            {
                                string foreigkeyName = "FK_" + tableName + "To_" + parentTableName;
                                string indexName = "IX_" + tableName + "_" + columnName;
                                if (IsAdd == 1)
                                {
                                    string fkSql = string.Format(fkAddFormat, tableName, foreigkeyName, columnName, parentTableName, "Id");
                                    connection.Execute(fkSql);
                                    string indexSql = string.Format(indexFormat, indexName, tableName, columnName);
                                    connection.Execute(indexSql);
                                }
                                else
                                {
                                    string dropFK = string.Format(fkDropFormat, tableName, foreigkeyName);
                                    //connection.Execute(dropFK);
                                }

                                // Console.WriteLine(string.Format(sqlDelete,tableName));
                                Console.WriteLine(string.Format(sqlCount, tableName));
                                // Console.WriteLine(foreigkeyName + " Success!");


                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());

                            }
                        }

                    }

                }



            }

            Console.WriteLine("Done");

            Console.Read();
        }
        //public static readonly string connectionString = "Data Source=.;Initial Catalog=test2;Integrated Security=True";
        public static readonly string connectionString = "data source=MSTV-SHGUAN-04;initial catalog=TravelDB;uid=sa;pwd=sa";
        public static SqlConnection GetOpenConnection(bool mars = false)
        {
            var cs = connectionString;
            if (mars)
            {
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(cs);
                scsb.MultipleActiveResultSets = true;
                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
        public static SqlConnection GetClosedConnection()
        {
            return new SqlConnection(connectionString);
        }

    }

    public static class StringExtensions
    {
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return null;
            }
            return Regex.Match(value, pattern).Value;
        }
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }
    }
}
