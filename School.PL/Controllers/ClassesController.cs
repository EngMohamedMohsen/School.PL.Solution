using Hangfire;
using Microsoft.AspNetCore.Mvc;
using School.PL.Helper.CustomAttributes;
using School.PL.Helper.Services;
using School.PL.Models;

namespace School.PL.Controllers
{
    [CustomAuthorize("Admin")]
    public class ClassesController : Controller
    {
        private readonly IClassesServices _classesServices;

        public ClassesController(IClassesServices classesServices)
        {
            _classesServices = classesServices;
        }

        // GET: ClassesController
        public async Task<IActionResult> Index()
        {
            //var AllClass = await _unitOfWork.ClassesRepository.GetAllAsync();
            var AllClass = await _classesServices.GetAllClassAsync();
            return View(AllClass);
        }

        // GET: ClassesController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null) return BadRequest(); //400

            var Cls = await _classesServices.GetIdClassRoomAsync(id.Value);

            if (Cls is null)
            {
                return NotFound(); //404
            }

            return View(Cls);
        }

        // GET: ClassesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassesController/Create
        [HttpPost]
        public async Task<IActionResult> Create(ClassRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _classesServices.CreateClassRoomAsync(model);
                await _classesServices.SaveData();
               return RedirectToAction(nameof(Index)); //404
            }
            return View(model);
        }

        // GET: ClassesController/Edit/5
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id is null) return BadRequest();//400
            var CLS = await _classesServices.GetIdClassRoomAsync(id.Value);
            if (CLS is null)
            {
                return NotFound(); //404
            }
            return View(CLS);
        }

        // POST: ClassesController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Update(ClassRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _classesServices.UpdateClassRoom(model);
                await _classesServices.SaveData();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ClassesController/Delete/5
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if (Id == null)
            {
                return BadRequest("Invalid ID provided.");
            }

            // Get the class object using the provided ID.
            var CLS = await _classesServices.GetIdClassRoomAsync(Id);

            // Check if the class was found.
            if (CLS == null)
            {
                return NotFound($"Classroom with ID {Id} not found.");
            }

            // Delete the class.
            await _classesServices.DeleteClassRoom(CLS);

            // Save changes (optional: handle exceptions here for better user feedback).
            await _classesServices.SaveData();

            // Redirect to the Index page after deletion.
            return RedirectToAction(nameof(Index));
        }

    }
}
