using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

class Program
{
    const string connectionString = "Server=localhost;Database=MailingListDB;Integrated Security=true;TrustServerCertificate=true";

    static void Main()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Успешное подключение к базе данных!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                return;
            }
        }

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Показать всех покупателей");
            Console.WriteLine("2. Показать все email покупателей");
            Console.WriteLine("3. Показать все города");
            Console.WriteLine("4. Показать все страны");
            Console.WriteLine("5. Показать покупателей из конкретного города");
            Console.WriteLine("6. Показать покупателей из конкретной страны");
            Console.WriteLine("7. Выйти");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowCustomers(); break;
                case "2": ShowEmails(); break;
                case "3": ShowCities(); break;
                case "4": ShowCountries(); break;
                case "5": ShowCustomersByCity(); break;
                case "6": ShowCustomersByCountry(); break;
                case "7": return;
                default: Console.WriteLine("Неверный ввод, попробуйте снова."); break;
            }
        }
    }

    static void ShowCustomers()
    {
        using var connection = new SqlConnection(connectionString);
        var customers = connection.Query("SELECT FullName, Email FROM Customers");
        foreach (var customer in customers)
            Console.WriteLine($"{customer.FullName} - {customer.Email}");
    }

    static void ShowEmails()
    {
        using var connection = new SqlConnection(connectionString);
        var emails = connection.Query<string>("SELECT Email FROM Customers");
        foreach (var email in emails)
            Console.WriteLine(email);
    }

    static void ShowCities()
    {
        using var connection = new SqlConnection(connectionString);
        var cities = connection.Query<string>("SELECT DISTINCT City FROM Customers");
        foreach (var city in cities)
            Console.WriteLine(city);
    }

    static void ShowCountries()
    {
        using var connection = new SqlConnection(connectionString);
        var countries = connection.Query<string>("SELECT DISTINCT Country FROM Customers");
        foreach (var country in countries)
            Console.WriteLine(country);
    }

    static void ShowCustomersByCity()
    {
        Console.Write("Введите город: ");
        string city = Console.ReadLine();
        using var connection = new SqlConnection(connectionString);
        var customers = connection.Query("SELECT FullName, Email FROM Customers WHERE City = @City", new { City = city });
        foreach (var customer in customers)
            Console.WriteLine($"{customer.FullName} - {customer.Email}");
    }

    static void ShowCustomersByCountry()
    {
        Console.Write("Введите страну: ");
        string country = Console.ReadLine();
        using var connection = new SqlConnection(connectionString);
        var customers = connection.Query("SELECT FullName, Email FROM Customers WHERE Country = @Country", new { Country = country });
        foreach (var customer in customers)
            Console.WriteLine($"{customer.FullName} - {customer.Email}");
    }
}
