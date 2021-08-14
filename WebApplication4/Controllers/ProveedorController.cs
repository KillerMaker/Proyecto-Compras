using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ProveedorController : Controller,IController
    {
        private static string query;
        public ActionResult InsertarOpen() => View();
        public async Task<ActionResult> InsertarSend()
        {
            CProveedor proveedor = new CProveedor(
                null,
                Request.Form["Cedula"].ToString(),
                Request.Form["NombreComercial"].ToString(),
                int.Parse(Request.Form["Estado"].ToString()));

            await proveedor.Insert();
            return Redirect("https://localhost:44368/Proveedor/SelectShow");
        }

        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }
        public async Task<ActionResult> ActualizarSend(int id)
        {
            CProveedor proveedor = new CProveedor(
               id,
               Request.Form["Cedula"].ToString(),
               Request.Form["NombreComercial"].ToString(),
               int.Parse(Request.Form["Estado"].ToString()));

            await proveedor.Update();
            return Redirect("https://localhost:44368/Proveedor/SelectShow");
        }

        public async  Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            CProveedor proveedor = null;

            foreach (CProveedor prov in await CProveedor.Select($"WHERE P.ID={id}"))
                proveedor = prov;

            return View(proveedor);
        }
        public async Task<ActionResult> EliminarSend(int id)
        {
            await CProveedor.Delete(id);
            return Redirect("https://localhost:44368/Proveedor/SelectShow");
        }
        public async Task<ActionResult> SelectShow() => View(await CProveedor.Select(query));

        public ActionResult SelectShowSearch()
        {
            string query = " WHERE ";
            query += Request.Form["Columnas"].ToString() + " ";
            query += Request.Form["Operadores"].ToString() + " ";
            query += "'" + Request.Form["Criterio"].ToString() + "';";
            ProveedorController.query = query;
            return RedirectToAction("SelectShow");
        }
    }
}
