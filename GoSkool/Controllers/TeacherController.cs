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
using GoSkool.Services;
using GoSkool.DTO;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Teacher")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITeacherService _teacherService;

        public TeacherController(ApplicationDbContext context,UserManager<IdentityUser> userManager,ITeacherService teacherService)
        {
            _context = context;
            _userManager = userManager;
            _teacherService = teacherService;
        }

        // GET: Teacher
        public async Task<IActionResult> Index()
        {
            var TeacherHomeObj = new TeacherHomeModel();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _teacherService.GetTeacherHomeObj(user, TeacherHomeObj);
            return View(TeacherHomeObj);
        }

        public async Task<IActionResult> Schedule()
        {
            var teacherId = _teacherService.GetCurrentTeacherId(await _userManager.GetUserAsync(HttpContext.User));
            TeacherScheduleDTO teacherScheduleDTO = new TeacherScheduleDTO();
            _teacherService.GetScheduleData(teacherId, teacherScheduleDTO);
            
            return View(teacherScheduleDTO);
        }

        public async Task<IActionResult> Assignments()
        {
            var TeacherHomeObj = new TeacherHomeModel();
            var teacherId = _teacherService.GetCurrentTeacherId(await _userManager.GetUserAsync(HttpContext.User));
            _teacherService.GetTeacherAssignments(teacherId, TeacherHomeObj);
            return View(TeacherHomeObj);
        }

        public IActionResult CheckExam(int ExamId)
        {
            CheckExamDTO checkExamdto = new CheckExamDTO();
            _teacherService.FillExamDetails(ExamId,checkExamdto);
            return View(checkExamdto);
        }

        public IActionResult AddScore(CheckExamDTO checkExamdto)
        {
            _teacherService.AddStudentScore(checkExamdto);
            return RedirectToAction("CheckExam", new { ExamId = checkExamdto.ExamId });
        }

        public IActionResult Class(int teacherId, int ClassId)
        {
            TeacherClassDTO classdto = new TeacherClassDTO();
            _teacherService.FillClassDetails(classdto, teacherId,  ClassId);
            return View(classdto);
        }

        public async Task<IActionResult> TakeAttendance()
        {
            TakeAttendanceDTO takeAttendanceDTO = new TakeAttendanceDTO();
            var teacherId = _teacherService.GetCurrentTeacherId(await _userManager.GetUserAsync(HttpContext.User));
            _teacherService.FillAttendanceRecords(teacherId,takeAttendanceDTO);
            return View(takeAttendanceDTO);
        }

        public IActionResult SubmitAttendance(TakeAttendanceDTO takeAttendanceDTO)
        {
            _teacherService.SubmitAttendance(takeAttendanceDTO);
            return RedirectToAction("Index","Home");
        }

    }
}
