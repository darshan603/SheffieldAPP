using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SheffieldAPP.DataAccess.Repository.IRepository;
using SheffieldAPP.Models;
using SheffieldAPP.Models.ViewModels;
using SheffieldAPP.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SheffieldAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            StudentVM studentVM = new StudentVM()
            {
                Student = new Student(),
                GradeList = _unitOfWork.Grade.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(studentVM);
            }
            //this is for edit
            studentVM.Student = await _unitOfWork.Student.GetAsync(id.GetValueOrDefault());
            if (studentVM.Student == null)
            {
                return NotFound();
            }
            return View(studentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(StudentVM studentVM)
        {
            if (ModelState.IsValid)
            {
                if (studentVM.Student.Id == 0)
                {
                    await _unitOfWork.Student.AddAsync(studentVM.Student);
                }
                else
                {
                    _unitOfWork.Student.Update(studentVM.Student);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                studentVM.GradeList = _unitOfWork.Grade.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return View(studentVM.Student);
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alf = await _unitOfWork.Student.GetAllAsync();
            return Json(new { data = alf });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Student.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Student.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
