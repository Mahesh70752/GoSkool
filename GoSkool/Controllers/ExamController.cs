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
using GoSkool.Views.Exam;
using GoSkool.Services;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Admin,Teacher")]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IExamService _examService;

        public ExamController(ApplicationDbContext context,IExamService examService)
        {
            _context = context;
            _examService = examService;
        }

        public IActionResult Create(int subjectId)
        {
            var examModelObj = new ExamModel();
            _examService.GetExamModelObj(subjectId,examModelObj);
            return View(examModelObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamModel examModelObj)
        {
            if(await _examService.CreateExam(examModelObj)) return RedirectToAction("Index","Home");
            return View(examModelObj);
        }
       
    }
}
