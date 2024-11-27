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
using Microsoft.AspNetCore.Authorization;


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
        // GET: Eventos
        public async Task<IActionResult> MyEvent()
        {
            // Recupera o ID do usuário logado
            var usuarioId = User.FindFirst("UsuarioId")?.Value;

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            int id = int.Parse(usuarioId);

            ViewBag.UsuarioId = id;

            var eventos = await _context.Eventos.Where(e => e.UsuarioId == id).ToListAsync();
            return View(eventos);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.totalConvidados = 0;
            if (id == null)
            {
                return NotFound();
            }           

            //recuperar evento
            var evento = await _context.Eventos.FindAsync(id);
            if(evento == null)
            {
                return NotFound();
            }

            //verifcar se usuario esta autenticado
            if (User.Identity.IsAuthenticated)
            {
                //recurar o id do usuario logado
                var usuarioId = User.FindFirst("UsuarioId")?.Value;
                //verifica se usuario id não e nulo e se o evento pertence a esse usuario 
                if(usuarioId != null && int.Parse(usuarioId) == evento.UsuarioId)
                {
                    //faz a busca dos convidados
                    evento = await _context.Eventos
                        .Include(e => e.Convidados)
                        .FirstOrDefaultAsync(e => e.Id == id);
                        ViewBag.totalConvidados = evento.Convidados.Count;
                }
                else
                {
                    TempData["MensagemError"] = $"Sem permição para visualizar participantes";
                }
            }
            var totalConvidados = await _context.Convidados
                .Where(c => c.EventoId == id)
                .CountAsync();
            ViewBag.totalConvidados = totalConvidados;

            return View(evento);
        }

        // GET: Eventos/Create
        [Authorize]
        public IActionResult Create()
        {
            // Recupera o ID do usuário logado
            var usuarioId = User.FindFirst("UsuarioId")?.Value;

            if(usuarioId == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }

            ViewBag.UsuarioId = int.Parse(usuarioId);
            return View();
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        { // pode ser retirado o bind Task<IActionResult> Create(Evento evento)
            if (ModelState.IsValid)
            {
                _context.Eventos.Add(evento);
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = $"Evento Criado com sucesso";
                return RedirectToAction("Details", "Usuarios", new { id = evento.UsuarioId });
            }
            return View(evento);
        }



        // GET: Eventos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
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

            // Recupera o ID do usuário logado
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);
            
            if( evento.UsuarioId != usuarioId)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            ViewBag.UsuarioId = usuarioId;
            return View(evento);
        }

        // POST: Eventos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Local,Data,Hora,QuantidadeMaxPessoas,valorIngresso, UsuarioId")] Evento evento)
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
                TempData["MensagemSucesso"] = $"Alteração realizada ";
                return RedirectToAction("Details", "Usuarios");
            }
            return View(evento);
        }

        // GET: Eventos/Delete/5
        [Authorize]
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
            // Recupera o ID do usuário logado
            var usuarioId = int.Parse(User.FindFirst("UsuarioId")?.Value);

            if (evento.UsuarioId != usuarioId)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [Authorize]
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
            TempData["MensagemSucesso"] = $"Evento apagado com sucesso ";
            return RedirectToAction("Details", "Usuarios");
        }

        
        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
