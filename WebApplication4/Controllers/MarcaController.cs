using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class MarcaController : Controller,IController
    {
        private static string query;
        private static List<CMarca> lista;
        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            CMarca marca = new CMarca(id, Request.Form["Descripcion"], int.Parse(Request.Form["estado"]));
            await marca.Update();
            return Redirect("https://localhost:44368/Marca/SelectShow");
        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            CMarca marca = null;

            foreach (CMarca mrc in await CMarca.Select($"WHERE M.ID={id}"))
                marca = mrc;
            
            return View(marca);
        }

        public async Task<ActionResult> EliminarSend(int id)
        {
           await CMarca.Delete(id);
            return Redirect("https://localhost:44368/Marca/SelectShow");
        }

        public ActionResult ErrorView()
        {
            throw new NotImplementedException();
        }

        public ActionResult Exportar(IEnumerable<CEntidad> entidades)
        {
            Excel e = new Excel();
            e.Write(lista);
            return Redirect("https://localhost:44368/Marca/SelectShow");
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            CMarca marca = new CMarca(null,Request.Form["Descripcion"], int.Parse(Request.Form["estado"]));
            await marca.Insert();
            return Redirect("https://localhost:44368/Marca/SelectShow");
        }

        public async Task<ActionResult> SelectShow()
        {
            lista = await CMarca.Select(query);
            return View(lista);
        }
        public ActionResult SelectShowSearch()
        {
            string query = " WHERE ";
            query += Request.Form["Columnas"].ToString() + " ";
            query += Request.Form["Operadores"].ToString() + " ";
            query += "'" + Request.Form["Criterio"].ToString() + "';";
            MarcaController.query = query;
            return RedirectToAction("SelectShow");
        }
    }
}
