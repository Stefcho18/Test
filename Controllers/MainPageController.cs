using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

namespace Test.Controllers
{
    public class MainPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainPageController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: MainPage
        public async Task<IActionResult> MainPage()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

   
    }
}
