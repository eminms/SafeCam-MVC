using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCam.DAL;
using SafeCam.Models;
using SafeCam.Utilities;
using SafeCam.Utilities.FileUpload;
using SafeCam.ViewModels.ProductVM;
using System.Threading.Tasks;

namespace SafeCam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var product=_context.Products.Include(p=>p.Category).ToList();
            return View(product);
        }

        public IActionResult Create() 
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM vM)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(vM);
            }
            if (vM == null)
            {
                ModelState.AddModelError("", "Product dont null");
                return View();
            }
            if (vM.File != null)
            {
                if (!vM.File.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("", "File is nor image");
                    return View();
                }
                if (vM.File.Length>2*1024*1024)
                {
                    ModelState.AddModelError("", "Length more than 2mb");
                    return View();
                }
                return View();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == vM.CategoryId);
            var image = await vM.File.SaveImage(_env, "Upload/Product");

            Product product = new()
            {
                Name = vM.Name,
                CategoryId=vM.CategoryId,
                Category=category,
                Image=image
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


        public IActionResult Update(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();

            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            UpdateProductVM vM = new()
            {
                Name = product.Name,

                Id=product.Id,
                CategoryId=product.CategoryId
            };
            return View(vM);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM vM)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(vM);
            }
            if (vM == null)
            {
                ModelState.AddModelError("", "Product dont null");
                return View();
            }
            if (vM.File != null)
            {
                if (vM.File.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("", "File is nor image");
                    return View();
                }
                if (vM.File.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Length more than 2mb");
                    return View();
                }
                return View();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == vM.CategoryId);
            var image = await vM.File.SaveImage(_env, "Upload/Product");

            

            Product existproduct = await _context.Products.FirstAsync(x => x.Id == vM.Id);
            existproduct.Name = vM.Name;
            existproduct.CategoryId = vM.CategoryId;
            existproduct.Category = category;
            existproduct.Image = image;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);
            _context.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
