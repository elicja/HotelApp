using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class BookRoomModel : PageModel
    {
        private readonly IDatabaseData _db;

        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }
                
        [BindProperty(SupportsGet = true)]
        public string StartDateStr { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string EndDateStr { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        public RoomTypeModel RoomType { get; set; }

        public BookRoomModel(IDatabaseData db)
        {
            _db = db;
        }

        public void OnGet()
        {
            HandleDate();

            if (RoomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(RoomTypeId);
            }
        }

        public IActionResult OnPost()
        {
            HandleDate();

            _db.BookGuest(FirstName, LastName, StartDate, EndDate, RoomTypeId);
            return RedirectToPage("/Index");
        }

        private void HandleDate()
        {
            if (!string.IsNullOrEmpty(StartDateStr))
            {
                StartDate = DateTime.Parse(StartDateStr);
            }

            if (!string.IsNullOrEmpty(EndDateStr))
            {
                EndDate = DateTime.Parse(EndDateStr);
            }
        }
    }    
}
