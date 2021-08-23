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
    public class StudentMarksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentMarksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MarkSubjects(int studentId)
        {
            TempData["studentId"] = studentId;
            var subject = await _unitOfWork.Student.GetAsync(studentId);
            TempData["stName"] = subject.Name;
            return View();
        }
        public async Task<JsonResult> LoadMarks(string fetch)
        {
            int stSubId = fetch.Trim() != "" ? Convert.ToInt32(fetch.Trim()) : 0;
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);
            var studentMarks = await _unitOfWork.StudentMarks.GetFirstOrDefaultAsync(a => a.StudentSubjectId == stSubId);
            return Json(new { studentMarks = JsonSerializer.Serialize(studentMarks) });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            int studentSubjectId = Convert.ToInt32(TempData.Peek("studentSubjectId") ?? 0);

            StudentMarks studentMarks = new StudentMarks()
            {
                StudentSubjectId = studentSubjectId
            };
            if (id == null || id == 0)
            {
                //this is for create
                return View(studentMarks);
            }
            //this is for edit
            studentMarks = await _unitOfWork.StudentMarks.GetAsync(id.GetValueOrDefault());
            if (studentMarks == null)
            {
                return NotFound();
            }
            return View(studentMarks);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Upsert(StudentMarks studentMarks)
        {
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);

            if (ModelState.IsValid)
            {
                if (studentMarks.Id == 0)
                {
                    await _unitOfWork.StudentMarks.AddAsync(studentMarks);
                }
                else
                {
                    _unitOfWork.StudentMarks.Update(studentMarks);
                }
                _unitOfWork.Save();
                return RedirectToAction("MarkSubjects", "StudentMarks", new { area = "User", studentId = stId });
            }
            return View(studentMarks);
        }
        public async Task<IActionResult> RoadToUpsert(int studentSubjectId)
        {
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);
            TempData["studentSubjectId"] = studentSubjectId;

            var alf = from stSub in await _unitOfWork.StudentSubject.GetAllAsync()
                      join stMk in await _unitOfWork.StudentMarks.GetAllAsync() on stSub.Id equals stMk.StudentSubjectId
                      where stSub.StudentId == stId && stSub.Id == studentSubjectId
                      select new
                      {
                          stMk.Id
                      };
            int id = alf.Any() ? alf.FirstOrDefault().Id : 0;
            return RedirectToAction("Upsert", "StudentMarks", new { area = "User", id });
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var alf = await _unitOfWork.Student.GetAllAsync();
            return Json(new { data = alf });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudentSubjects()
        {
            int stId = Convert.ToInt32(TempData.Peek("studentId") ?? 0);

            var alf = from st in await _unitOfWork.Student.GetAllAsync()
                      join gr in await _unitOfWork.Grade.GetAllAsync() on st.GradeId equals gr.Id
                      join stSub in await _unitOfWork.StudentSubject.GetAllAsync() on st.Id equals stSub.StudentId
                      join sub in await _unitOfWork.Subject.GetAllAsync() on stSub.SubjectId equals sub.Id
                      where st.Id == stId
                      select new
                      {
                          subCode = sub.Code,
                          subject = sub.Name,
                          score = getScore(stSub.Id).GetAwaiter().GetResult(),
                          grade = gr.Name,
                          id = stSub.Id
                      };

            return Json(new { data = alf });
        }
        private async Task<double> getScore(int id)
        {
            var studentMarks = await _unitOfWork.StudentMarks.GetFirstOrDefaultAsync(a => a.StudentSubjectId == id);
            if (studentMarks != null)
            {
                return studentMarks.Score;
            }
            return 0;
        }

        #endregion
    }
}
