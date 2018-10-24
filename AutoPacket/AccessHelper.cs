using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPacket
{
    class AccessHelper
    {
        private static string connStr = @"Provider = Microsoft.Ace.OLEDB.12.0;Data Source = E:\\SVN\\trunk\\ConsoleApplication4\\bin\\Debug\\test.mdb";
        public static OleDbConnection GetConn()

        {

            OleDbConnection tempconn = new OleDbConnection(connStr);

            //MessageBox.Show(tempconn.DataSource);

            tempconn.Open();

           // MessageBox.Show(tempconn.State.ToString());

            return (tempconn);

        }



        /// <summary>  

        /// 执行增加、删除、修改指令  

        /// </summary>  

        /// <param name="sql">增加、删除、修改的sql语句</param>  

        /// <param name="param">sql语句的参数</param>  

        /// <returns></returns>  

        public static int ExecuteNonQuery(string sql, params OleDbParameter[] param)

        {

            using (OleDbConnection conn = new OleDbConnection(connStr))

            {

                using (OleDbCommand cmd = new OleDbCommand(sql, conn))

                {

                    if (param != null)

                    {

                        cmd.Parameters.AddRange(param);

                    }

                    conn.Open();

                    return (cmd.ExecuteNonQuery());

                }

            }

        }



        /// <summary>  

        /// 执行查询指令，获取返回的首行首列的值  

        /// </summary>  

        /// <param name="sql">查询sql语句</param>  

        /// <param name="param">sql语句的参数</param>  

        /// <returns></returns>  

        public static object ExecuteScalar(string sql, params OleDbParameter[] param)

        {

            using (OleDbConnection conn = new OleDbConnection(connStr))

            {

                using (OleDbCommand cmd = new OleDbCommand(sql, conn))

                {

                    if (param != null)

                    {

                        cmd.Parameters.AddRange(param);

                    }

                    conn.Open();

                    return (cmd.ExecuteScalar());

                }

            }

        }



        /// <summary>  

        /// 执行查询指令，获取返回的datareader  

        /// </summary>  

        /// <param name="sql">查询sql语句</param>  

        /// <param name="param">sql语句的参数</param>  

        /// <returns></returns>  

        public static OleDbDataReader ExecuteReader(string sql, params OleDbParameter[] param)

        {

            OleDbConnection conn = new OleDbConnection(connStr);

            OleDbCommand cmd = conn.CreateCommand();

            cmd.CommandText = sql;

            cmd.CommandType = CommandType.Text;

            if (param != null)

            {

                cmd.Parameters.AddRange(param);

            }

            conn.Open();

            return (cmd.ExecuteReader(CommandBehavior.CloseConnection));



        }



        /// <summary>  

        /// 执行查询指令，获取返回datatable  

        /// </summary>  

        /// <param name="sql">查询sql语句</param>  

        /// <param name="param">sql语句的参数</param>  

        /// <returns></returns>  

        public static DataTable ExecuteDatable(string sql, params OleDbParameter[] param)

        {

            using (OleDbConnection conn = new OleDbConnection(connStr))

            {

                using (OleDbCommand cmd = new OleDbCommand(sql, conn))

                {

                    if (param != null)

                    {

                        cmd.Parameters.AddRange(param);

                    }

                    DataTable dt = new DataTable();

                    OleDbDataAdapter sda = new OleDbDataAdapter(cmd);

                    sda.Fill(dt);

                    return (dt);

                }

            }



        }

    }

}
