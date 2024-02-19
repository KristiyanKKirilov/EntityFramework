using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;

string connection = @"Server=DESKTOP-M5SEPFK\SQLEXPRESS;Database=SoftUni;Integrated Security = True;Encrypt = False;"; 

using SqlConnection sqlConnection = new(connection);

await sqlConnection.OpenAsync();

string username = Console.ReadLine();

//using SHA256 sha256hasher = SHA256.Create();

string password = Console.ReadLine();

string query = $"SELECT Id FROM Users WHERE Username = @usernameParam AND Password = @passwordParam";

using SqlCommand sqlCommand = new(query, sqlConnection);
sqlCommand.Parameters.AddWithValue("@usernameParam", username);
sqlCommand.Parameters.AddWithValue("@passwordParam", password); 
int? id = (int?)(await sqlCommand.ExecuteScalarAsync());

if(id != null && id > 0)
{
    Console.WriteLine($"You are user with Id: {id}");
}
else
{
    Console.WriteLine("Invalid username or password!");
}


