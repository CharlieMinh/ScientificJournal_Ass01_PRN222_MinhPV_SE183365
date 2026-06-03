using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.WebMVCApp.MinhPV.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesMinhPvService _categoriesService;

        public CategoriesController(ICategoriesMinhPvService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoriesService.GetDetailAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,Description")] CategoriesMinhPv category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedAt = DateTime.Now;
                await _categoriesService.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoriesService.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryIdMinhPv,CategoryName,Description,CreatedAt")] CategoriesMinhPv category)
        {
            if (id != category.CategoryIdMinhPv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesService.UpdateAsync(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _categoriesService.GetByIdAsync(category.CategoryIdMinhPv);
                    if (exists == null)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoriesService.GetDetailAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoriesService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                var category = await _categoriesService.GetDetailAsync(id);
                ModelState.AddModelError(string.Empty, "Cannot delete this category because it is assigned to journals.");
                return category == null ? NotFound() : View("Delete", category);
            }
        }
    }
}
