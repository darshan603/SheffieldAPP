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
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly ApplicationDbContext _db;
        public SubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Subject subject)
        {
            var objFromDb = _db.Subjects.FirstOrDefault(s => s.Id == subject.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = subject.Name;
            }
        }
    }
}
