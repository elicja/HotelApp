﻿using HotelAppLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Data.EF;

public class EFDataContext : DbContext
{
    IConfiguration _configuration;

    public EFDataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<RoomTypeModel> RoomTypes { get; set; }
    public DbSet<RoomModel> Rooms { get; set; }
    public DbSet<GuestModel> Guests { get; set; }
    public DbSet<BookingFullModel> Bookings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("EF"));
    }
}
