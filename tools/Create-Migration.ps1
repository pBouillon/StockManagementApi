param ($MigrationName)

if ($MigrationName -eq $null) {
    $MigrationName = read-host -Prompt "Migration name" 
}

Write-Output "Creating the migration..."
dotnet ef migrations add $MigrationName --output-dir Persistence/Migrations/
Write-Output "Mirgration created."

Write-Output "Applying the migration..."
dotnet ef database update
Write-Output "Migration applied."
