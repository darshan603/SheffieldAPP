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
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        private readonly ApplicationDbContext _db;
        public GradeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Grade grade)
        {
            var objFromDb = _db.Grades.FirstOrDefault(s => s.Id == grade.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = grade.Name;
            }
        }
    }
}
