#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonitorTargetApp.Data;
using MonitorTargetApp.Models;
using MonitorTargetApp.Repositories.TargetApp;

namespace MonitorTargetApp.Controllers
{
    public class TargetAppController : Controller
    {
        private readonly ITargetAppRepository _targetAppRepository;

        public TargetAppController(ITargetAppRepository targetAppRepository)
        {
            _targetAppRepository = targetAppRepository;
        }

        // GET: TargetApp
        public async Task<IActionResult> Index()
        {
            return View(_targetAppRepository.Index());
        }

        // GET: TargetApp/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var targetApp = _targetAppRepository.Details(id);
            if(targetApp == null)
                return NotFound();

            return View(targetApp);
        }

        // GET: TargetApp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TargetApp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Url,Name")] TargetApp targetApp)
        {
            if (ModelState.IsValid)
            {
                _targetAppRepository.Create(targetApp);
                return RedirectToAction(nameof(Index));
            }
            return NoContent();
        }

        // GET: TargetApp/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetApp = _targetAppRepository.Details(id);
            if (targetApp == null)
            {
                return NotFound();
            }
            return View(targetApp);
        }

        // POST: TargetApp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Name,LastStatus")] TargetApp targetApp)
        {
            if (id != targetApp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var EditedTargetApp=_targetAppRepository.Edit(id,targetApp);
                return RedirectToAction(nameof(Index));
            }
            return View(targetApp);
        }

        // GET: TargetApp/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetApp = _targetAppRepository.Details(id);
            if (targetApp == null)
            {
                return NotFound();
            }
            return View(targetApp);
        }

        // POST: TargetApp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _targetAppRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
