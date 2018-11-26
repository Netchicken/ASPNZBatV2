using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BATTest
{
    using ASPNZBat.Business;

    [TestClass]
    public class OverdueStudentTest
    {
        private OverdueStudents _overdueStudents;

        public OverdueStudentTest(OverdueStudents overdueStudents)
        {
            _overdueStudents = overdueStudents;
        }


        [TestMethod]
        public void AllStudentsHasData()
        {
            Assert.IsNotNull(_overdueStudents.AllStudents(), "We have data in the DB");
        }
    }
}
