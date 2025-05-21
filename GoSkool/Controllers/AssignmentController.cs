using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Assignment;
using GoSkool.Services;

namespace GoSkool.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly IAssignmentService _assignmentService;

        public AssignmentController(ApplicationDbContext context,IAssignmentService assignmentService)
        {
            _context = context;
            _assignmentService = assignmentService;
        }

        public IActionResult TeacherAssignment(int asId)
        {
            var TeacherAssignmentObj = new TeacherAssignmentModel();
            _assignmentService.GetTeacherAssignment(asId, TeacherAssignmentObj);
            return View(TeacherAssignmentObj);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _assignmentService.GetAssignmentsAsync());
        }


        public IActionResult Create(int teacherId)
        {
            var AssignmentCreationObj = new AssignmentCreationModel();
            _assignmentService.GetAssignmentCreationObj(teacherId, AssignmentCreationObj);
            return View(AssignmentCreationObj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(AssignmentCreationModel assignmentCreationObj)
        {
            _assignmentService.CreateAssignment(assignmentCreationObj);
            return RedirectToAction("Index", "Teacher");
        }

        
    }
}
