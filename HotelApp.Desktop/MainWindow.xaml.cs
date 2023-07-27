using HotelAppLibrary.Models;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using HotelAppLibrary.Interfaces;

namespace HotelApp.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IDatabaseData _db;

    public MainWindow(IDatabaseData db)
    {
        InitializeComponent();
        _db = db;
    }

    private void searchForGuest_Click(object sender, RoutedEventArgs e)
    {
        List<BookingFullModel> bookings = _db.SearchBookings(firstNameText.Text,lastNameText.Text);
        resultsList.ItemsSource = bookings;
    }

    private void CheckInButton_Click(object sender, RoutedEventArgs e)
    {
        var checkInForm = App.serviceProvider.GetService<CheckInForm>();
        var model = (BookingFullModel)((Button)e.Source).DataContext;

        checkInForm.PopulateCheckInInfo(model);

        checkInForm.Show();
    }
}
