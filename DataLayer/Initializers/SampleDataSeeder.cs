using Microsoft.Data.SqlClient;
using System;

namespace WebApplication4.DataLayer.Initializers
{
    public static class SampleDataSeeder
    {
        public static void SeedSampleData(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if data already exists
                    using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Category", connection))
                    {
                        int categoryCount = (int)checkCmd.ExecuteScalar();
                        if (categoryCount > 0)
                        {
                            Console.WriteLine("Sample data already exists. Skipping seed.");
                            return;
                        }
                    }

                    // Insert sample categories
                    string insertCategories = @"
                        INSERT INTO Category (name) VALUES ('Fruits');
                        INSERT INTO Category (name) VALUES ('Vegetables');
                        INSERT INTO Category (name) VALUES ('Dairy');
                        INSERT INTO Category (name) VALUES ('Beverages');
                    ";

                    using (var command = new SqlCommand(insertCategories, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Get category IDs (assuming they're 1, 2, 3, 4)
                    // Insert sample products
                    string insertProducts = @"
                        INSERT INTO Product (name, Price, stock, ImageUrl, CategoryId) VALUES 
                        ('Apple', 150, 50, '/UploadedImages/care.jpeg', 1),
                        ('Banana', 80, 30, '/UploadedImages/donuts.jpeg', 1),
                        ('Orange', 120, 40, '/UploadedImages/Blue Band.jpg', 1),
                        ('Tomato', 100, 60, '/UploadedImages/care.jpeg', 2),
                        ('Potato', 60, 80, '/UploadedImages/donuts.jpeg', 2),
                        ('Onion', 70, 50, '/UploadedImages/JAR.png', 2),
                        ('Milk', 200, 25, '/UploadedImages/Milkpack.jpg', 3),
                        ('Cheese', 350, 15, '/UploadedImages/Milkpack2.jpg', 3),
                        ('Yogurt', 180, 20, '/UploadedImages/Blue Band.jpg', 3),
                        ('Water', 50, 100, '/UploadedImages/SEVENUP.jpg', 4),
                        ('Juice', 120, 35, '/UploadedImages/SEVENUP.jpg', 4),
                        ('Soft Drink', 100, 45, '/UploadedImages/SEVENUP.jpg', 4);
                    ";

                    using (var command = new SqlCommand(insertProducts, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Sample data seeded successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding sample data: {ex.Message}");
            }
        }
    }
}

