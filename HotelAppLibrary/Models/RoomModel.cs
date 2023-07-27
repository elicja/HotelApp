using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAppLibrary.Models;

public class RoomModel
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }

    [ForeignKey(nameof(RoomType))]
    public int RoomTypeId { get; set; }
    public RoomTypeModel RoomType { get; set; }
}
