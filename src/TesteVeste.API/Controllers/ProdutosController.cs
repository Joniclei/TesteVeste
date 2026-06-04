using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TesteVeste.Application.DTOs;
using TesteVeste.Application.Interfaces;
using TesteVeste.Domain.Shared;

namespace TesteVeste.API.Controllers;

// =============================================================================
//  TODO — SUA TAREFA
// =============================================================================
//  Implemente os endpoints do CRUD de Produtos abaixo.
//  Consulte o CategoriasController.cs como referência de implementação.
//
//  ENDPOINTS A IMPLEMENTAR:
//  GET    /api/produtos?pagina=1&tamanhoPagina=10  → lista paginada de produtos
//  GET    /api/produtos/{id}                       → produto por Id (404 se não encontrado)
//  POST   /api/produtos                            → criar produto (201 Created em sucesso)
//  PUT    /api/produtos/{id}                       → atualizar produto (400 se inválido)
//  DELETE /api/produtos/{id}                       → desativar produto (204 No Content)
//
//  DICAS DE HTTP STATUS:
//  - Ok(result)        → 200
//  - Created(...)      → 201  ex: CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
//  - NoContent()       → 204
//  - BadRequest(result)→ 400
//  - NotFound(result)  → 404
// =============================================================================

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(CommandResult<PagedResult<ProdutoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
    {
        var result = await _service.GetAllAsync(pagina, tamanhoPagina);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CommandResult<ProdutoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommandResult<ProdutoDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProdutoDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CommandResult<ProdutoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProdutoDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.Succeeded)
            return result.Messages.Any(m => m.Contains("não encontrado"))
                ? NotFound(result)
                : BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.Succeeded)
            return NotFound(result);

        return NoContent();
    }
}
