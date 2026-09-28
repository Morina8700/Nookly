namespace BookMyHome.Domain.Models;

public class Guest : User
{
    public ICollection<Booking> Bookings { get; private set; }
        = new List<Booking>();

    private Guest()
    {
    }

    public Guest(string name, string email)
        : base(name, email)
    {
    }
}