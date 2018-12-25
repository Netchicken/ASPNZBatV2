using System;
using System.Collections.Generic;
using System.Text;
using ASPNZBat;
namespace Tests
{
    using ASPNZBat.Business;
    using ASPNZBat.Models;

    class StudentsFake
    {
        //https://code-maze.com/unit-testing-aspnetcore-web-api/
        readonly List<Students> _myStudents;

        public StudentsFake()
        {
            _myStudents = new List<Students>() {
            new Students() { ID = "1", Email = "aaa.aaa.com" },
            new Students() { ID = "2", Email = "bbb.bbb.com" },
            new Students() { ID = "3", Email = "ccc.ccc.com" }
            };
        }

        public IEnumerable<Students> GetAllStudents()
        {
            return _myStudents;
        }

    }
}
