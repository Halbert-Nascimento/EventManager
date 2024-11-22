using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManager.Data;
using EventManager.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EventManager.Controllers
{
    public class EventosController : Controller
    {
        private readonly GerenciadorEventosContext _context;

        public EventosController(GerenciadorEventosContext context)
        {
            _context = context;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventos = await _context.Eventos.ToListAsync();
            return View(eventos);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }// usar try chat no lugar do if

            var evento = await _context.Eventos
            .Include(e => e.Convidados)
            .FirstOrDefaultAsync(e => e.Id == id);

            
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        { // pode ser retirado o bind Task<IActionResult> Create(Evento evento)
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }



        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            } // usar um try catch no lugar do if
         

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Local,Data,Hora,QuantidadeMaxPessoas,valorIngresso")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
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
            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ___________________________________________________________apagar

        // GET: Eventos/Register/5
        public async Task<IActionResult> Register(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["EventoId"] = evento.Id;
            ViewData["EventoTitulo"] = evento.Titulo;
            ViewData["QtMaxPessoas"] = evento.QuantidadeMaxPessoas;

            return View();
        }

        // POST: Eventos/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventoId,[Bind("Id,Nome,Sex,Email,EventoId, Evento")] Convidado convidado)
        {
            if (!ModelState.IsValid)
            { // Log dos erros de validação
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Error id >>>>>: {eventoId}");
                        Console.WriteLine($"Error >>>>>: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                // Buscando o evento do banco de dados
                var evento = await _context.Eventos.FindAsync(eventoId); 
                if (evento == null) 
                { 
                    return NotFound(); 
                } // Definindo a relação entre Convidado e Evento
                convidado.EventoId = eventoId; 
                //convidado.Evento = evento;


                _context.Convidados.Add(convidado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = eventoId });
            }
            ViewData["EventoId"] = eventoId;
            return View(convidado);
        }

        // ___________________________________________________________

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
