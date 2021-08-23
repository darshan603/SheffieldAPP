using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SheffieldAPP.DataAccess.Repository.IRepository;
using SheffieldAPP.Models;
using SheffieldAPP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SheffieldAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
    public class StudentSubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentSubjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SetSubjects(int studentId)
        {
            TempData["studentId"] = studentId;
            var subject = await _unitOfWork.Student.GetAsync(studentId);
            TempData["stName"] = subject.Name;
            return View();
        }
        public async Task<JsonResult> AddSubject(string fetch)
        {
            int subId = fetch.Trim() != "" ?  Convert.ToInt32(fetch.Trim()) : 0;
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);
            if (subId > 0 && stId > 0)
            {
                var objFromDb = await _unitOfWork.StudentSubject.GetAllAsync(a => a.SubjectId == subId && a.StudentId == stId);
                if (!objFromDb.Any())
                {
                    StudentSubject studentSubject = new StudentSubject()
                    {
                        StudentId = stId,
                        SubjectId = subId
                    };

                    await _unitOfWork.StudentSubject.AddAsync(studentSubject);
                    _unitOfWork.Save();
                    return Json(new { success = true, message = "Subject Added Successfully" });
                }
                return Json(new { success = false, message = "Subject Already Added" });
            }
            return Json(new { success = false, message = "Faild To Add Subject" });
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var alf = await _unitOfWork.Student.GetAllAsync();
            return Json(new { data = alf });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjects()
        {
            var alf = await _unitOfWork.Subject.GetAllAsync();
            return Json(new { data = alf });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentSubjects()
        {
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);
            List<int> ids = new List<int>();
            ids = (await _unitOfWork.StudentSubject.GetAllAsync(a => a.StudentId == stId)).Select(a => a.SubjectId).ToList();
            var alf = await _unitOfWork.Subject.GetAllAsync(a => ids.Contains(a.Id));
            return Json(new { data = alf });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);

            var objFromDb = await _unitOfWork.StudentSubject.GetFirstOrDefaultAsync(a => a.SubjectId == id && a.StudentId == stId);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.StudentSubject.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
