using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class GradeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GradeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Grade grade = new Grade();
            if (id == null)
            {
                //this is for create
                return View(grade);
            }
            //this is for edit
            grade = await _unitOfWork.Grade.GetAsync(id.GetValueOrDefault());
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Grade grade)
        {
            if (ModelState.IsValid)
            {
                if (grade.Id == 0)
                {
                    await _unitOfWork.Grade.AddAsync(grade);
                }
                else
                {
                    _unitOfWork.Grade.Update(grade);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(grade);
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alf = await _unitOfWork.Grade.GetAllAsync();
            return Json(new { data = alf });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Grade.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Grade.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
