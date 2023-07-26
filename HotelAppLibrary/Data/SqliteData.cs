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
        string sql = @"select 1 from Guests where FirstName = @firstName and LastName = @lastName";
        int results = _db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName }, connectionStringName).Count();

        if (results == 0)
        {
            sql = @"insert into Guests (FirstName, LastName)
		                    values (@firstName, @lastName);";

            _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
        }

        sql = @"select [Id], [FirstName], [LastName]
	                    from Guests
	                    where FirstName = @firstName and LastName = @lastName LIMIT 1;";

        GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql,
                                                             new { firstName, lastName },
                                                             connectionStringName).First();

        RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from RoomTypes where Id = @Id",
                                                                      new { Id = roomTypeId },
                                                                      connectionStringName).First();

        TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

        sql = @"select r.*
	            from Rooms r
	            inner join RoomTypes t on t.Id = r.RoomTypeId
	            where r.RoomTypeId = @roomTypeId
	            and r.Id not in (
	            select b.RoomId
	            from Bookings b
	            where (@startDate < b.StartDate and @endDate > b.EndDate)
		            or (b.StartDate <= @endDate and @endDate < b.EndDate)
		            or (b.StartDate <= @startDate and @startDate < b.EndDate)
	            );";

        List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql,
                                                                          new { startDate, endDate, roomTypeId },
                                                                          connectionStringName);

        sql = @"insert into Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
	            values (@roomId, @guestId, @startDate, @endDate, @totalCost);";

        _db.SaveData(sql,
                     new
                     {
                         roomId = availableRooms.First().Id,
                         guestId = guest.Id,
                         startDate = startDate,
                         endDate = endDate,
                         totalCost = timeStaying.Days * roomType.Price
                     },
                     connectionStringName);
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
        string sql = @"select [Id], [Title], [Description], [Price]
	                    from RoomTypes
	                    where Id = @id;";

        return _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                    new { id },
                                                    connectionStringName).FirstOrDefault();
    }

    public List<BookingFullModel> SearchBookings(string lastName)
    {
        throw new NotImplementedException();
    }
}
