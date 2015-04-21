using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LinqToExcel;
using DAL;
using Model;
namespace CtripCodeListInitilizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "CodeList.xls";
            var excelFile = new ExcelQueryFactory(path);
            var artistAlbums = (from a in excelFile.WorksheetRangeNoHeader("B8", "C1257", "Codelists") select a).ToList();
            string tableName = string.Empty;
            
            foreach (var a in artistAlbums)
            {
                if (!string.IsNullOrWhiteSpace(a[0]))
                {
                    int s;

                    if (Int32.TryParse(a[0], out s))
                    {
                        try
                        {
                            using (var context = new TravelDBContext())
                            {
                               
                            Rego:

                                string checkTableSql = string.Format("select 1 from  sysobjects  where  id = object_id('[dbo].[{0}]')  and   type = 'U'", tableName);

                                object checkTableExsits = context.Database.SqlQuery<object>(checkTableSql).FirstOrDefault();

                                if (checkTableExsits != null)
                                {

                                    string checkSql = string.Format("select 1 from {0} where Id=@p0 and Name=@p1", tableName, a[0], a[1]);

                                    object exsits = context.Database.SqlQuery<object>(checkSql, a[0].Value, a[1].Value).FirstOrDefault();


                                    //delete
                                    //{
                                    //    string sql = "delete from {0}";

                                    //    context.Database.ExecuteSqlCommand(string.Format(sql, tableName, a[0], a[1]), a[0].Value, a[1].Value);
                                    //    Console.WriteLine(tableName + "," + a[0] + "," + a[1]);
                                    //}

                                    //insert

                                    if (exsits == null)
                                    {

                                       // string sql = "delete from {0}";
                                         string sql = "insert into {0} values (@p0,@p1,'" + DateTime.Now + "')";
                                        context.Database.ExecuteSqlCommand(string.Format(sql, tableName, a[0], a[1]), a[0].Value, a[1].Value);
                                        Console.WriteLine(tableName + "," + a[0] + "," + a[1]);
                                    }
                                }
                                else
                                {
                                    string createTableSql = string.Format("create table {0} (Id int primary key ,Name nvarchar(500) , LastModiyTime datetime) ", tableName);
                                   
                                    string sqlPath ="c:\\sql\\" + tableName + ".sql";

                                    if (!System.IO.File.Exists(sqlPath))
                                        System.IO.File.AppendAllText(sqlPath, createTableSql);
                                    //string createTableSql = string.Format("drop table {0}", tableName);
                                    context.Database.ExecuteSqlCommand(createTableSql);

                                    Console.WriteLine(tableName + "created");

                                    goto Rego;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                       
                    }
                    else
                    {
                        tableName = a[0];
                    }
                }

            }

            Console.WriteLine("Done!");
            Console.Read();
        }
    }
}
