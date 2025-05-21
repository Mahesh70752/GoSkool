using GoSkool.Views.Student;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Services
{
    public interface IStudentService
    {
        int GetCurrentStudentId(IdentityUser user);
        void GetStudentHomePageObj(IdentityUser user, StudentHomePageModel StudentHomePageObj);

        void GetClassScheduleObj(IdentityUser user, ClassScheduleModel scheduleObj);
    }
}
