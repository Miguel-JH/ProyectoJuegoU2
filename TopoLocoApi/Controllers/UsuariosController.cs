﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TopoLocoApi.Models;
using TopoLocoApi.Repositories;
using TopoLocoApi.Services;

namespace TopoLocoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        Sistem21TopolocoContext context;
        Repository<Usuario> repository;
        private readonly IHubContext<TopoLocoHub> numerosHub;
        public UsuariosController(Sistem21TopolocoContext context, IHubContext<TopoLocoHub> contextHub)
        {
            this.context = context;
            repository = new Repository<Usuario>(context);
            this.numerosHub = contextHub;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var u = repository.Get();
            return Ok(u);
        }

        [HttpPost]
        public IActionResult Post(Usuario user)
        {
            if (user == null)
            {
                return BadRequest("Usuario vacío");
            }

            try
            {
                repository.Insert(user);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public IActionResult Put(Usuario user)
        {
            if (user == null)
            {
                return BadRequest("Usuario vacío");
            }

            var u = repository.GetByName(user.NombreUsuario);

            if (u == null)
            {
                return NotFound("Usuario no encontrado");
            }
            u.NombreUsuario = user.NombreUsuario;
            
            if(user.Puntaje > u.Puntaje)
            u.Puntaje = user.Puntaje;

            try
            {
                repository.Update(u);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("NuevaPuntuacion")]
        public async Task<IActionResult> NuevaPuntuacion()
        {
            await numerosHub.Clients.All.SendAsync("NuevaPuntuacion");
            return Ok();
        }

        [HttpGet("Entrar/{id}")]
        public async Task<IActionResult> Entrar(string id)
        {
            var u = repository.GetByName(id);
            await numerosHub.Clients.All.SendAsync("Entrar", u);
            return Ok();
        }
    }
}
