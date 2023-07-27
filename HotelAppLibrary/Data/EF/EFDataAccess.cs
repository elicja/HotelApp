using HotelAppLibrary.Interfaces;
using HotelAppLibrary.Models;
using HotelAppLibrary.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Data.EF;

public class EFDataAccess : IDatabaseData, IEFDataAccess
{
    private readonly EFDataContext _dataContext;

    public EFDataAccess(IConfiguration config)
    {
        _dataContext = new EFDataContext(config);
    }

    public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
    {
        GuestModel guestModel = _dataContext.Guests.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

        if (guestModel == null)
        {
            guestModel = new GuestModel { FirstName = firstName, LastName = lastName };
            _dataContext.Guests.Add(guestModel);
            _dataContext.SaveChanges();
        }

        RoomTypeModel roomTypeModel = _dataContext.RoomTypes.FirstOrDefault(x => x.Id == roomTypeId);

        TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

        List<RoomModel> availableRooms = GetAvaiableRooms(startDate, endDate).Where(x => x.RoomTypeId == roomTypeId).ToList();

        BookingFullModel booking = new BookingFullModel
        {
            RoomId = availableRooms.First().Id,
            GuestId = guestModel.Id,
            StartDate = startDate,
            EndDate = endDate,
            TotalCost = timeStaying.Days * roomTypeModel.Price
        };

        _dataContext.Bookings.Add(booking);
        _dataContext.SaveChanges();
    }

    public void CheckInGuest(int bookingId)
    {
        BookingFullModel booking = _dataContext.Bookings.FirstOrDefault(x => x.Id == bookingId);

        if (booking == null)
        {
            return;
        }

        booking.CheckedIn = true;

        _dataContext.Bookings.Update(booking);
        _dataContext.SaveChanges();
    }

    public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
    {
        return GetAvaiableRooms(startDate, endDate).Select(x => x.RoomType).Distinct().ToList();
    }

    public List<RoomModel> GetAvaiableRooms(DateTime startDate, DateTime endDate)
    {
        List<int> takenRooms = _dataContext.Bookings
            .Where(b => startDate < b.StartDate && endDate > b.EndDate ||
                        b.StartDate <= endDate && endDate < b.EndDate ||
                        b.StartDate <= startDate && startDate < b.EndDate
            ).Select(x => x.RoomId).ToList();

        List<RoomModel> availableRooms = _dataContext.Rooms.Include(x => x.RoomType)
                                                           .Where(r => !takenRooms.Contains(r.Id)).ToList();

        return availableRooms;
    }

    public RoomTypeModel GetRoomTypeById(int id)
    {
        return _dataContext.RoomTypes.FirstOrDefault(x => x.Id == id);
    }

    public List<BookingFullModel> SearchBookings(string firstName, string lastName)
    {
        List<BookingFullModel> guestBookings = _dataContext.Bookings
            .Include(x => x.Guest)
            .Include(x => x.Room)
                .ThenInclude(x => x.RoomType)
            .Where(x => x.Guest.LastName == lastName && x.Guest.FirstName == firstName).ToList();

        foreach (var booking in guestBookings)
        {
            booking.FirstName = booking.Guest.FirstName;
            booking.LastName = booking.Guest.LastName;
            booking.RoomNumber = booking.Room.RoomNumber;
            booking.RoomTypeId = booking.Room.RoomTypeId;
            booking.Title = booking.Room.RoomType.Title;
            booking.Description = booking.Room.RoomType.Description;
            booking.Price = booking.Room.RoomType.Price;
        }

        return guestBookings;
    }
}
