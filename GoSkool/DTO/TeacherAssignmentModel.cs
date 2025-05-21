using GoSkool.Models;

namespace GoSkool.Views.Assignment
{
    public class TeacherAssignmentModel
    {
        public AssignmentEntity Assignment {  get; set; }
        public List<Tuple<StudentEntity,string>> Students { get; set; }
    }
}
