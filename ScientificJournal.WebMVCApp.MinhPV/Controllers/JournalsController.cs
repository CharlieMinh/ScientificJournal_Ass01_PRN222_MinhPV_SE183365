using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Services.MinhPV.Interfaces;
using ScientificJournal.WebMVCApp.MinhPV.ViewModels;

namespace ScientificJournal.WebMVCApp.MinhPV.Controllers
{
    [Authorize]
    public class JournalsController : Controller
    {
        private readonly IJournalsMinhPvService _journalsService;
        private readonly IPublishersService _publishersService;
        private readonly ICategoriesMinhPvService _categoriesService;

        public JournalsController(
            IJournalsMinhPvService journalsService,
            IPublishersService publishersService,
            ICategoriesMinhPvService categoriesService)
        {
            _journalsService = journalsService;
            _publishersService = publishersService;
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var journals = await _journalsService.GetAllAsync();
            return View(journals);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _journalsService.GetDetailAsync(id.Value);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        public async Task<IActionResult> Create()
        {
            await LoadPublisherOptionsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JournalName,Issn,Eissn,PublisherId,Country,WebsiteUrl,ImpactFactor,Ranking,Description,IsActive")] JournalsMinhPv journal)
        {
            if (ModelState.IsValid)
            {
                journal.CreatedAt = DateTime.Now;
                await _journalsService.CreateAsync(journal);
                return RedirectToAction(nameof(Index));
            }

            await LoadPublisherOptionsAsync(journal.PublisherId);
            return View(journal);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _journalsService.GetByIdAsync(id.Value);
            if (journal == null)
            {
                return NotFound();
            }

            await LoadPublisherOptionsAsync(journal.PublisherId);
            return View(journal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JournalIdMinhPv,JournalName,Issn,Eissn,PublisherId,Country,WebsiteUrl,ImpactFactor,Ranking,Description,IsActive,CreatedAt")] JournalsMinhPv journal)
        {
            if (id != journal.JournalIdMinhPv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _journalsService.UpdateAsync(journal);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _journalsService.GetByIdAsync(journal.JournalIdMinhPv);
                    if (exists == null)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadPublisherOptionsAsync(journal.PublisherId);
            return View(journal);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _journalsService.GetDetailAsync(id.Value);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _journalsService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                var journal = await _journalsService.GetDetailAsync(id);
                ModelState.AddModelError(string.Empty, "Cannot delete this journal because it has related papers, categories, or follows.");
                return journal == null ? NotFound() : View("Delete", journal);
            }
        }

        public async Task<IActionResult> AssignCategories(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _journalsService.GetWithCategoriesAsync(id.Value);
            if (journal == null)
            {
                return NotFound();
            }

            var selectedIds = journal.CategoryIdMinhPvs.Select(c => c.CategoryIdMinhPv).ToList();
            var categories = await _categoriesService.GetAllAsync();
            var model = new AssignCategoriesViewModel
            {
                JournalId = journal.JournalIdMinhPv,
                JournalName = journal.JournalName,
                SelectedCategoryIds = selectedIds,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryIdMinhPv.ToString(),
                    Text = c.CategoryName,
                    Selected = selectedIds.Contains(c.CategoryIdMinhPv)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCategories(int id, AssignCategoriesViewModel model)
        {
            await _journalsService.AssignCategoriesAsync(id, model.SelectedCategoryIds ?? new List<int>());
            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task LoadPublisherOptionsAsync(int? selectedPublisherId = null)
        {
            var publishers = await _publishersService.GetAllAsync();
            ViewData["PublisherId"] = new SelectList(publishers, "PublisherId", "PublisherName", selectedPublisherId);
        }
    }
}
