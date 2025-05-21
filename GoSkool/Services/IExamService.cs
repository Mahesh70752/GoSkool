using GoSkool.Views.Exam;

namespace GoSkool.Services
{
    public interface IExamService
    {
        void GetExamModelObj(int subjectId, ExamModel examModelObj);
        bool CreateExam(ExamModel examModelObj);
    }
}
