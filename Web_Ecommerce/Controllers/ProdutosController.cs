using ApplicationApp.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Web_Ecommerce.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        
        public readonly InterfaceProductApp _InterfaceProductApp;

        public ProdutosController(InterfaceProductApp InterfaceProductApp, UserManager<ApplicationUser> userManager)
        {
            _InterfaceProductApp = InterfaceProductApp;
            _userManager = userManager;
        }

        // GET: ProdutosController
        public async Task<IActionResult> Index()
        {
            var userId = await RetornarIdUsuarioLogado();
            
            return View(await _InterfaceProductApp.ListarProdutosUsuario(userId));
        }

        // GET: ProdutosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await _InterfaceProductApp.GetEntityById(id));
        }

        // GET: ProdutosController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: ProdutosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            try
            {
                var userId = await RetornarIdUsuarioLogado();
                
                produto.UserId = userId;
                
                await _InterfaceProductApp.AddProduct(produto);

                if (produto.Notitycoes.Any())
                {
                    foreach (var item in produto.Notitycoes)
                    {
                        ModelState.AddModelError(item.NomePropriedade, item.mensagem);
                    }

                    return View("Create", produto);
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Erro interno: " + ex.Message);
                return View("Create", produto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ProdutosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _InterfaceProductApp.GetEntityById(id));
        }

        // POST: ProdutosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            try
            {
                await _InterfaceProductApp.UpdateProduct(produto);

                if (produto.Notitycoes.Any())
                {
                    foreach (var item in produto.Notitycoes)
                    {
                        ModelState.AddModelError(item.NomePropriedade, item.mensagem);
                    }

                    return View("Edit", produto);
                }

            }
            catch
            {
                return View("Edit", produto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ProdutosController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _InterfaceProductApp.GetEntityById(id));
        }

        // POST: ProdutosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Produto produto)
        {
            try
            {
                var product = await _InterfaceProductApp.GetEntityById(id);

                await _InterfaceProductApp.Delete(produto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<string> RetornarIdUsuarioLogado()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return user.Id;
        }
        
        [AllowAnonymous]
        [HttpGet("/api/ListarProdutosComEstoque")]
        public async Task<JsonResult> ListarProdutosComEstoque()
        {
            return Json(await _InterfaceProductApp.ListarProdutosComEstoque());
        }
    }
}
