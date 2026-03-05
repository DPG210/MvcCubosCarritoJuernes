using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MvcCubosCarritoJuernes.Extensions;
using MvcCubosCarritoJuernes.Helper;
using MvcCubosCarritoJuernes.Models;
using MvcCubosCarritoJuernes.Repositories;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace MvcCubosCarritoJuernes.Controllers
{
    public class CubosController : Controller
    {
        private IMemoryCache memoryCache;
        private HelperPathProvider helper;
        private RepositoryCubos repo;
        public CubosController(IMemoryCache memoryCache,RepositoryCubos repo, HelperPathProvider helper)
        {
            this.memoryCache = memoryCache;
            this.repo = repo;
            this.helper = helper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Cubos(int? idfavorito, int? idcubo)
        {
            if (idfavorito != null)
            {
                List<Cubo> cubosFavoritos;
                if (this.memoryCache.Get("FAVORITOS") == null)
                {
                    cubosFavoritos = new List<Cubo>();
                }
                else
                {
                    cubosFavoritos = this.memoryCache.Get<List<Cubo>>("FAVORITOS");
                }
                Cubo favorito = await this.repo.FindCubo(idfavorito.Value);
                cubosFavoritos.Add(favorito);
                this.memoryCache.Set("FAVORITOS", cubosFavoritos);

            }
            if (idcubo != null)
            {
                List<int> idsCubos;
                if(HttpContext.Session.GetObject<List<int>>
                    ("IDSCUBOS") != null)
                {
                    idsCubos = HttpContext.Session.GetObject<List<int>>("IDSCUBOS");
                }
                else
                {
                    idsCubos = new List<int>();
                }
                idsCubos.Add(idcubo.Value);
                HttpContext.Session.SetObject("IDSCUBOS", idsCubos);
            }
            List < Cubo > cubos= await this.repo.GetCubosAsync();
            return View(cubos);
        }
        public async Task<IActionResult> DetallesCubos(int idcubo)
        {
            Cubo cubo = await this.repo.FindCubo(idcubo);
            return View(cubo);
        }
        public async Task<IActionResult> CreateCubo()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCubo(Cubo cubo, IFormFile imagen)
        {
            if(imagen != null)
            {
                string fileName = imagen.FileName;
                string path = this.helper.MapPath(fileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }
                cubo.Imagen = fileName;
            }
            await this.repo.CreateCubo(cubo.IdCubo, cubo.Nombre, cubo.Modelo, cubo.Marca, cubo.Imagen, cubo.Precio);
            return RedirectToAction("Cubos");
        }
        public async Task<IActionResult> UpdateCubo(int idcubo)
        {
            Cubo cubo = await this.repo.FindCubo(idcubo);
            return View(cubo);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCubo(Cubo cubo)
        {
            await this.repo.UpdateCubo(cubo.IdCubo, cubo.Nombre, cubo.Modelo, cubo.Marca, cubo.Imagen, cubo.Precio);
            return RedirectToAction("Cubos");
        }
        public async Task<IActionResult> CubosFavoritos(int? ideliminar)
        {
            if(ideliminar != null)
            {
                List<Cubo> cubosFavoritos =
                    this.memoryCache.Get<List<Cubo>>("FAVORITOS");
                Cubo delete =
                    cubosFavoritos.Find(z => z.IdCubo == ideliminar.Value);
                cubosFavoritos.Remove(delete);
                if(cubosFavoritos.Count == 0)
                {
                    this.memoryCache.Remove("FAVORITOS");
                }
                else
                {
                    this.memoryCache.Set("FAVORITOS", cubosFavoritos);
                }
            }
            return View();
        }
        public async Task <IActionResult> CarritoCompra(int? ideliminar)
        {

        }
    }
}
