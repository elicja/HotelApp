using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAppLibrary.Models;

public class BookingFullModel
{
    public int Id { get; set; }
    [ForeignKey(nameof(Room))]
    public int RoomId { get; set; }

    [ForeignKey(nameof(GuestModel))]
    public int GuestId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool CheckedIn { get; set; }
    public decimal TotalCost { get; set; }

    [NotMapped]
    public string FirstName { get; set; }
    [NotMapped]
    public string LastName { get; set; }

    [NotMapped]
    public string RoomNumber { get; set; }

    [NotMapped]
    public int RoomTypeId { get; set; }
    [NotMapped]
    public string Title { get; set; }
    [NotMapped]
    public string Description { get; set; }
    [NotMapped]
    public decimal Price { get; set; }

    public GuestModel Guest { get; set; }
    public RoomModel Room { get; set; }
}
