using Microsoft.EntityFrameworkCore;
using TesteVeste.Domain.Entities;
using TesteVeste.Domain.Interfaces;
using TesteVeste.Domain.Shared;
using TesteVeste.Infrastructure.Data;

namespace TesteVeste.Infrastructure.Repositories;

// =============================================================================
//  TODO — SUA TAREFA
// =============================================================================
//  Implemente todos os métodos deste repositório usando Entity Framework Core.
//  Consulte o CategoriaRepository.cs como referência de implementação.
//
//  DICAS:
//  - Use _context.Produtos para acessar a tabela de produtos.
//  - Use .Include(p => p.Categoria) para carregar a categoria junto com o produto.
//  - Use .AsNoTracking() em consultas de leitura.
//  - Use Skip/Take para paginação: Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina)
//  - Para ExistsWithNameAsync, compare nomes ignorando maiúsculas/minúsculas.
// =============================================================================

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Produto>> GetAllAsync(int pagina, int tamanhoPagina)
    {
        var query = _context.Produtos
            .Include(p => p.Categoria)
            .AsNoTracking();

        var totalItens = await query.CountAsync();

        var itens = await query
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return new PagedResult<Produto>
        {
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = totalItens,
            Itens = itens
        };
    }

    public async Task<Produto?> GetByIdAsync(int id)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsWithNameAsync(string nome, int? excludeId = null)
    {
        var query = _context.Produtos
            .Where(p => p.Nome.ToLower() == nome.ToLower());

        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task AddAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
    }

    public void Update(Produto produto)
    {
        _context.Produtos.Update(produto);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
