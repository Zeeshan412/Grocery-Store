using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace WebApplication4.DataLayer.Initializers
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(string connectionString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;
                builder.InitialCatalog = "master";
                string masterConnectionString = builder.ToString();

                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE [{databaseName}]";
                    command.ExecuteNonQuery();
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create Category table
                    string createCategoryTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE [dbo].[Category] (
                                [ID] INT IDENTITY(1,1) PRIMARY KEY,
                                [name] NVARCHAR(255) NOT NULL
                            )
                        END";

                    // Create Product table
                    string createProductTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE [dbo].[Product] (
                                [ID] INT IDENTITY(1,1) PRIMARY KEY,
                                [name] NVARCHAR(255) NOT NULL,
                                [Price] INT NOT NULL,
                                [stock] INT NOT NULL DEFAULT 0,
                                [ImageUrl] NVARCHAR(500) NULL,
                                [CategoryId] INT NULL,
                                CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category]([ID])
                            )
                        END";

                    // Create Order table
                    string createOrderTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE [dbo].[Order] (
                                [ID] INT IDENTITY(1,1) PRIMARY KEY,
                                [UID] NVARCHAR(450) NOT NULL,
                                [OrderDate] DATETIME NOT NULL,
                                [Bill] DECIMAL(18,2) NOT NULL,
                                [Name] NVARCHAR(255) NOT NULL,
                                [Address] NVARCHAR(500) NOT NULL,
                                [City] NVARCHAR(100) NOT NULL,
                                [State] NVARCHAR(100) NOT NULL,
                                [ZipCode] NVARCHAR(20) NOT NULL
                            )
                        END";

                    // Create OrderProducts table
                    string createOrderProductsTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderProducts]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE [dbo].[OrderProducts] (
                                [OrderID] INT NOT NULL,
                                [ProductID] INT NOT NULL,
                                [Quantity] INT NOT NULL,
                                [Price] DECIMAL(18,2) NOT NULL,
                                CONSTRAINT [PK_OrderProducts] PRIMARY KEY ([OrderID], [ProductID]),
                                CONSTRAINT [FK_OrderProducts_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order]([ID]) ON DELETE CASCADE,
                                CONSTRAINT [FK_OrderProducts_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product]([ID])
                            )
                        END";

                    using (var command = new SqlCommand(createCategoryTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqlCommand(createProductTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqlCommand(createOrderTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqlCommand(createOrderProductsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create RefreshTokens table for JWT token storage
                    string createRefreshTokensTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE [dbo].[RefreshTokens] (
                                [Id] INT IDENTITY(1,1) PRIMARY KEY,
                                [UserId] NVARCHAR(450) NOT NULL,
                                [Token] NVARCHAR(MAX) NOT NULL,
                                [JwtId] NVARCHAR(450) NOT NULL,
                                [CreatedAt] DATETIME2 NOT NULL,
                                [ExpiresAt] DATETIME2 NOT NULL,
                                [IsRevoked] BIT NOT NULL DEFAULT 0,
                                [IsUsed] BIT NOT NULL DEFAULT 0
                            )
                            CREATE INDEX IX_RefreshTokens_Token ON [dbo].[RefreshTokens]([Token])
                            CREATE INDEX IX_RefreshTokens_JwtId ON [dbo].[RefreshTokens]([JwtId])
                            CREATE INDEX IX_RefreshTokens_UserId ON [dbo].[RefreshTokens]([UserId])
                        END";

                    using (var command = new SqlCommand(createRefreshTokensTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - allows app to continue if tables already exist
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }
        }
    }
}

