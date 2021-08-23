using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheffieldAPP.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ISubjectRepository Subject { get; }
        IGradeRepository Grade { get; }
        IStudentRepository Student { get; }
        IStudentSubjectRepository StudentSubject { get; }
        IStudentMarksRepository StudentMarks { get; }
        void Save();
    }
}
