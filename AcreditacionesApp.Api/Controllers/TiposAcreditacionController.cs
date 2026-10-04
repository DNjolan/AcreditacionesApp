using AcreditacionesApp.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcreditacionesApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiposAcreditacionController: ControllerBase
{
    private readonly AppDbContext _db;
    public TiposAcreditacionController(AppDbContext db) => _db = db;

    // Listar todos los tipos de acreditaci[on
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var tipos = await _db.TiposAcreditacion
            .OrderBy(t => t.Nombre)
            .ToListAsync();
        return Ok(tipos);
    }

    // Obtener el tipo de acreditaci[on por id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ListarById(int id)
    {
        var tipoById = await _db.TiposAcreditacion
            .FindAsync(id);

        return tipoById is null ? NotFound() : Ok(tipoById);
    }
}
