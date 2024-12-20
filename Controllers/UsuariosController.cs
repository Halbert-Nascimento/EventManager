﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManager.Data;
using EventManager.Models;
using Microsoft.CodeAnalysis.Scripting;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace EventManager.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GerenciadorEventosContext _context;

        public UsuariosController(GerenciadorEventosContext context)
        {
            _context = context;
        }
        // ------------------------------------------ implemetação de login --------------

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null || !usuario.VerificarSenha(senha))
            {
                ViewBag.Erro = "Email ou senha inválidos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            var identify = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identify);

            await HttpContext.SignInAsync("CookieAuth", principal);


            
            return RedirectToAction(nameof(Details));
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            TempData["MensagemSucesso"] = $"Logout realizado com sucesso ";
            return RedirectToAction("Login", "Usuarios");
        }


        // ------------------------------------------

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details()
        {
            // Recupera o ID do usuário logado
            var usuarioId = User.FindFirst("UsuarioId")?.Value;

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            int id = int.Parse(usuarioId);

            var usuario = await _context.Usuarios
                .Include(e => e.Eventos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,SenhaHash")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Gere o hash para senha
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = $"Conta criada, faça o login usando email e senha cadastrados!";
                return RedirectToAction(nameof(Login));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return RedirectToAction("NoAccess", "Home");
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,SenhaHash")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Gere o hash para senha
                    usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return RedirectToAction("NoAccess", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["MensagemSucesso"] = $"Alterações salvas";
                return RedirectToAction(nameof(Details));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            await HttpContext.SignOutAsync("CookieAuth");
            TempData["MensagemSucesso"] = $"Conta excluida";
            return RedirectToAction(nameof(Create));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
