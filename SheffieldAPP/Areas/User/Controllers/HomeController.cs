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
using System.Text.Json;
using System.Threading.Tasks;

namespace SheffieldAPP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetSummury(string fetch)
        {
            var alf = from st in await _unitOfWork.Student.GetAllAsync()
                      join gr in await _unitOfWork.Grade.GetAllAsync() on st.GradeId equals gr.Id
                      join stSub in await _unitOfWork.StudentSubject.GetAllAsync() on st.Id equals stSub.StudentId
                      join sub in await _unitOfWork.Subject.GetAllAsync() on stSub.SubjectId equals sub.Id
                      join stMk in await _unitOfWork.StudentMarks.GetAllAsync() on stSub.Id equals stMk.StudentSubjectId
                      where st.Code.StartsWith(fetch)
                      select new
                      {
                          code = st.Code,
                          name = st.Name,
                          subject = sub.Name,
                          marks = stMk.Score,
                          grade = gr.Name
                      };

            return Json(new { jsonString = JsonSerializer.Serialize(alf), list = alf });
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll(string searchString)
        {
            var alf = from st in await _unitOfWork.Student.GetAllAsync()
                      join gr in await _unitOfWork.Grade.GetAllAsync() on st.GradeId equals gr.Id
                      join stSub in await _unitOfWork.StudentSubject.GetAllAsync() on st.Id equals stSub.StudentId
                      join sub in await _unitOfWork.Subject.GetAllAsync() on stSub.SubjectId equals sub.Id
                      join stMk in await _unitOfWork.StudentMarks.GetAllAsync() on stSub.Id equals stMk.StudentSubjectId
                      select new
                      {
                          code = st.Code,
                          name = st.Name,
                          subject = sub.Name,
                          marks = stMk.Score,
                          grade = gr.Name
                      };

            if (!string.IsNullOrEmpty(searchString))
            {
                alf = alf.Where(a => a.code.Trim().StartsWith(searchString));
            }
            else
            {
                alf = alf.Where(a => a.code.Trim() == "");
            }

            return Json(new { data = alf });
        }
        #endregion

    }
}
