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
        public async Task<JsonResult> Index()
        {
            var allClass = await _unitOfWork.ClassesRepository.GetAllAsync();
            return new JsonResult(new {Data=allClass});
        }


        // GET: ClassesController/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id is null) return BadRequest(); //400

            var Cls = await _unitOfWork.ClassesRepository.GetByIdAsync(id.Value);

            if (Cls is null)
            {
                return NotFound(); //404
            }

            return new JsonResult(Cls);
        }


        // GET: ClassesController/Create
        public JsonResult Create()
        {
            return Json(new { success = true, message = "Create action accessed successfully." });
        }
    
        // POST: ClassesController/Create
        [HttpPost]
        public async Task<JsonResult> Create(Classes model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ClassesRepository.AddAsync(model);
                var Count = await _unitOfWork.SaveDataAsync();
                if (Count > 0)
                {
                    return Json(new { success = true, message = "Class created successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Error saving the class" });
                }
            }

            // If the model is invalid, return validation errors.
            return Json(new { success = false, message = "Invalid model", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList() });
        }


        // GET: ClassesController/Edit/5
        public async Task<JsonResult> Update(Guid? id)
        {
            if (id is null)
            {
                return Json(new { success = false, message = "Invalid ID", statusCode = 400 });
            }

            var Cls = await _unitOfWork.ClassesRepository.GetByIdAsync(id.Value);

            if (Cls is null)
            {
                return Json(new { success = false, message = "Class not found", statusCode = 404 });
            }

            // If the class is found, return the data.
            return Json(new { success = true, data = Cls });
        }


        // POST: ClassesController/Edit/5
        [HttpPost]
        public async Task<JsonResult> Update(Classes model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ClassesRepository.Update(model);
                var count = await _unitOfWork.SaveDataAsync();

                if (count > 0)
                {
                    return Json(new { success = true, message = "Data updated successfully." });
                }
            }
            return Json(new { success = false, message = "Error occurred while updating the data." });
        }


        // GET: ClassesController/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            var Cls =await _unitOfWork.ClassesRepository.GetByIdAsync(id);
            if (Cls is null) return BadRequest();
            _unitOfWork.ClassesRepository.Delete(Cls);
            return RedirectToAction(nameof(Index));
        }


        // POST: HomeController1/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(Classes model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.ClassesRepository.Delete(model);
                    var Count =await _unitOfWork.SaveDataAsync();
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
    }
}
