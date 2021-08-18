using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class OrdenCompraController : Controller,IController
    {
        private static string query;
        private static List<COrdenCompra> lista;
        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            COrdenCompra ordenCompra = new COrdenCompra
                (
                    id,
                    int.Parse(Request.Form["Solicitud"]),
                    Request.Form["Fecha"].ToString().Replace("/", "").Replace("-", ""),
                    int.Parse(Request.Form["Articulo"]),
                    int.Parse(Request.Form["Cantidad"]),
                    int.Parse(Request.Form["UnidadMedida"]),
                    int.Parse(Request.Form["Marca"]),
                    int.Parse(Request.Form["CostoUnitario"]),
                    int.Parse(Request.Form["Estado"])
                );

            await ordenCompra.Update();

            return RedirectToAction("SelectShow");
        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            COrdenCompra ordenCompra = null;

            foreach (COrdenCompra OP in await COrdenCompra.Select($"WHERE ID = {id}"))
                ordenCompra = OP;

            return View(ordenCompra);
        }

        public async Task<ActionResult> EliminarSend(int id)
        {
            await COrdenCompra.Delete(id);

            return RedirectToAction("SelectShow");
        }

        public ActionResult Exportar(IEnumerable<CEntidad> entidades)
        {
            Excel e = new Excel();
            e.Write(lista);
            return Redirect("https://localhost:44368/OrdenCompra/SelectShow");
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            COrdenCompra ordenCompra = new COrdenCompra
                (
                    null,
                    int.Parse(Request.Form["Solicitud"]),
                    Request.Form["Fecha"].ToString().Replace("/", "").Replace("-", ""),
                    int.Parse(Request.Form["Articulo"]),
                    int.Parse(Request.Form["Cantidad"]),
                    int.Parse(Request.Form["UnidadMedida"]),
                    int.Parse(Request.Form["Marca"]),
                    int.Parse(Request.Form["CostoUnitario"]),
                    int.Parse(Request.Form["Estado"])
                );

            await ordenCompra.Insert();

            return RedirectToAction("SelectShow");
        }

        public async Task<ActionResult> SelectShow()
        {
            lista = await COrdenCompra.Select(query);
            return View(lista);
        }
        public ActionResult SelectShowSearch()
        {
            string query = " WHERE ";
            query += Request.Form["Columnas"].ToString() + " ";
            query += Request.Form["Operadores"].ToString() + " ";
            query += "'" + Request.Form["Criterio"].ToString() + "';";
            OrdenCompraController.query = query;
            return RedirectToAction("SelectShow");
        }
    }
}
