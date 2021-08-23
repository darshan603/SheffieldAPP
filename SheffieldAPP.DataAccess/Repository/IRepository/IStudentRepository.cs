using SheffieldAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheffieldAPP.DataAccess.Repository.IRepository
{
    public interface IStudentRepository: IRepository<Student>
    {
        void Update(Student student);
    }
}
