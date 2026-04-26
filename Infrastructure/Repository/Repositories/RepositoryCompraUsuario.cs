using Domain.Interfaces.InterfaceCompraUsuario;
using Entities.Entities;
using Entities.Entities.Enums;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repositories;

public class RepositoryCompraUsuario : RepositoryGenerics<CompraUsuario>, ICompraUsuario
{
    private readonly DbContextOptions<ContextBase> _optionsBuilder;
    
    public RepositoryCompraUsuario(DbContextOptions<ContextBase> optionsBuilder)
    {
        _optionsBuilder = optionsBuilder;
    }
    
    public async Task<int> QuantidadeProdutoCarrinhoUsuario(string userId)
    {
        using (var banco = new ContextBase(_optionsBuilder))
        {
            return await banco.CompraUsuarios.CountAsync(c => c.UserId == userId && c.Estado == EstadoCompra.Produto_Carrinho);
        }
    }
}