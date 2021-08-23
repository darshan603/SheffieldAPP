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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Student student)
        {
            var objFromDb = _db.Students.FirstOrDefault(s => s.Id == student.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = student.Name;
                objFromDb.Address = student.Address;
                objFromDb.City = student.City;
                objFromDb.TpNo = student.TpNo;
            }
        }
    }
}
