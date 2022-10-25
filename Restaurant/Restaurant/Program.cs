using System.Diagnostics;
using rest = Restaurant.Models;

var restaurant = new rest.Restaurant(10, new rest.Messenger());
restaurant.StartClearJob();

while(true)
{
    Console.WriteLine("Выберите действите:");
    Console.WriteLine(" 0 - синхронно забронировать \n 1 - асинхронно забронировать \n 2 - синхронно освободить \n 3 - асинхронно освободить ");

    if (!int.TryParse(Console.ReadLine(), out var choice) && choice is not (0 or 1 or 3 or 4))
    {
        Console.WriteLine("Повторите ввод");
        continue;
    }

    var timer = new Stopwatch();
    timer.Start();

    switch (choice)
    {
        case 0:
            restaurant.BookFreeTable(1); break;
        case 1:
            restaurant.BookFreeTableAsync(1); break;
        case 2:
            {
                Console.WriteLine("Какой стол освободить?");
                if (!int.TryParse(Console.ReadLine(), out var number))
                {
                    Console.WriteLine("Повторите ввод");
                    continue;
                }
                restaurant.MakeFreeTable(number);
            }; break;
        case 3:
            {
                Console.WriteLine("Какой стол освободить?");
                if (!int.TryParse(Console.ReadLine(), out var number))
                {
                    Console.WriteLine("Повторите ввод");
                    continue;
                }
                restaurant.MakeFreeTableAsync(number);
            }; break;
    }

    timer.Stop();
    Console.WriteLine($"Затрачено: {timer.Elapsed.Seconds:00}:{timer.Elapsed.Milliseconds:00}мс");
}
