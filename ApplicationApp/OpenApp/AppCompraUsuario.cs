using ApplicationApp.Interfaces;
using Domain.Interfaces.InterfaceCompraUsuario;
using Entities.Entities;

namespace ApplicationApp.OpenApp;

public class AppCompraUsuario : InterfaceCompraUsuarioApp
{
    private readonly ICompraUsuario _ICompraUsuario;
    
    public AppCompraUsuario(ICompraUsuario ICompraUsuario)
    {
        _ICompraUsuario = ICompraUsuario;
    }
    
    
    public async Task Add(CompraUsuario Objeto)
    {
        await _ICompraUsuario.Add(Objeto);
    }

    public async Task Update(CompraUsuario Objeto)
    {
        await _ICompraUsuario.Update(Objeto);
    }

    public async Task Delete(CompraUsuario Objeto)
    {
        await _ICompraUsuario.Delete(Objeto);
    }

    public async Task<CompraUsuario> GetEntityById(int Id)
    {
        return await _ICompraUsuario.GetEntityById(Id);
    }

    public async Task<List<CompraUsuario>> List()
    {
        return await _ICompraUsuario.List();
    }
}