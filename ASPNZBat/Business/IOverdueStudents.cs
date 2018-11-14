using System.Collections;
using System.Collections.Generic;

namespace ASPNZBat.Business
{
    public interface IOverdueStudents
    {
        IEnumerable<string> FindOverDueStudents();
    }
}