using GoSkool.Models;
using GoSkool.Views.Assignment;

namespace GoSkool.Services
{
    public interface IAssignmentService
    {
        void GetTeacherAssignment(int asId, TeacherAssignmentModel TeacherAssignmentObj);
        Task<List<AssignmentEntity>> GetAssignmentsAsync();
        void GetAssignmentCreationObj(int teacherId, AssignmentCreationModel AssignmentCreationObj);
        void CreateAssignment(AssignmentCreationModel assignmentCreationObj);
    }
}
