using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private static readonly List<StudentViewModel> _students = new List<StudentViewModel>
        {
            new StudentViewModel { Id = 1, FullName = "Alice Johnson", Email = "alice@gmail.com"}
        };
        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }
        // GET: StudentController
        public ActionResult Index()
        {
            ModelState.Clear();
            return View(_students);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    model.Id = _students.Max(s => s.Id) + 1;
                    _students.Add(model);
                    _logger.LogInformation("Student added: {FullName}, {Email}", model.FullName, model.Email);
                }
                else {
                    _logger.LogWarning("Invalid model state for student creation.");
                }
                ModelState.Clear();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
