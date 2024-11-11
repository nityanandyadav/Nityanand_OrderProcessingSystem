# Nityanand_OrderProcessingSystem
Migration Steps
 1. First Go to your local database and create a db named OrderProcessingSystemDb
 2. Inside appsettings.Development.json change conection string as per your database.
 3. Run the migration command 
    Add-Migration "InitialMigration" 
    then run this update database command
    Update-Database
