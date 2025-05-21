using GoSkool.Views.Class;
using GoSkool.DTO;
namespace GoSkool.Services
{
    public interface IClassService
    {
        bool GetIndexObj(int classId, IndexModel IndexObj);
        void AssignTeacher(int classId,int SubjectId, string TeacherId);

        void CreateSubject(AddSubject addSubject);
    }
}
