namespace Restaurant.Booking;

public class Table
{
    private static readonly Random _random = new();

    private readonly object _lock = new();

    public Table(int id)
    {
        Id = id; // В учебном примере просто присвоим id при вызове
        State = State.Free; // Новый стол всегда свободен
        SeatsCount = _random.Next(2, 5); // Пусть количество мест за каждым столом будет случайным, от 2х до 5ти
    }

    public State State { get; private set; }
    public int SeatsCount { get; }
    public int Id { get; }

    public bool SetState(State state)
    {
        lock (_lock)
        {
            if (state == State)
                return false;

            State = state;
            return true;
        }
    }
}