namespace ASPNZBat.Business
{
    using System.Threading.Tasks;
    using Areas.Identity.Pages.Account;
    using Microsoft.AspNetCore.Mvc;

    public interface IAddUserToStudentTable
    {

        bool AddUserToStudent(string Email);
    }
}