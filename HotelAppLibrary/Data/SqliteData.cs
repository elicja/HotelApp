using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data;

public class SqliteData : IDatabaseData
{
    private const string connectionStringName = "SQLiteDb";
    private readonly ISqliteDataAccess _db;

    public SqliteData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
    {
        throw new NotImplementedException();
    }

    public void CheckInGuest(int bookingId)
    {
        throw new NotImplementedException();
    }

    public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
    {
        string sql = @"select t.Id, t.Title, t.Description, t.Price
	                from Rooms r
	                inner join RoomTypes t on t.Id = r.RoomTypeId
	                where r.Id not in (
	                select b.RoomId
	                from Bookings b
	                where (@startDate < b.StartDate and @endDate > b.EndDate)
		                or (b.StartDate <= @endDate and @endDate < b.EndDate)
		                or (b.StartDate <= @startDate and @startDate < b.EndDate)
	                )
                    group by t.Id, t.Title, t.Description, t.Price";

        var output = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                             new { startDate, endDate },
                                             connectionStringName);

        output.ForEach(x => x.Price = x.Price / 100);

        return output;
    }

    public RoomTypeModel GetRoomTypeById(int id)
    {
        throw new NotImplementedException();
    }

    public List<BookingFullModel> SearchBookings(string lastName)
    {
        throw new NotImplementedException();
    }
}
