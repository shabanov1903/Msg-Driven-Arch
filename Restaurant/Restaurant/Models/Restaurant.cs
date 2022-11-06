namespace Restaurant.Models
{
    internal class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly Messenger _messenger;
        private readonly object locker = new object();

        public Restaurant(ushort max, Messenger messenger)
        {
            for (ushort i = 1; i <= max; i++)
            {
                _tables.Add(new Table(i));
            }
            _messenger = messenger;
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Task.Run(async () =>
            {
                Table? table = null;

                lock (locker)
                {
                    table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == State.Free);
                    table?.SetState(State.Booked);
                }

                await _messenger.BookingAsync(table);
            });
        }

        public void MakeFreeTableAsync(int id)
        {
            Task.Run(async () =>
            {
                Table? table = null;

                lock (locker)
                {
                    table = _tables.FirstOrDefault(t => t.Id == id);
                    table?.SetState(State.Free);
                }

                await _messenger.MakeFreeAsync(table);
            });
        }

        public void StartClearJob()
        {
            Task.Run(async () =>
            {
                while(true)
                {
                    foreach (var table in _tables)
                    {
                        table.SetState(State.Free);
                    }
                    
                    _messenger.MakeFreeAll();
                    
                    await Task.Delay(60000);
                }
            });
        }
    }
}
