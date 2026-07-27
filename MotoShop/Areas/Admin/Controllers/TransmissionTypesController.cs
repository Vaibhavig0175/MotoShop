using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoShop.Interfaces.Services;
using MotoShop.Models;

namespace MotoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TransmissionTypesController : Controller
    {
        private readonly ITransmissionTypeService _transmissionTypeService;

        public TransmissionTypesController(ITransmissionTypeService transmissionTypeService)
        {
            _transmissionTypeService = transmissionTypeService;
        }

        // GET: Admin/TransmissionTypes
        public async Task<IActionResult> Index()
        {
            var transmissionTypes = await _transmissionTypeService.GetAllAsync();
            return View(transmissionTypes);
        }

        // GET: Admin/TransmissionTypes/Create
        public IActionResult Create()
        {
            return View(new TransmissionType());
        }

        // POST: Admin/TransmissionTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransmissionType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;

            await _transmissionTypeService.AddAsync(model);

            TempData["Success"] = "Transmission Type added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/TransmissionTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var transmissionType = await _transmissionTypeService.GetByIdAsync(id);

            if (transmissionType == null)
                return NotFound();

            return View(transmissionType);
        }

        // POST: Admin/TransmissionTypes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransmissionType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var transmissionType = await _transmissionTypeService.GetByIdAsync(model.Id);

            if (transmissionType == null)
                return NotFound();

            transmissionType.Name = model.Name;
            transmissionType.Description = model.Description;
            transmissionType.ModifiedOn = DateTime.UtcNow;

            await _transmissionTypeService.UpdateAsync(transmissionType);

            TempData["Success"] = "Transmission Type updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/TransmissionTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _transmissionTypeService.DeleteAsync(id);

            TempData["Success"] = "Transmission Type deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
