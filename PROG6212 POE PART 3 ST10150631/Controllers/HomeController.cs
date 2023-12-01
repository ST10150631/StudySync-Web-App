using Microsoft.AspNetCore.Mvc;
using PROG6212_POE_PART_3_ST10150631.Models;
using PROG6212_POE_PART_3_ST10150631.ViewModels;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace PROG6212_POE_PART_3_ST10150631.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private NoteModel _note;
        private NoteViewModel _noteViewModel = new NoteViewModel();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            NoteModel note = new NoteModel();
            try
            {
                note =  _noteViewModel.CheckNotes();
                return View(note);
            }
            catch
            {

            }
            return View();
        }
        

        [HttpPost,ActionName("AddNoteClicked")]        
        public async Task <IActionResult> AddNoteClicked(string noteName, string NoteContent,int hour,int minute,int day, int month, int year)
        {
            try
            {
                _note = new NoteModel();
                _note.NoteName = noteName;
                _note.NoteContent = NoteContent;
                var date = new DateTime(year, month, day,hour,minute,0);
                _note.NoteDate = date;
                _note.Username = MainViewModel.SignedInUser;
                _noteViewModel.AddNote(_note);
                

            }
            catch 
            {

            }
            return RedirectToAction("Index");
        }


        [HttpPost,ActionName("BtnDeleteNote")]
        public IActionResult BtnDeleteNote(string noteName)
        {
                _noteViewModel.RemoveNote(noteName);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}