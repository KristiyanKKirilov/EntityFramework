using AdoDotNetExercise;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Channels;

namespace AdoDotNetExercise
{
    public class StartUp
    {
        const string connectionString = @"Server=DESKTOP-M5SEPFK\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";
        static SqlConnection? connection;

        static async Task Main(string[] args)
        {
            try
            {
                connection = new(connectionString);
                connection.Open();

                //GetVillainWithTotalMinionsMoreThanThree();
                //Console.WriteLine("------");
                //int id = int.Parse(Console.ReadLine());
                //await GetVillainById(id);

                string minionInfoRow = Console.ReadLine();
                string villainInfoRow = Console.ReadLine();

                string minionInfo = minionInfoRow.Substring(minionInfoRow.IndexOf(':') + 1).Trim();
                string villainName = villainInfoRow.Substring(villainInfoRow.IndexOf(':') + 1).Trim();
                AddMinion(minionInfo, villainName);
                //await Console.Out.WriteLineAsync(minionInfo);
            }
            finally
            {

                connection?.Dispose();
            }


        }

        static void GetVillainWithTotalMinionsMoreThanThree()
        {
            using SqlCommand getVillainsCommand = new(SQLQueries.GetVilliansWithMinionsCount, connection);

            using SqlDataReader sqlDataReader = getVillainsCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                Console.WriteLine($"Villan name: {sqlDataReader["Name"]} -> Minions captured: {sqlDataReader["TotalMinions"]}");
            }
        }

        static async Task GetVillainById(int id)
        {
            using SqlCommand getVillianByIdCommand = new(SQLQueries.GetVillianById, connection);
            getVillianByIdCommand.Parameters.AddWithValue("@Id", id);

            var result = await getVillianByIdCommand.ExecuteScalarAsync();
            if (result is null)
            {
                await Console.Out.WriteLineAsync($"No villain with ID {id} exists in the database");
            }
            else
            {
                await Console.Out.WriteLineAsync($"Villain: {result}");

                using SqlCommand getMinionDataCommand = new(SQLQueries.GetMinionsPerVillianById, connection);
                getMinionDataCommand.Parameters.AddWithValue("@Id", id);
                using SqlDataReader minionReader = await getMinionDataCommand.ExecuteReaderAsync();

                while (await minionReader.ReadAsync())
                {
                    await Console.Out.WriteLineAsync($"Minion number:{minionReader["RowNum"]}\nName:{minionReader["Name"]}\nAge:{minionReader["Age"]}\n");
                }
            }

        }

        static async Task AddMinion(string minionInfo, string villainName)
        {
            SqlTransaction transaction = connection.BeginTransaction();

            string[] minionData = minionInfo.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string minionName = minionData[0];
            int minionAge = int.Parse(minionData[1]);
            string minionTown = minionData[2];

            await Console.Out.WriteLineAsync(minionData[0]);

            try
            {
                #region Town
                using SqlCommand getTownCommand = new(SQLQueries.GetTownByName, connection, transaction);
                getTownCommand.Parameters.AddWithValue("@townName", minionTown);

                var townResult = await getTownCommand.ExecuteScalarAsync();
                int townId = -1;

                if (townResult is null)
                {
                    using SqlCommand insertTownCommand = new(SQLQueries.InsertTown, connection, transaction);
                    insertTownCommand.Parameters.AddWithValue("@townName", minionTown);
                    townId = Convert.ToInt32(await insertTownCommand.ExecuteScalarAsync());
                    await Console.Out.WriteLineAsync($"Town {minionTown} was added to the database");
                }
                else
                {
                    townId = (int)townResult;
                }

                #endregion

                #region Villain
                using SqlCommand getVillainCommand = new(SQLQueries.GetVillainByName, connection, transaction);
                getVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                var villainResult = await getVillainCommand.ExecuteScalarAsync();
                int villainId = -1;

                if (villainResult is null)
                {
                    using SqlCommand insertVillainCommand = new(SQLQueries.InsertVillain, connection, transaction);
                    insertVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                    insertVillainCommand.Parameters.AddWithValue("@evilnessFactorId", 4);
                    villainId = Convert.ToInt32(await insertVillainCommand.ExecuteScalarAsync());
                    await Console.Out.WriteLineAsync($"Villain {villainName} was added to the database");
                }
                else
                {
                    villainId = (int)villainResult;
                }
                #endregion

                #region Minion                           

                using SqlCommand insertMinionCommand = new(SQLQueries.InsertMinion, connection, transaction);
                insertMinionCommand.Parameters.AddWithValue("@minionName", minionName);
                insertMinionCommand.Parameters.AddWithValue("@minionAge", minionAge);
                insertMinionCommand.Parameters.AddWithValue("@townId", minionTown);
                await Console.Out.WriteLineAsync($"Minion {minionName} was added to the database");

                int minionId = Convert.ToInt32(await insertMinionCommand.ExecuteScalarAsync());

                using SqlCommand insertMinionIntoVillain = new(SQLQueries.InsertIntoMinionsVillains, connection, transaction);
                insertMinionIntoVillain.Parameters.AddWithValue("@minionId", minionId);
                insertMinionIntoVillain.Parameters.AddWithValue("@villainId", villainId);
                await insertMinionIntoVillain.ExecuteNonQueryAsync();
                await Console.Out.WriteLineAsync($"Successfully added  {minionName} as servant to {villainName}");
                #endregion

                transaction.Commit();
            }
            catch 
            {

               transaction.Rollback();
            }
        }
    }
}
