using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManager.Data;
using EventManager.Models;

namespace EventManager.Controllers
{
    public class ConvidadosController : Controller
    {
        private readonly GerenciadorEventosContext _context;

        public ConvidadosController(GerenciadorEventosContext context)
        {
            _context = context;
        }

        // GET: Convidados
        public async Task<IActionResult> Index()
        {
            var gerenciadorEventosContext = _context.Convidados.Include(c => c.Evento);
            return View(await gerenciadorEventosContext.ToListAsync());
        }

        // GET: Convidados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convidado = await _context.Convidados
                .Include(c => c.Evento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (convidado == null)
            {
                return NotFound();
            }

            return View(convidado);
        }

        // GET: Convidados/Create
        //public IActionResult Create()
        //{
        //    ViewData["EventoId"] = new SelectList(_context.Eventos, "Id", "Titulo");
        //    return View();
        //}

        //// POST: Convidados/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Nome,Sex,Email,EventoId")] Convidado convidado)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(convidado);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EventoId"] = new SelectList(_context.Eventos, "Id", "id", convidado.EventoId);
        //    return View(convidado);
        //}

        //// ___________________________________________________________

        // Criação de um convidado (GET)
        public IActionResult Create(int eventoId)
        {
            ViewBag.EventoId = eventoId;
            return View();
        }

        // Criação de um convidado (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Convidado convidado)
        {
            if (ModelState.IsValid)
            {
                _context.Convidados.Add(convidado);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Eventos", new { id = convidado.EventoId });
            }

            ViewBag.EventoId = convidado.EventoId;
            return View(convidado);
        }

        //// ___________________________________________________________

        // GET: Convidados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convidado = await _context.Convidados.FindAsync(id);
            if (convidado == null)
            {
                return NotFound();
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "Id", "Descricao", convidado.EventoId);
            return View(convidado);
        }

        // POST: Convidados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Sex,Email,EventoId")] Convidado convidado)
        {
            if (id != convidado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(convidado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConvidadoExists(convidado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "Id", "Descricao", convidado.EventoId);
            return View(convidado);
        }

        // GET: Convidados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convidado = await _context.Convidados
                .Include(c => c.Evento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (convidado == null)
            {
                return NotFound();
            }

            return View(convidado);
        }

        // POST: Convidados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var convidado = await _context.Convidados.FindAsync(id);
            if (convidado != null)
            {
                _context.Convidados.Remove(convidado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConvidadoExists(int id)
        {
            return _context.Convidados.Any(e => e.Id == id);
        }
    }
}
