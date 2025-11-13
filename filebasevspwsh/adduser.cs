#:package Microsoft.Data.SqlClient@7.0.0-preview2.25289.6

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

// File-based top-level app that reads new user from command-line arguments
// and inserts into Users (namn, efternamn, isadmin, isactive) using ADO.NET.
// No external packages required.

// Usage example:
// dotnet run AddUser.cs -- --UserName=johan --LastName=ahlbäck --IsAdmin=1 --IsActive=1 --Server="dbserver\\INSTANCE" --Database=MinDB
// With SQL-auth:
// dotnet run AddUser.cs -- --UserName=johan --LastName=ahlbäck --IsAdmin=1 --IsActive=1 --Server=dbserver --Database=MinDB --SqlUser=sa --SqlPassword=hemligt

// Default DB settings (change with args)
string server = "localhost";
string database = "MytestDb";
string sqlUser = "sa";
string sqlPassword = "StrongP@ssword";

// Required user args
string? userName = null;
string? lastName = null;
int isAdmin = 1;
int isActive = 1;

// Simple arg parsing for --Key=Value
var parsed = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
foreach (var a in args)
{
    if (string.IsNullOrWhiteSpace(a))
        continue;
    var idx = a.IndexOf('=');
    if (a.StartsWith("--") && idx > 2)
    {
        var key = a.Substring(2, idx - 2);
        var val = a.Substring(idx + 1);
        parsed[key] = val;
    }
}

// Map recognized keys
if (parsed.TryGetValue("Server", out var v))
    server = v;
if (parsed.TryGetValue("Database", out v))
    database = v;
if (parsed.TryGetValue("SqlUser", out v))
    sqlUser = v;
if (parsed.TryGetValue("SqlPassword", out v))
    sqlPassword = v;

if (parsed.TryGetValue("UserName", out v))
    userName = v;
if (parsed.TryGetValue("LastName", out v))
    lastName = v;
if (parsed.TryGetValue("IsAdmin", out v) && int.TryParse(v, out var ia))
    isAdmin = ia;
if (parsed.TryGetValue("IsActive", out v) && int.TryParse(v, out var it))
    isActive = it;

// Validate required inputs
if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(lastName))
{
    Console.Error.WriteLine("Fel: UserName och LastName krävs.");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine(
        "  dotnet run AddUser.cs -- --UserName=<namn> --LastName=<efternamn> [--IsAdmin=0|1] [--IsActive=0|1] [--Server=<server>] [--Database=<db>] [--SqlUser=<user>] [--SqlPassword=<pw>]"
    );
    Environment.Exit(2);
}

// Build connection string
string connectionString =
    $"Server={server};Database={database};User Id={sqlUser};Password={sqlPassword};TrustServerCertificate=True;";

const string sql =
    @"
INSERT INTO [User] (namn, efternamn, isadmin, isactive)
VALUES (@namn, @efternamn, @isadmin, @isactive);
";

try
{
    using var conn = new SqlConnection(connectionString);
    using var cmd = new SqlCommand(sql, conn);

    // Use NVARCHAR to preserve åäö etc.
    cmd.Parameters.Add(
        new SqlParameter("@namn", SqlDbType.NVarChar, 200)
        {
            Value = (object)userName ?? DBNull.Value,
        }
    );
    cmd.Parameters.Add(
        new SqlParameter("@efternamn", SqlDbType.NVarChar, 200)
        {
            Value = (object)lastName ?? DBNull.Value,
        }
    );
    cmd.Parameters.Add(new SqlParameter("@isadmin", SqlDbType.Int) { Value = isAdmin });
    cmd.Parameters.Add(new SqlParameter("@isactive", SqlDbType.Int) { Value = isActive });

    conn.Open();
    int rows = cmd.ExecuteNonQuery();

    if (rows > 0)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Lyckades: {rows} rad(er) insatta för {userName} {lastName}.");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Inga rader insatta.");
        Console.ResetColor();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine("Fel vid databasoperation: " + ex.Message);
    Console.ResetColor();
    Environment.Exit(1);
}
