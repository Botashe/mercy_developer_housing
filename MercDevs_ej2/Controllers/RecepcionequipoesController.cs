using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MercDevs_ej2.Models;
using Microsoft.CodeAnalysis;
using System;

namespace MercDevs_ej2.Controllers
{
    public class RecepcionequipoesController : Controller
    {
        private readonly MercyDeveloperContext _context;

        public RecepcionequipoesController(MercyDeveloperContext context)
        {
            _context = context;
        }

        // GET: Recepcionequipoes
        public async Task<IActionResult> Index()
        {
            var mercydeveloperContext = _context.Recepcionequipos.Include(r => r.IdClienteNavigation).Include(r => r.IdServicioNavigation);
            return View(await mercydeveloperContext.ToListAsync());
        }

        public IActionResult FichaTecnica(int idRecepcionEquipo)
        {
            var recepcionEquipo = _context.Recepcionequipos
                .Include(r => r.IdClienteNavigation) // Incluir datos del cliente
                .FirstOrDefault(r => r.Id == idRecepcionEquipo);

            if (recepcionEquipo == null)
            {
                return NotFound();
            }

            return View(recepcionEquipo);
        }


        // POST: Recepcionequipoes/FinalizarServicio/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarServicio(int id)
        {
            var equipo = await _context.Recepcionequipos.FindAsync(id);

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.Estado = 0;

            _context.Update(equipo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EquiposRecepcionadosDelCliente), new { clienteId = equipo.IdCliente });
        }



        // GET: Recepcionequipoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionequipo = await _context.Recepcionequipos
                .Include(r => r.IdClienteNavigation)
                .Include(r => r.IdServicioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recepcionequipo == null)
            {
                return NotFound();
            }

            return View(recepcionequipo);
        }

        // GET: Recepcionequipoes/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente");
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio");
            return View();
        }

        // POST: Recepcionequipoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,IdServicio,Fecha,TipoPc,Accesorio,MarcaPc,ModeloPc,Nserie,CapacidadRam,TipoAlmacenamiento,CapacidadAlmacenamiento,TipoGpu,Grafico,Estado")] Recepcionequipo recepcionequipo)
        {

            var tablaDato = new Dictionary<string, object?>
    {
        { nameof(recepcionequipo.MarcaPc), recepcionequipo.MarcaPc  },
        { nameof(recepcionequipo.Fecha), recepcionequipo.Fecha },
        { nameof(recepcionequipo.TipoPc), recepcionequipo.TipoPc },
        { nameof(recepcionequipo.Accesorio), recepcionequipo.Accesorio },
        { nameof(recepcionequipo.ModeloPc), recepcionequipo.ModeloPc },
        { nameof(recepcionequipo.Nserie), recepcionequipo.Nserie },
        { nameof(recepcionequipo.CapacidadRam), recepcionequipo.CapacidadRam },
        { nameof(recepcionequipo.TipoAlmacenamiento), recepcionequipo.TipoAlmacenamiento },
        { nameof(recepcionequipo.CapacidadAlmacenamiento), recepcionequipo.CapacidadAlmacenamiento },
        { nameof(recepcionequipo.TipoGpu), recepcionequipo.TipoGpu },
        { nameof(recepcionequipo.Grafico), recepcionequipo.Grafico },
        { nameof(recepcionequipo.Estado), recepcionequipo.Estado }
    };

            foreach (var dato in tablaDato)
            {
                if (dato.Value == null || (dato.Value is int intValue && intValue == 0))
                {
                    ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", recepcionequipo.IdCliente);
                    ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", recepcionequipo.IdServicio);
                    return View(recepcionequipo);
                }
            }

            _context.Add(recepcionequipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EquiposRecepcionadosDelCliente(int clienteId)
        {
            var equiposRecepcionados = await _context.Recepcionequipos
                .Include(re => re.IdClienteNavigation)
                .Where(re => re.IdCliente == clienteId) 
                .ToListAsync();

            return View(equiposRecepcionados);
        }

        // GET: Recepcionequipoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionequipo = await _context.Recepcionequipos.FindAsync(id);
            if (recepcionequipo == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", recepcionequipo.IdCliente);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", recepcionequipo.IdServicio);
            return View(recepcionequipo);
        }

        // POST: Recepcionequipoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCliente,IdServicio,Fecha,TipoPc,Accesorio,MarcaPc,ModeloPc," +
    "Nserie,CapacidadRam,TipoAlmacenamiento,CapacidadAlmacenamiento,TipoGpu,Grafico,Estado")] Recepcionequipo recepcionequipo)
        {
            if (id != recepcionequipo.Id)
            {
                return NotFound();
            }

            if (recepcionequipo.IdCliente != 0) // Verificar si el modelo es válido
            {
                try
                {
                    _context.Update(recepcionequipo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionequipoExists(recepcionequipo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios. El registro fue actualizado o eliminado por otro usuario.");
                    }
                }
                catch (Exception ex)
                {
                    // Imprimir el mensaje de error en la consola
                    Console.WriteLine($"Error al guardar en la base de datos: {ex.Message}");

                    // También puedes imprimir detalles adicionales si los necesitas
                    Console.WriteLine($"Detalles adicionales: {ex.InnerException}");

                    ModelState.AddModelError(string.Empty, $"Ocurrió un error: {ex.Message}");
                }
            }

            // Si el modelo no es válido o si ocurre un error, vuelve a cargar los datos necesarios para mostrar la vista de edición
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", recepcionequipo.IdCliente);
            ViewData["IdServicio"] = new SelectList(_context.Servicios, "IdServicio", "IdServicio", recepcionequipo.IdServicio);
            return View(recepcionequipo);
        }

        private bool RecepcionequipoExists(int id)
        {
            return _context.Recepcionequipos.Any(e => e.Id == id);
        }

        // GET: Recepcionequipoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionequipo = await _context.Recepcionequipos
                .Include(r => r.IdClienteNavigation)
                .Include(r => r.IdServicioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recepcionequipo == null)
            {
                return NotFound();
            }

            return View(recepcionequipo);
        }

        // POST: Recepcionequipoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recepcionequipo = await _context.Recepcionequipos.FindAsync(id);
            if (recepcionequipo != null)
            {
                _context.Recepcionequipos.Remove(recepcionequipo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
