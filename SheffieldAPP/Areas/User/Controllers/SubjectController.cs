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
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Subject subject = new Subject();
            if (id == null)
            {
                //this is for create
                return View(subject);
            }
            //this is for edit
            subject = await _unitOfWork.Subject.GetAsync(id.GetValueOrDefault());
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (subject.Id == 0)
                {
                    await _unitOfWork.Subject.AddAsync(subject);
                }
                else
                {
                    _unitOfWork.Subject.Update(subject);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alf = await _unitOfWork.Subject.GetAllAsync();
            return Json(new { data = alf });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Subject.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Subject.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
