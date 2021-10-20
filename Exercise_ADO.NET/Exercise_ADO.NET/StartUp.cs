using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text;

namespace Exercise_ADO.NET
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(Configuration.CONNECTION_STRING);
            //Problems.FirstProblem(connection);
            //int id = 3;
            //Problems.SecondProblem(connection, id);
            Problems.ThirdProblem(connection);
        }
       
    }
}
