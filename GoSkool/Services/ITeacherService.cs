using GoSkool.DTO;
using GoSkool.Views.Teacher;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Services
{
    public interface ITeacherService
    {
        void GetTeacherHomeObj(IdentityUser user,TeacherHomeModel teacherHomeObj);
        int GetCurrentTeacherId(IdentityUser user);
        void GetScheduleData(int teacherId,ScheduleModel teacherScheduleObj );
        void GetTeacherAssignments(int teacherId,TeacherHomeModel TeacherHomeObj);
        void FillExamDetails(int ExamId, CheckExamDTO checkExamdto);
        void AddStudentScore(CheckExamDTO checkExamdto);
    }
}
