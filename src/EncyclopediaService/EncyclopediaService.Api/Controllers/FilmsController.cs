using EncyclopediaService.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Controllers
{
    [Route("films")]
    public class FilmsController : Controller
    {
        // GET: films
        public async Task<IActionResult> Index()
        {
            var vm = new FilmsViewModel();

            return View(vm);
        }

        // GET: films/:{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vm = new FilmsViewModel();

            return View(vm);
        }

        [HttpGet("cinema")]
        public async Task<IActionResult> Cinema()
        {
            return View();
        }

        // GET: films/Create
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: films/Create
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

        // GET: films/Edit/5
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: films/Edit/5
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

        // GET: films/Delete/5
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: films/Delete/5
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
