using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    internal class Messenger
    {
        private readonly string message_no_seats = "Все столы заняты!";
        private readonly string message_table_booked = "Стол {0} забронирован для вас!";
        private readonly string message_table_not_found = "Такого стола не существует!";
        private readonly string message_table_rided = "Стол {0} освобожден!";
        private readonly string message_all_tables_free = "Все столы освобождены";

        private readonly int delay = 3000;

        public void Send(string text)
        {
            Thread.Sleep(delay);

            Console.WriteLine(text);
        }

        public async Task SendAsync(string text)
        {
            await Task.Delay(delay);
            
            Console.WriteLine(text);
        }

        public void Booking(Table? table)
        {
            if (table == null)
            {
                Send(message_no_seats);
            }
            else
            {
                Send(String.Format(message_table_booked, table.Id));
            }
        }

        public void MakeFree(Table? table)
        {
            if (table == null)
            {
                Send(message_table_not_found);
            }
            else
            {
                Send(String.Format(message_table_rided, table.Id));
            }
        }

        public async Task BookingAsync(Table? table)
        {
            if (table == null)
            {
                await SendAsync(message_no_seats);
            }
            else
            {
                await SendAsync(String.Format(message_table_booked, table.Id));
            }
        }

        public async Task MakeFreeAsync(Table? table)
        {
            if (table == null)
            {
                await SendAsync(message_table_not_found);
            }
            else
            {
                await SendAsync(String.Format(message_table_rided, table.Id));
            }
        }

        public void MakeFreeAll()
        {
            Console.WriteLine(message_all_tables_free);
        }
    }
}
