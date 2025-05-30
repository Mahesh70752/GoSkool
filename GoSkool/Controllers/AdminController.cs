﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string Selection)
        {
            AdminHomeModel adminHomeModel = new AdminHomeModel(_context);
            
            if (!string.IsNullOrWhiteSpace(Selection))
            {
                switch (Selection)
                {
                    case "class":
                        ViewData["ClassSelectionParam"] = "";
                        ViewData["TeacherSelectionParam"] = "teacher";
                        ViewData["AdminSelectionParam"] = "admin";
                        
                        adminHomeModel.classes = await _context.Classes.Include(x => x.Standard).Include(x => x.Section).ToListAsync();
                        await adminHomeModel.FillAllClasses();
                        break;
                    case "teacher":
                        ViewData["TeacherSelectionParam"] = "";
                        ViewData["ClassSelectionParam"] = "class";
                        ViewData["AdminSelectionParam"] = "admin";
                        adminHomeModel.teachers = await _context.Teachers.Include(x=>x.Classes).Include(x=>x.Periods).ToListAsync();
                        break;
                    case "admin":
                        ViewData["ClassSelectionParam"] = "class";
                        ViewData["TeacherSelectionParam"] = "teacher";
                        ViewData["AdminSelectionParam"] = "";
                        adminHomeModel.admins = await _context.Admin.ToListAsync();
                        break;
                    default:
                        break;
                }
                adminHomeModel.selection = Selection;
            }
            else
            {
                ViewData["ClassSelectionParam"] = "class";
                ViewData["TeacherSelectionParam"] = "teacher";
                ViewData["AdminSelectionParam"] = "admin";
            }

                return View(adminHomeModel);
        }

       
    }
}
