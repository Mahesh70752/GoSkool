using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoSkool.Data;
using GoSkool.Models;
using GoSkool.Services;
using GoSkool.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GoSkool.Controllers
{
    [Authorize(Roles ="Driver")]
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDriverService _driverservice;
        private readonly UserManager<IdentityUser> _userManager;

        public DriverController(ApplicationDbContext context, IDriverService driverService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _driverservice = driverService;
            _userManager = userManager;
        }

        [HttpPost]

        public IActionResult savelocation([FromBody] LocationDTO location)
        {
            _driverservice.SaveLocation(location);
            return RedirectToAction("RouteMap");
        }

        public async Task<IActionResult> RouteMap()
        {
            DriverDTO driver = new DriverDTO();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _driverservice.FillDriverDetails(user, driver);
            return View(driver);          
        }

        // GET: Driver
        public async Task<IActionResult> Index()
        {
            return View(await _context.Driver.ToListAsync());
        }

        // GET: Driver/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverEntity = await _context.Driver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverEntity == null)
            {
                return NotFound();
            }

            return View(driverEntity);
        }

        // GET: Driver/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Driver/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Salary,BusNumber")] DriverEntity driverEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driverEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driverEntity);
        }

        // GET: Driver/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverEntity = await _context.Driver.FindAsync(id);
            if (driverEntity == null)
            {
                return NotFound();
            }
            return View(driverEntity);
        }

        // POST: Driver/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Salary,BusNumber")] DriverEntity driverEntity)
        {
            if (id != driverEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driverEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverEntityExists(driverEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(driverEntity);
        }

        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverEntity = await _context.Driver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverEntity == null)
            {
                return NotFound();
            }

            return View(driverEntity);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driverEntity = await _context.Driver.FindAsync(id);
            if (driverEntity != null)
            {
                _context.Driver.Remove(driverEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverEntityExists(int id)
        {
            return _context.Driver.Any(e => e.Id == id);
        }
    }
}
