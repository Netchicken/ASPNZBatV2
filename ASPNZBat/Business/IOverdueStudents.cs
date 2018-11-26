using System.Collections;
using System.Collections.Generic;

namespace ASPNZBat.Business
{
    public interface IOverdueStudents
    {
        IEnumerable<string> FindOverDueStudents(IEnumerable<string> StudentsWithCurrentSchedules,
            IEnumerable<string> AllStudents);

        List<string> AllStudents();

        List<string> StudentsWithCurrentSchedules();
    }
}