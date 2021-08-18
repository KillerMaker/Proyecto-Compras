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
        private static List<CArticulo> lista;
        private static string errorMessage;

        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            try
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
            catch(Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }

        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            try
            {
                ViewData["id"] = id;
                CArticulo articulo = null;

                foreach (CArticulo art in await CArticulo.Select($"WHERE ID={id}"))
                    articulo = art;

                return View(articulo);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }
        }

        public async Task<ActionResult> EliminarSend(int id)
        {
            try
            {
                await CArticulo.Delete(id);

                return Redirect("https://localhost:44368/Articulo/SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }
        }

        public ActionResult ErrorView()
        {
            return View(errorMessage);
        }

        public ActionResult Exportar(IEnumerable<CEntidad> entidades)
        {
            try
            {
                Excel e = new Excel();
                e.Write(lista);
                return Redirect("https://localhost:44368/Articulo/SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            try
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
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }
           
        }

        public async Task<ActionResult> SelectShow()
        {
            try
            {
                lista = await CArticulo.Select(query);
                return View(lista);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }

        }
            
        public ActionResult SelectShowSearch()
        {
            try
            {
                string query = " WHERE ";
                query += Request.Form["Columnas"].ToString() + " ";
                query += Request.Form["Operadores"].ToString() + " ";
                query += "'" + Request.Form["Criterio"].ToString() + "';";
                ArticuloController.query = query;
                return RedirectToAction("SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/Articulo/ErrorView");
            }

        }
    }
}
