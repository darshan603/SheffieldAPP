using SheffieldAPP.DataAccess.Data;
using SheffieldAPP.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheffieldAPP.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Subject = new SubjectRepository(_db);
            Grade = new GradeRepository(_db);
            Student = new StudentRepository(_db);
            StudentSubject = new StudentSubjectRepository(_db);
            StudentMarks = new StudentMarksRepository(_db);
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISubjectRepository Subject { get; private set; }
        public IGradeRepository Grade { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IStudentSubjectRepository StudentSubject { get; private set; }
        public IStudentMarksRepository StudentMarks { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
