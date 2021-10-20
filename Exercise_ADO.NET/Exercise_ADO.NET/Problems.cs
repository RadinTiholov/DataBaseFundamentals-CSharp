using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Exercise_ADO.NET
{
    public abstract class Problems
    {
        internal static void FirstProblem(SqlConnection connection)
        {
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(@"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                                        FROM Villains AS v 
                                                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                                        GROUP BY v.Id, v.Name 
                                                        HAVING COUNT(mv.VillainId) > 3 
                                                        ORDER BY COUNT(mv.VillainId)"
                , connection);
                SqlDataReader result = command.ExecuteReader();
                using (result)
                {
                    while (result.Read())
                    {
                        string name = result.GetString(0);
                        int count = result.GetInt32(1);
                        Console.WriteLine($"{name} - {count}");
                    }
                }
            }
        }
        internal static void SecondProblem(SqlConnection connection, int id)
        {
            connection.Open();
            using (connection)
            {
                SqlCommand comm = new SqlCommand("SELECT Name FROM Villains WHERE Id = @Id", connection);
                SqlCommand comm2 = new SqlCommand(@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name,
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = 2
                                ORDER BY m.Name", connection);
                SqlParameter parameter = new SqlParameter("@Id", SqlDbType.Int);
                SqlParameter parameter2 = new SqlParameter("@Id", SqlDbType.Int);
                parameter.Value = id;
                parameter2.Value = id;

                comm.Parameters.Add(parameter);
                comm2.Parameters.Add(parameter2);
                var result = comm.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine($"No villain with ID {id}; exists in the database.");
                    return;
                }
                string villanName = (string)result;
                SqlDataReader reader = comm2.ExecuteReader();

                StringBuilder minionsText = new StringBuilder();
                using (reader)
                {
                    int count = 1;
                    while (reader.Read())
                    {
                        minionsText.Append($"{count}. {reader.GetString(1)} {reader.GetInt32(2)} years old.\n");
                        count++;
                    }
                }
                Console.WriteLine(villanName);
                Console.WriteLine(minionsText.ToString());
            }
        }

        internal static void ThirdProblem(SqlConnection connection)
        {
            string[] firstInput = Console.ReadLine().Split(" ");
            string[] secondInput = Console.ReadLine().Split(" ");

            string minionName = firstInput[1];
            int minionAge = int.Parse(firstInput[2]);
            string townName = firstInput[3];
            string villianName = secondInput[1];

            connection.Open();
            using(connection) 
            {
                SqlCommand commandFopSelectingTownNames = new SqlCommand(@"SELECT [Id] FROM Towns WHERE Name = @Name", connection);
                SqlParameter parameter = new SqlParameter("@Name", SqlDbType.VarChar);
                parameter.Value = townName;
                commandFopSelectingTownNames.Parameters.Add(parameter);
                var result = commandFopSelectingTownNames.ExecuteScalar();

                if (result == null) 
                {
                    SqlCommand insertingTownName = new SqlCommand("INSERT INTO Towns (Name) VALUES (@townName)", connection);
                    SqlParameter parameterForInserting = new SqlParameter("@townName", SqlDbType.VarChar);
                    parameterForInserting.Value = townName;
                    insertingTownName.Parameters.Add(parameterForInserting);
                    insertingTownName.ExecuteNonQuery();
                    Console.WriteLine($"Town {townName} was added to the database.");
                }

                var commandForSelectingVillianNames = new SqlCommand(@"SELECT Id FROM Villains WHERE Name = @Name", connection);
                SqlParameter parameterForSelectingVillianNames = new SqlParameter("@Name", SqlDbType.VarChar);
                parameterForSelectingVillianNames.Value = villianName;
                commandForSelectingVillianNames.Parameters.Add(parameterForSelectingVillianNames);
                var resultForVillian = commandForSelectingVillianNames.ExecuteScalar();

                if (resultForVillian == null)
                {
                    SqlCommand insertVillian = new SqlCommand("INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)", connection);
                    SqlParameter parameterForInsertingVillian = new SqlParameter("@villainName", SqlDbType.VarChar);
                    parameterForInsertingVillian.Value = villianName;
                    insertVillian.Parameters.Add(parameterForInsertingVillian);
                    insertVillian.ExecuteNonQuery();
                    Console.WriteLine($"Villian {villianName} was added to the database.");
                }
                int villianId = (int)resultForVillian;
                int townId = (int)result;
                SqlCommand insertMinions = new SqlCommand("INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)", connection);
                SqlParameter parForName = new SqlParameter("@name", SqlDbType.VarChar);
                SqlParameter parForAge = new SqlParameter("@age", SqlDbType.Int);
                SqlParameter parForTownId = new SqlParameter("@townId", SqlDbType.Int);

                parForName.Value = minionName;
                parForAge.Value = minionAge;
                parForTownId.Value = townId;

                insertMinions.Parameters.Add(parForName);
                insertMinions.Parameters.Add(parForAge);
                insertMinions.Parameters.Add(parForTownId);
                insertMinions.ExecuteNonQuery();

                //
                var commandForSelectingMinionNames = new SqlCommand(@"SELECT Id FROM Minions WHERE Name = @Name", connection);
                SqlParameter parameterForSelectingMinionName = new SqlParameter("@Name", SqlDbType.VarChar);
                parameterForSelectingMinionName.Value = minionName;
                commandForSelectingMinionNames.Parameters.Add(parameterForSelectingMinionName);
                int minionId = (int)commandForSelectingMinionNames.ExecuteScalar();


                SqlCommand insertIntoMinionsVillains = new SqlCommand("INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(@minionId, @villainId)", connection);
                SqlParameter parameterForInsertingVillianId = new SqlParameter("@villainId", SqlDbType.Int);
                SqlParameter parameterForInsertingMinion = new SqlParameter("@minionId", SqlDbType.Int);
                parameterForInsertingVillianId.Value = villianId;
                parameterForInsertingMinion.Value = minionId;

                insertIntoMinionsVillains.Parameters.Add(parameterForInsertingVillianId);
                insertIntoMinionsVillains.Parameters.Add(parameterForInsertingMinion);
                int rowsAffected = insertIntoMinionsVillains.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Succesfully added {minionAge} to be minion of {villianName}");
                    return;
                }
                Console.WriteLine("Smt went wrong");
            }
        }

    }
}
