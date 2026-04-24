using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.InterfaceServices
{
    public interface IServiceProduct
    {
        Task AddProduct(Produto produto);
        Task UpdateProduct(Produto produto);
    }
}
