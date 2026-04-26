using Domain.Interfaces.Generics;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces.InterfaceProduct
{
    public interface IProduct : IGenerics<Produto>
    {
        Task<List<Produto>> ListarProdutosUsuario(string userId);
        
        Task<List<Produto>> ListarProdutos(Expression<Func<Produto, bool>> exProduto);
    }
}
