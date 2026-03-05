using Microsoft.EntityFrameworkCore;
using MvcCubosCarritoJuernes.Data;
using MvcCubosCarritoJuernes.Models;
using MySql.Data.MySqlClient;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;

namespace MvcCubosCarritoJuernes.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;
        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }
        public async Task<List<Cubo>> GetCubosAsync()
        {
            var consulta= from datos in this.context.Cubos
                          select datos;
            return await consulta.ToListAsync();
        }
        public async Task<Cubo> FindCubo(int idCubo)
        {
            var consulta= from datos in this.context.Cubos
                          where datos.IdCubo == idCubo
                          select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task CreateCubo(int idCubo, string nombre, string modelo, string marca, string imagen, int precio)
        {
            string sql = "insert into cubos values (@idcubo,@nombre,@modelo,@marca,@imagen,@precio)";
            MySqlParameter pamId = new MySqlParameter("@idcubo", idCubo);
            MySqlParameter pamNom = new MySqlParameter("@nombre", nombre);
            MySqlParameter pamMod = new MySqlParameter("@modelo", modelo);
            MySqlParameter pamMarca = new MySqlParameter("@marca", marca);
            MySqlParameter pamImagen = new MySqlParameter("@imagen", imagen);
            MySqlParameter pamPrecio = new MySqlParameter("@precio", precio);

            await this.context.Database.ExecuteSqlRawAsync(sql, pamId, pamNom, pamMod, pamMarca, pamImagen, pamPrecio);
        }
        public async  Task UpdateCubo(int idCubo, string nombre, string modelo, string marca, string imagen, int precio)
        {
            string sql = "update  cubos set nombre=@nombre, modelo=@modelo,marca=@marca,precio=@precio where @idcubo=@idcubo";
            MySqlParameter pamId = new MySqlParameter("@idcubo", idCubo);
            MySqlParameter pamNom = new MySqlParameter("@nombre", nombre);
            MySqlParameter pamMod = new MySqlParameter("@modelo", modelo);
            MySqlParameter pamMarca = new MySqlParameter("@marca", marca);
            MySqlParameter pamPrecio = new MySqlParameter("@precio", precio);

            await this.context.Database.ExecuteSqlRawAsync(sql, pamId, pamNom, pamMod, pamMarca, pamPrecio);
        }
        public async Task<List<Cubo>> GetCubosSessionAsync(List<int> idsCubos)
        {
            var consulta = from datos in this.context.Cubos
                           where idsCubos.Contains(datos.IdCubo)
                           select datos;
            var cubosDb= await consulta.ToListAsync();
            List<Cubo> listaCubos = new List<Cubo>();
            foreach(int id in idsCubos)
            {
                Cubo c = cubosDb.FirstOrDefault(x => x.IdCubo == id);
                if (c != null)
                {
                    listaCubos.Add(c);
                }
            }
            return listaCubos;
        }
        public int GetMaxId()
        {
            var consulta= from datos in this.context.Compras
                          select datos;
            if (this.context.Compras.Any())
            {
                return this.context.Compras.Max(z => z.Id_compra);
            }
            else
            {
                return 0; 
            }
        }
        public async Task InsertCompra(int idCubo, int cantidad, int precio)
        {
            Compra compra = new Compra();
            compra.Id_compra = GetMaxId()+1;
            compra.Id_cubo = idCubo;
            compra.Precio = precio;
            compra.Cantidad = cantidad;
            compra.Fechapedido = DateTime.Now;
            this.context.Compras.Add(compra);
            await this.context.SaveChangesAsync();
        }
    }
}
