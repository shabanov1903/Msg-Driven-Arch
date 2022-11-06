using Messaging;

namespace Restaurant.Models
{
    internal class Messenger
    {
        private readonly Producer _producer;

        private readonly string message_no_seats = "Все столы заняты!";
        private readonly string message_table_booked = "Стол {0} забронирован для вас!";
        private readonly string message_table_not_found = "Такого стола не существует!";
        private readonly string message_table_rided = "Стол {0} освобожден!";
        private readonly string message_all_tables_free = "Все столы освобождены...";

        internal Messenger(Producer producer) => _producer = producer;

        private readonly int delay = 3000;

        public async Task SendAsync(string text)
        {
            await Task.Delay(delay);

            _producer.Send(text);
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
            _producer.Send(message_all_tables_free);
        }
    }
}
