using SheffieldAPP.DataAccess.Data;
using SheffieldAPP.DataAccess.Repository.IRepository;
using SheffieldAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheffieldAPP.DataAccess.Repository
{
    public class StudentMarksRepository : Repository<StudentMarks>, IStudentMarksRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentMarksRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(StudentMarks studentMarks)
        {
            var objFromDb = _db.StudentMarks.FirstOrDefault(s => s.Id == studentMarks.Id);
            if (objFromDb != null)
            {
                objFromDb.Score = studentMarks.Score;
            }
        }
    }
}
