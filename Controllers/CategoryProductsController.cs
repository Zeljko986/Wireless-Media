using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using Products.Models;

namespace Products.Controllers
{
    public class CategoryProductsController : Controller
    {
        private readonly ProductsDbContext _context;

        public CategoryProductsController(ProductsDbContext context)
        {
            _context = context;
        }

        // GET: CategoryProducts
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryProducts.ToListAsync());
        }

        // Save Category data from database in json file
        [HttpPost]
        public IActionResult Index(CategoryProduct obj)
        {
            var categories = _context.CategoryProducts.ToList();
            string jsondata = new JavaScriptSerializer().Serialize(categories);
            string path = Path.Combine("wwwroot/json/");
            System.IO.File.WriteAllText(path + "CategoryProduct.json", jsondata);
            TempData["msg"] = "Json file Generated! check this in your ~/wwwroot/json folder";
            return RedirectToAction("Index"); ;
        }

        // Import json file in database
        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            string contentRootPath = "wwwroot/json/CategoryProduct.json";
            var jsondata = System.IO.File.ReadAllText(contentRootPath);
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(jsondata);
            categoryProducts.ForEach(p =>
            {
                CategoryProduct categoryProduct = new CategoryProduct()
                {
                    CategoryProductId = p.CategoryProductId,
                    CategoryName = p.CategoryName
                };
                var ourCategoryProducts = _context.CategoryProducts.SingleOrDefault(x => x.CategoryProductId.Equals(p.CategoryProductId));
                if (ourCategoryProducts == null)
                {
                    _context.CategoryProducts.Add(categoryProduct);
                    _context.SaveChanges();
                    ViewBag.Success = "File uploaded Successfully..Please click on the List Of Category";
                }
                
            });
            return View("Index");
        }

        // GET: CategoryProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts
                .FirstOrDefaultAsync(m => m.CategoryProductId == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryProducts/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryProductId,CategoryName")] CategoryProduct categoryProduct)
        {
            var max = _context.CategoryProducts.FirstOrDefault();
            if (max == null)
            {
                categoryProduct.CategoryProductId = 1;
            }
            else
            {
                categoryProduct.CategoryProductId = _context.CategoryProducts.Max(item => item.CategoryProductId) + 1;
            }
            if (ModelState.IsValid)
            {
                _context.Add(categoryProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts.FindAsync(id);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            return View(categoryProduct);
        }

        // POST: CategoryProducts/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryProductId,CategoryName")] CategoryProduct categoryProduct)
        {
            if (id != categoryProduct.CategoryProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryProductExists(categoryProduct.CategoryProductId))
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
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts
                .FirstOrDefaultAsync(m => m.CategoryProductId == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            return View(categoryProduct);
        }

        // POST: CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryProduct = await _context.CategoryProducts.FindAsync(id);
            _context.CategoryProducts.Remove(categoryProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryProductExists(int id)
        {
            return _context.CategoryProducts.Any(e => e.CategoryProductId == id);
        }
    }
}
