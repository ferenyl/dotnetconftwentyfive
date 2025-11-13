# PowerShell-script för att lägga till en användare i tabellen "Users"
# Fälten i tabellen: namn, efternamn, isadmin, isactive
# Standardvärden är satt till: johan ahlbäck 1 1
#
# OBS:
# - Spara denna fil i UTF-8 (gärna med BOM på Windows) så att åäö hanteras korrekt.
# - Uppdatera $Server och $Database, eller kör scriptet med parametrar nedan.
# - Kan använda Integrated Security (standard) eller SQL-auth genom att ange -SqlUser och -SqlPassword.

param(
    [string]$Server = "localhost",
    [string]$Database = "MytestDb",
    [string]$UserName = "johan",
    [string]$LastName = "ahlbäck",
    [int]$IsAdmin = 1,
    [int]$IsActive = 1,
    [string]$SqlUser = "sa",
    [string]$SqlPassword = "StrongP@ssword"
)

# Bygg connection string (använder Integrated Security om ingen SqlUser anges)
if ($SqlUser -ne "") {
    $connectionString = "Server=$Server;Database=$Database;User Id=$SqlUser;Password=$SqlPassword;TrustServerCertificate=True;"
} else {
    $connectionString = "Server=$Server;Database=$Database;Integrated Security=SSPI;TrustServerCertificate=True;"
}

# Parameteriserad INSERT (skyddar mot SQL-injection och hanterar specialtecken)
$query = @"
INSERT INTO [User] (namn, efternamn, isadmin, isactive)
VALUES (@namn, @efternamn, @isadmin, @isactive);
"@

try {
    Add-Type -AssemblyName "System.Data"

    $conn = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $query

    # Lägg till parametrar med passande typer
    $p1 = $cmd.Parameters.Add("@namn", [System.Data.SqlDbType]::NVarChar, 200)
    $p1.Value = $UserName

    $p2 = $cmd.Parameters.Add("@efternamn", [System.Data.SqlDbType]::NVarChar, 200)
    $p2.Value = $LastName

    $p3 = $cmd.Parameters.Add("@isadmin", [System.Data.SqlDbType]::Int)
    $p3.Value = [int]$IsAdmin

    $p4 = $cmd.Parameters.Add("@isactive", [System.Data.SqlDbType]::Int)
    $p4.Value = [int]$IsActive

    $conn.Open()
    $rows = $cmd.ExecuteNonQuery()
    $conn.Close()

    if ($rows -gt 0) {
        Write-Host "Lyckades: $rows rad(er) insatta." -ForegroundColor Green
    } else {
        Write-Host "Inga rader insatta." -ForegroundColor Yellow
    }
}
catch {
    Write-Error "Fel vid databasoperation: $_"
}
