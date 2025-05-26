using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.DTO;
using Microsoft.AspNetCore.Identity;
using GoSkool.Services;

namespace GoSkool.Controllers
{
    public class AccountantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountantService _accountantService;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountantController(ApplicationDbContext context,IAccountantService accountantService,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _accountantService = accountantService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            AccountantHomeDTO accountantHomeDTO = new AccountantHomeDTO();
            var AccountantId = _accountantService.GetCurrentAccountantId(await _userManager.GetUserAsync(HttpContext.User));
            _accountantService.FillAccountantHomeDetails(AccountantId,accountantHomeDTO);
            return View(accountantHomeDTO);
        }

        public IActionResult Class(string SortParam,int ClassId)
        {
            ClassDetailsDTO classDetailsDTO = new ClassDetailsDTO();
            classDetailsDTO.SortParam = SortParam;
            _accountantService.GetClassDetails(ClassId, classDetailsDTO);
            return View(classDetailsDTO);
        }

        
    }
}
