using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .ToList();

            return View(products);
        }
      
        public IActionResult Details(int id, string? returnUrl)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
            return View(product);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
             
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

     
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                product.UrlImg = "/images/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id, string returnUrl)
        {

            if (id == null || _context.Products == null) {
                return NotFound();
            }

            var product = _context.Products.Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.Category);
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product, IFormFile? ImageFile, string? returnUrl) {
            if (id != product.Id) return BadRequest();

            if (!ModelState.IsValid) 
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
                return View(product);
            }

            var productInDb = _context.Products.Find(id);

            if (productInDb == null) return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

             
                if (!string.IsNullOrEmpty(productInDb.UrlImg))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productInDb.UrlImg.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        try { System.IO.File.Delete(oldFilePath); } catch {  }
                    }
                }

                productInDb.UrlImg = "/images/" + uniqueFileName;
            }

         
            productInDb.Name = product.Name;
            productInDb.Price = product.Price;
            productInDb.CategoryId = product.CategoryId;
            productInDb.Description = product.Description;

            _context.SaveChanges();

            var destination = returnUrl ?? Url.Action("Index");
            return Redirect(destination);




        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
