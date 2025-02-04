using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.BLL.Interfaces;
using School.DAL.Models;

namespace School.PL.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: ClassesController
        public async Task<IActionResult> Index()
        {
            var AllClass = await _unitOfWork.ClassesRepository.GetAllAsync();
            return View(AllClass);
        }


        // GET: ClassesController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null) return BadRequest(); //400

            var Cls = await _unitOfWork.ClassesRepository.GetByIdAsync(id.Value);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Classes model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ClassesRepository.AddAsync(model);
                var Count = await _unitOfWork.SaveDataAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index)); //404
                }
            }
            return View(model);
        }


        // GET: ClassesController/Edit/5
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id is null) return BadRequest();//400
            var CLS = await _unitOfWork.ClassesRepository.GetByIdAsync(id.Value);
            if (CLS is null)
            {
                return NotFound(); //404
            }
            return View(CLS);
        }


        // POST: ClassesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Classes model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ClassesRepository.Update(model);
                var Count = await _unitOfWork.SaveDataAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
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

            var CLS = await _unitOfWork.ClassesRepository.GetByIdAsync(Id);
            if (CLS == null)
            {
                return NotFound("Class not found.");
            }

            _unitOfWork.ClassesRepository.Delete(CLS);
            var count = await _unitOfWork.SaveDataAsync();

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle case where save failed
                return StatusCode(500, "An error occurred while deleting the class.");
            }
        }


    }
}
