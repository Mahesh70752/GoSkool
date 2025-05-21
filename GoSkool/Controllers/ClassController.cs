using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Views.Class;
using GoSkool.DTO;
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;
using GoSkool.Services;

namespace GoSkool.Controllers
{
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClassController> _logger;
        private readonly IClassService _classService;
        


        public ClassController(ApplicationDbContext context,IClassService classService, ILogger<ClassController> logger)
        {
            _context = context;
            _classService = classService;
            _logger = logger;
        }


        public IActionResult Index(int classId)
        {
            var IndexObj = new IndexModel();
            if (!_classService.GetIndexObj(classId, IndexObj))
            {
                _logger.LogInformation("class not loaded properly");
                return RedirectToAction("Index", "Admin");
            }
            return View(IndexObj);
        }

        public IActionResult AssignTeacher(int classId,int subjectId)
        {
            var TeacherId = Request.Form["TeacherSelection"];
            _classService.AssignTeacher(classId, subjectId, TeacherId);
            return RedirectToAction("Index", new { classId = classId });
        }


        public IActionResult AddSubject(int classId)
        {
            AddSubject addSubject = new AddSubject() { ClassId = classId };
            return View(addSubject);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateSubject(AddSubject addSubject)
        {
            _classService.CreateSubject(addSubject);
            return RedirectToAction("Index", new { classId = addSubject.ClassId });
        }
    }
}
