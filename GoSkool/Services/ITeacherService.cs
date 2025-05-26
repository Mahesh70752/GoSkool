using GoSkool.DTO;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Services
{
    public interface ITeacherService
    {
        void GetTeacherHomeObj(IdentityUser user,TeacherHomeModel teacherHomeObj);
        int GetCurrentTeacherId(IdentityUser user);
        void GetScheduleData(int teacherId,TeacherScheduleDTO teacherScheduleDTO );
        void GetTeacherAssignments(int teacherId,TeacherHomeModel TeacherHomeObj);
        void FillExamDetails(int ExamId, CheckExamDTO checkExamdto);
        void AddStudentScore(CheckExamDTO checkExamdto);
        void FillClassDetails(TeacherClassDTO classDTO,int teacherId, int ClassId);

        void FillAttendanceRecords(int teacherId,TakeAttendanceDTO takeAttendanceDTO);
        void SubmitAttendance(TakeAttendanceDTO takeAttendanceDTO);
    }
}
