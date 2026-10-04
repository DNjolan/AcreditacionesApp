using AcreditacionesApp.Api.Data;
using AcreditacionesApp.Api.Dtos;
using AcreditacionesApp.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcreditacionesApp.Api.Controllers;

[ApiController]                 // activa comportamientos de API (ej. validaci[on autom[atica del body)
[Route("api/[controller]")]     // URL: api/Acreditaciones ([controller] = nombre sin "Controller")
public class AcreditacionesController: ControllerBase   // ControllerBase = controlador sin vistas HTML
{
    private readonly AppDbContext _db;

    // ASP.NET ve este constructor y nos entrega un AppDbContext (eso es la DI).
    public AcreditacionesController(AppDbContext db) => _db = db;

    // POST api/acreditaciones
    [HttpPost]
    [ProducesResponseType(typeof(Acreditacion), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(CrearAcreditacionDto dto)
    {
        // Regla 1: el tipo debe existir (lo necesitamos para calcular el vencimiento)
        var tipo = await _db.TiposAcreditacion.FindAsync(dto.TipoAcreditacionId);
        if (tipo is null)
            return BadRequest("El tipo de acreditaci[on no existe.");

        // Regla 2: el nombre no puede venir vac[io
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio.");

        var acreditacion = new Acreditacion
        {
            TipoAcreditacionId = tipo.Id,
            Nombre = dto.Nombre.Trim(),
            EntidadAcreditadora = dto.EntidadAcreditadora.Trim(),
            FechaEmision = dto.FechaEmision,
            FechaVencimiento = dto.FechaEmision.AddMonths(tipo.VigenciaMeses),  // regla de negocio
            Estado = EstadoAcreditacion.Pendiente
        };

        _db.Acreditaciones.Add(acreditacion);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = acreditacion.Id }, acreditacion);
    }

    // GET api/acreditaciones
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var listar = await _db.Acreditaciones
            .Include(a => a.Tipo)
            .OrderByDescending(a => a.FechaEmision)
            .ToListAsync();
        return Ok(listar);
    }

    // GET api/acreditaciones/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Acreditacion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var a = await _db.Acreditaciones
            .Include(x => x.Tipo)
            .Include(x => x.Requisitos)
            .FirstOrDefaultAsync(x => x.Id == id);
        return a is null ? NotFound() : Ok(a);
    }
}
