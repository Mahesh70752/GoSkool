using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GoSkool.Views.Student;
using GoSkool.Views.Teacher;
using GoSkool.Services;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentService _studentService;

        public StudentController(ApplicationDbContext context,UserManager<IdentityUser> userManager,IStudentService studentService)
        {
            _context = context;
            _userManager = userManager;
            _studentService = studentService;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var StudentHomePageObj = new StudentHomePageModel();
            _studentService.GetStudentHomePageObj(user, StudentHomePageObj);
            return View(StudentHomePageObj);

        }
        public async Task<IActionResult> Schedule()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ClassScheduleModel scheduleObj = new ClassScheduleModel();
            _studentService.GetClassScheduleObj(user, scheduleObj);
            return View(scheduleObj);
        }

        
    }
}
