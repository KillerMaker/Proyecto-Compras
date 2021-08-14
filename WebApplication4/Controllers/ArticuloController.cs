using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ArticuloController : Controller, IController
    {
        private static string query = "";
        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            CArticulo articulo = new CArticulo
                (
                    id,
                    Request.Form["Descripcion"],
                    int.Parse(Request.Form["Marca"]),
                    int.Parse(Request.Form["UnidadMedida"]),
                    int.Parse(Request.Form["Existencia"]),
                    int.Parse(Request.Form["Estado"])
                );

            await articulo.Update();
            return Redirect("https://localhost:44368/Articulo/SelectShow");

        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            CArticulo articulo = null;

            foreach (CArticulo art in await CArticulo.Select($"WHERE ID={id}"))
                articulo = art;

            return View(articulo);
        }

        public async Task<ActionResult> EliminarSend(int id)
        {
            await CArticulo.Delete(id);

            return Redirect("https://localhost:44368/Articulo/SelectShow");
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            CArticulo articulo = new CArticulo
                (
                    null,
                    Request.Form["Descripcion"],
                    int.Parse(Request.Form["Marca"]),
                    int.Parse(Request.Form["UnidadMedida"]),
                    int.Parse(Request.Form["Existencia"]),
                    int.Parse(Request.Form["Estado"])
                );

            await articulo.Insert();
            return Redirect("https://localhost:44368/Articulo/SelectShow");
        }

        public async Task<ActionResult> SelectShow()
        {
            return View(await CArticulo.Select(query));
        }
            
        public ActionResult SelectShowSearch()
        {
            string query=" WHERE ";
            query += Request.Form["Columnas"].ToString()+" ";
            query += Request.Form["Operadores"].ToString() + " ";
            query +="'"+ Request.Form["Criterio"].ToString()+ "';";
            ArticuloController.query = query;
            return RedirectToAction("SelectShow");
        }
    }
}
