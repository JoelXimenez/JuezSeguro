using JuezSeguro.Data;
using JuezSeguro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuezSeguro.Controllers
{
    [Authorize(Policy = "UsuarioInterno")]
    public class ExpedientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpedientesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Expedientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Expedientes.Where(e => e.Activo).ToListAsync());
        }

        // GET: Expedientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FirstOrDefaultAsync(m => m.Id == id && m.Activo);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        // GET: Expedientes/Create
        [Authorize(Policy = "JuezOAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expedientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "JuezOAdmin")]
        public async Task<IActionResult> Create([Bind("NumeroExpediente,Titulo,Descripcion,Materia,Estado")] Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                expediente.CreadoPorId = _userManager.GetUserId(User)!;
                expediente.FechaApertura = DateTime.UtcNow;
                _context.Add(expediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expediente);
        }

        // GET: Expedientes/Edit/5
        [Authorize(Policy = "JuezOAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        // POST: Expedientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "JuezOAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroExpediente,Titulo,Descripcion,Materia,Estado,FechaApertura,FechaCierre,HashIntegridad,FirmaDigital,CreadoPorId,Activo")] Expediente expediente)
        {
            if (id != expediente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    expediente.ModificadoPorId = _userManager.GetUserId(User);
                    expediente.FechaModificacion = DateTime.UtcNow;
                    _context.Update(expediente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Expedientes.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expediente);
        }

        // GET: Expedientes/Delete/5
        [Authorize(Policy = "SoloAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FirstOrDefaultAsync(m => m.Id == id && m.Activo);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        // POST: Expedientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente != null)
            {
                expediente.Activo = false;
                _context.Update(expediente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
