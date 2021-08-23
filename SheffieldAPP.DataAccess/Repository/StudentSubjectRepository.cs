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
    public class StudentSubjectRepository : Repository<StudentSubject>, IStudentSubjectRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentSubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
