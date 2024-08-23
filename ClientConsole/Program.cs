using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new ClientServiceProvider.Client("https://localhost:7253/"
                , "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IktleWhhbiIsIm5iZiI6MTcyNDQxNTM2NCwiZXhwIjoxNzI0NDE4OTY0LCJpYXQiOjE3MjQ0MTUzNjQsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxMTkiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUxNzIifQ.QuDItDnEgdRGUjDSPgWOpYjeJQ5Mj64PsGUvQmImwdI"); 
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("calling SimpleGet api");
                Console.ForegroundColor = ConsoleColor.Gray;
                var result = service.GetAsync<List<WeatherForecast>>("/WeatherForecast/SimpleGet").Result;
                foreach (var item in result)
                {
                    Console.WriteLine($"Summary : {item.Summary}");
                    Console.WriteLine($"TemperatureF : {item.TemperatureF}");
                    Console.WriteLine($"Date : {item.Date}");
                    Console.WriteLine("\n");
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("calling Get api");
                Console.ForegroundColor = ConsoleColor.Gray;
                var result = service.GetAsync<WeatherForecast, List<WeatherForecast>>("/WeatherForecast/Get", new WeatherForecast()
                {
                    Date = DateTime.Now,
                    Summary = "test",
                    TemperatureC = 32,
                }).Result;
                foreach (var item in result)
                {
                    Console.WriteLine($"Summary : {item.Summary}");
                    Console.WriteLine($"TemperatureF : {item.TemperatureF}");
                    Console.WriteLine($"Date : {item.Date}");
                    Console.WriteLine("\n");
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("calling Post api");
                Console.ForegroundColor = ConsoleColor.Gray;
                var result = service.PostAsync<WeatherForecast, List<WeatherForecast>>("/WeatherForecast/Post", new WeatherForecast()).Result;
                foreach (var item in result)
                {
                    Console.WriteLine($"Summary : {item.Summary}");
                    Console.WriteLine($"TemperatureF : {item.TemperatureF}");
                    Console.WriteLine($"Date : {item.Date}");
                    Console.WriteLine("\n");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("calling Post api From body");
                Console.ForegroundColor = ConsoleColor.Gray;
                var result = service.PostAsync<WeatherForecast, List<WeatherForecast>>("/WeatherForecast/PostFromBody", new WeatherForecast()).Result;
                foreach (var item in result)
                {
                    Console.WriteLine($"Summary : {item.Summary}");
                    Console.WriteLine($"TemperatureF : {item.TemperatureF}");
                    Console.WriteLine($"Date : {item.Date}");
                    Console.WriteLine("\n");
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("press any key to exit");
            Console.ReadKey();
        }
    }
}
