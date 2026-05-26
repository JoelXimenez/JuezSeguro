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
            // Solo muestra aquellos expedientes cuyo estado NO sea "ARCHIVADO" (simulando que está activo)
            return View(await _context.Expedientes.Where(e => e.EstadoCod != "ARCHIVADO").ToListAsync());
        }

        // GET: Expedientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FirstOrDefaultAsync(m => m.Id == id && m.EstadoCod != "ARCHIVADO");
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
        public async Task<IActionResult> Create([Bind("NumeroExpediente,Titulo,Descripcion,MateriaCod,EstadoCod,IdTribunal")] Expediente expediente)
        {
            // Omitimos validación del campo que llenaremos del lado del servidor
            ModelState.Remove("CreadoPorId");
            ModelState.Remove("PseudonimoCreador");

            if (ModelState.IsValid)
            {
                expediente.CreadoPorId = _userManager.GetUserId(User)!;
                expediente.PseudonimoCreador = User.Identity?.Name ?? "Anónimo";
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroExpediente,Titulo,Descripcion,MateriaCod,EstadoCod,IdTribunal")] Expediente expediente)
        {
            if (id != expediente.Id) return NotFound();

            // Ignorar validación de campos requeridos que no se actualizan desde este formulario
            ModelState.Remove("CreadoPorId");
            ModelState.Remove("PseudonimoCreador");

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar el registro original en la base de datos
                    var expedienteDb = await _context.Expedientes.FindAsync(id);
                    if (expedienteDb == null) return NotFound();

                    // Actualizar sólo los campos permitidos
                    expedienteDb.NumeroExpediente = expediente.NumeroExpediente;
                    expedienteDb.Titulo = expediente.Titulo;
                    expedienteDb.Descripcion = expediente.Descripcion;
                    expedienteDb.MateriaCod = expediente.MateriaCod;
                    expedienteDb.EstadoCod = expediente.EstadoCod;
                    expedienteDb.IdTribunal = expediente.IdTribunal;

                    expedienteDb.FechaModificacion = DateTime.UtcNow;

                    _context.Update(expedienteDb);
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
            var expediente = await _context.Expedientes.FirstOrDefaultAsync(m => m.Id == id && m.EstadoCod != "ARCHIVADO");
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
                expediente.EstadoCod = "ARCHIVADO"; // "Activo = false" fue reemplazado por ESTADO_COD = ARCHIVADO
                _context.Update(expediente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
