using Domain.Interfaces.InterfaceProduct;
using Entities.Entities;
using Infrastructure.Repository.Generics;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Entities.Entities.Enums;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryProduct : RepositoryGenerics<Produto>, IProduct
    {
        
        private readonly DbContextOptions<ContextBase> _optionsBuilder;
        
        public RepositoryProduct(DbContextOptions<ContextBase> optionsBuilder)
        {
            _optionsBuilder = optionsBuilder;
        }
        
        public async Task<List<Produto>> ListarProdutosUsuario(string userId)
        {
            using (var banco = new ContextBase(_optionsBuilder))
            {
                return await banco.Produtos.Where(p => p.UserId == userId).AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<Produto>> ListarProdutos(Expression<Func<Produto, bool>> exProduto)
        {
            using (var banco = new ContextBase(_optionsBuilder))
            {
                return await banco.Produtos.Where(exProduto).AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<Produto>> ListarProdutosCarrinhoUsuario(string userId)
        {
            using (var banco = new ContextBase(_optionsBuilder))
            {
                var produtosCarrinhoUsuario = (from p in banco.Produtos
                    join c in banco.CompraUsuarios on p.Id equals c.IdProduto
                    where c.UserId.Equals(userId) && c.Estado == EstadoCompra.Produto_Carrinho
                    select new Produto
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Observacao =  p.Observacao,
                        Valor =  p.Valor,
                        QtdCompra = c.QtdCompra,
                        IdProdutoCarrinho = c.Id
                    }).AsNoTracking().ToListAsync();

                return await produtosCarrinhoUsuario;
            }
        }

        public async Task<Produto> ObterProdutoCarrinho(int idProdutoCarrinho)
        {
            using (var banco = new ContextBase(_optionsBuilder))
            {
                var produtosCarrinhoUsuario = (from p in banco.Produtos
                    join c in banco.CompraUsuarios on p.Id equals c.IdProduto
                    where c.Id.Equals(idProdutoCarrinho) && c.Estado == EstadoCompra.Produto_Carrinho
                    select new Produto
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Observacao =  p.Observacao,
                        Valor =  p.Valor,
                        QtdCompra = c.QtdCompra,
                        IdProdutoCarrinho = c.Id
                    }).AsNoTracking().FirstOrDefaultAsync();

                return await produtosCarrinhoUsuario;
            }
        }
    }
}
