using AcreditacionesApp.Api.Data;
using AcreditacionesApp.Api.Dtos;
using AcreditacionesApp.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcreditacionesApp.Api.Controllers;

[ApiController]                 // activa comportamientos de API (ej. validaci[on autom[atica del body)
[Route("api/[controller]")]     // URL: api/Acreditaciones ([controller] = nombre sin "Controller")
public class AcreditacionesController : ControllerBase   // ControllerBase = controlador sin vistas HTML
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

        _db.Acreditaciones.Add(acreditacion);   // marca como "nuevo" (a[un no toca la DB)
        await _db.SaveChangesAsync();           // AQU[I se ejecuta el INSERT

        return CreatedAtAction(nameof(ObtenerPorId), new { id = acreditacion.Id }, acreditacion);
    }

    // GET api/acreditaciones
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var listar = await _db.Acreditaciones
            .Include(a => a.Tipo)                       // JOIN con tipo_acreditacion
            .OrderByDescending(a => a.FechaEmision)
            .ToListAsync();                             // aqu[i se ejecuta el SELECT
        return Ok(listar);                              // 200 + JSON
    }

    // GET api/acreditaciones/5
    [HttpGet("{id:int}")]                                   // {id:int} = parte variable, solo enteros
    [ProducesResponseType(typeof(Acreditacion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var a = await _db.Acreditaciones
            .Include(x => x.Tipo)
            .Include(x => x.Requisitos)                     // trae tambi[en los requisitos
            .FirstOrDefaultAsync(x => x.Id == id);          // el primero o null
        return a is null ? NotFound() : Ok(a);
    }

    // POST api/acreditaciones/5/requisitos/12/cumplir
    [HttpPost("{id:int}/requisitos/{requisitoId:int}/cumplir")]
    public async Task<IActionResult> CumplirRequisito(int id, int requisitoId)
    {
        var requisito = await _db.Requisitos
            .FirstOrDefaultAsync(r => r.Id == requisitoId && r.AcreditacionId == id);
        if (requisito is null) return NotFound();

        if (requisito.Cumplido)
            return BadRequest("El requisito ya est[a cumplido.");

        requisito.Cumplido = true;                                      // EF detecta el cambio
        requisito.FechaCumplimiento = DateOnly.FromDateTime(DateTime.UtcNow);
        await _db.SaveChangesAsync();                                   // genera el UPDATE
        return NoContent();                                             // 204: [exito sin cuerpo
    }

    // POST api/acreditaciones/5/marcar-vigente
    [HttpPost("{id:int}/marca-vigente")]
    public async Task<IActionResult> MarcaVigente(int id)
    {
        var acreditacionMarcaVigente = await _db.Acreditaciones
            .Include(x => x.Requisitos)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (acreditacionMarcaVigente is null) return NotFound();

        // Regla de negocio 1: solo desde Pendiente o EnRevision.
        if (acreditacionMarcaVigente.Estado is not (EstadoAcreditacion.Pendiente or EstadoAcreditacion.EnRevision))
            return BadRequest("Solo se puede marcar vigente desde Pendiende o EnRevision.");

        // Regla de nogocio 2: Todos los requistiso obligatorios deben estar cumplidos.
        if (acreditacionMarcaVigente.Requisitos.Any(r => r.Obligatorio && !r.Cumplido))
            return BadRequest("Faltan requisitos obligatorios por cumplir.");

        acreditacionMarcaVigente.Estado = EstadoAcreditacion.Vigente;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Change Tipo de acreditaci[on para la acreditaci[on 
    // POST api/acreditaciones/5/PostChangeTipo
    [HttpPost("{id:int}/PostChangeTipo")] // Decorador
    public async Task<IActionResult> ChangeTipoOfAcreditacion(int id)
    {
        var acreditacion = await _db.Acreditaciones
            .FindAsync(id);
        if (acreditacion is null)
            return NotFound();

        acreditacion.Estado = EstadoAcreditacion.Pendiente;
        await _db.SaveChangesAsync();
        return Ok(acreditacion);
    }
}
