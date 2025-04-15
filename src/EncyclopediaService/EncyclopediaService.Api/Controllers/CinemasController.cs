using EncyclopediaService.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Controllers
{
    [Route("cinemas")]
    public class CinemasController : Controller
    {
        // GET: cinemas
        public async Task<IActionResult> Index()
        {
            var vm = new CinemasViewModel();

            return View(vm);
        }

        // GET: cinemas/:{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vm = new CinemasViewModel();

            return View(vm);
        }

        [HttpGet("cinema")]
        public async Task<IActionResult> Cinema()
        {
            return View();
        }

        // GET: cinemas/Create
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: cinemas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: cinemas/Edit/5
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: cinemas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: cinemas/Delete/5
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: cinemas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
