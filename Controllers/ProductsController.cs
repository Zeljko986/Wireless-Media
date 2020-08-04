using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Products.Models;
using Nancy.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Products.Data;

namespace Products.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsDbContext _context;        

        public ProductsController(ProductsDbContext context)
        {
            _context = context;            
        }

        // GET: Products
        // List of All Products on page
        public async Task<IActionResult> Index()
        {
            var productsDbContext = _context.Products.Include(p => p.CategoryProduct);
            return View(await productsDbContext.ToListAsync());
        }

        // Save data from database in json file
        [HttpPost]
        public IActionResult Index(Product obj)
        {   
            var produts = _context.Products.ToList();
            string jsondata = new JavaScriptSerializer().Serialize(produts);
            string path = Path.Combine("wwwroot/json/");
            System.IO.File.WriteAllText(path + "Product.json", jsondata);
            TempData["msg"] = "Json file Generated! check this in your ~/wwwroot/json folder";            
            return RedirectToAction("Index"); 
        }

        // Import json file in database
        [HttpPost]
        public IActionResult ImportProducts()
        {
            string contentRootPath = "wwwroot/json/Product.json";
            var jsondata = System.IO.File.ReadAllText(contentRootPath);
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsondata);
            products.ForEach(p =>
            {                
                Product product = new Product()
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryProductId = p.CategoryProductId,
                    CategoryProduct = p.CategoryProduct,
                    Manufacturer = p.Manufacturer,
                    Supplier = p.Supplier,
                    Price = p.Price
                };
                var ourProducts = _context.Products.SingleOrDefault(x => x.ProductId.Equals(p.ProductId));
                if (ourProducts == null)                
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    ViewBag.Success = "File uploaded Successfully..Please click on the List Of Products";
                }                                
            });            
            return View("Index");
        }

        // GET: Products/Details/5
        // Details for Product
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        // Create and Save new Product
        public IActionResult Create()
        {
            ViewData["CategoryProductId"] = new SelectList(_context.CategoryProducts, "CategoryProductId", "CategoryName");
            return View();
        }

        // POST: Products/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,CategoryProductId,Manufacturer,Supplier,Price")] Product product)
        {
            var max = _context.Products.FirstOrDefault();
            if (max == null)
            {
                product.ProductId = 1;
            }
            else
            {
                product.ProductId = _context.Products.Max(item => item.ProductId) + 1;
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }           
            return View(product);
        }

        // GET: Products/Edit/5
        // Edit Product
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryProductId"] = new SelectList(_context.CategoryProducts, "CategoryProductId", "CategoryName", product.CategoryProductId);
            return View(product);
        }

        // POST: Products/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,CategoryProductId,Manufacturer,Supplier,Price")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        // Delete Product
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
