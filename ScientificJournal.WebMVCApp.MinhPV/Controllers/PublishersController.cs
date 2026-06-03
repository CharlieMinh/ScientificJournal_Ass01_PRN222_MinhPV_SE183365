using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.WebMVCApp.MinhPV.Controllers
{
    [Authorize]
    public class PublishersController : Controller
    {
        private readonly IPublishersService _publishersService;

        public PublishersController(IPublishersService publishersService)
        {
            _publishersService = publishersService;
        }

        public async Task<IActionResult> Index()
        {
            var publishers = await _publishersService.GetAllAsync();
            return View(publishers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _publishersService.GetDetailAsync(id.Value);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublisherName,Country,WebsiteUrl")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                publisher.CreatedAt = DateTime.Now;
                await _publishersService.CreateAsync(publisher);
                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _publishersService.GetByIdAsync(id.Value);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublisherId,PublisherName,Country,WebsiteUrl,CreatedAt")] Publisher publisher)
        {
            if (id != publisher.PublisherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _publishersService.UpdateAsync(publisher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _publishersService.GetByIdAsync(publisher.PublisherId);
                    if (exists == null)
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _publishersService.GetDetailAsync(id.Value);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _publishersService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                var publisher = await _publishersService.GetDetailAsync(id);
                ModelState.AddModelError(string.Empty, "Cannot delete this publisher because it is being used by journals.");
                return publisher == null ? NotFound() : View("Delete", publisher);
            }
        }
    }
}
