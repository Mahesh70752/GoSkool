using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.DTO;
using GoSkool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GoSkool.Views.Student;
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
            var studentId = _studentService.GetCurrentStudentId(await _userManager.GetUserAsync(HttpContext.User));
            ClassScheduleDTO classScheduleDTO = new ClassScheduleDTO();
            _studentService.GetScheduleData(studentId, classScheduleDTO);
            return View(classScheduleDTO);
        }

        public IActionResult UploadAssignment(StudentHomePageModel studentHomePageObj)
        {
            Console.WriteLine(studentHomePageObj.file.FileName);
            _studentService.UploadAssignment(studentHomePageObj.file);
            return RedirectToAction("Index");
        }

        
    }
}
