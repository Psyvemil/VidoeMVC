using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidoeMVC.DAL;
using vidoeMVC.Models;
using vidoeMVC.ViewModels.Categories;

namespace vidoeMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(VidoeDBContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = await _context.Categories.Where(x => !x.IsDeleted).Select(s => new GetCategoryAdminVM
            {
                Name = s.Name,
                Id = s.Id,
                CreatedTime = s.CreatedTime,
                UpdatedTime = s.UpdatedTime,


            }).ToListAsync();
            return View(data);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {
            if (vm.Name != null && await _context.Categories.AnyAsync(c => c.Name == vm.Name))
            {
                ModelState.AddModelError("Name", "Ad movcuddur");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Category category = new Category
            {
                Name = vm.Name,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now


            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            UpdateCategoryVM categoryVM = new UpdateCategoryVM { Name = category.Name };
            return View(categoryVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, UpdateCategoryVM categoryVM)
        {
            if (id == null || id < 1) return BadRequest();
            Category exisdet = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (exisdet == null) return NotFound();
            exisdet.Name = categoryVM.Name;
            exisdet.UpdatedTime= DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            var delS = await _context.Categories.FindAsync(id);
            if (delS == null) return BadRequest();
            _context.Categories.Remove(delS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
