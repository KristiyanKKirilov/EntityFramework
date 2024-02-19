using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDotNetExercise
{
    //ctrl D -> copy row
    //shift alt -> mark and edit 
    public static class SQLQueries
    {
        public const string GetVilliansWithMinionsCount = "SELECT [Name], COUNT(*) AS [TotalMinions]\r" +
                                                     "\nFROM Villains AS v" +
                                                     "\r\nJOIN MinionsVillains AS mv ON v.Id = mv.VillainId" +
                                                     " \r\nGROUP BY [Name]" +
                                                     "\r\nHAVING COUNT(*) > 3" +
                                                     "\r\nORDER BY TotalMinions DESC";

        public const string GetVillianById = "SELECT [Name] FROM Villains WHERE Id = @Id";
        public const string GetMinionsPerVillianById = "SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,\r\n" +
                                                        "m.Name, \r\n" +
                                                        "m.Age\r\n" +
                                                        " FROM MinionsVillains AS mv\r\n" +
                                                        "JOIN Minions As m ON mv.MinionId = m.Id\r\n" +
                                                        "WHERE mv.VillainId = @Id\r\n" +
                                                        " ORDER BY m.Name";

        public const string GetTownByName = "SELECT [Id] FROM [Towns] WHERE [Name] = @townName";
        public const string GetVillainByName = "SELECT [Id] FROM [Villains] WHERE [Name] = @villainName";
        public const string GetMinionByName = "SELECT [Id] FROM [Minions] WHERE [Name] = @minionName";

        public const string InsertTown = "INSERT INTO Towns([Name])" +
                                        "OUTPUT inserted.Id" +
                                        "VALUES(@townName)";

        public const string InsertVillain = "INSERT INTO [Villains]([Name], [EvilnessFactorId])" +
                                        "OUTPUT inserted.Id" +
                                        "VALUES(@villainName, @evilnessFactorId)";

        public const string InsertMinion = "INSERT INTO [Minions]([Name], [Age], [TownId])" +
                                       "OUTPUT inserted.Id" +
                                       "VALUES(@minionName, @minionAge, @townId)";

        public const string InsertIntoMinionsVillains = "INSERT INTO MinionsVillains([MinionId], [VillainId]) " +
                                                         "VALUES(@minionId, @villainId)";
    }
}
