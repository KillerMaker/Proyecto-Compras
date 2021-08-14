using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class UnidadMedidaController : Controller, IController
    {
        private static string query;
        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            CUnidadMedida unidadMedida = new CUnidadMedida(id,Request.Form["Descripcion"], int.Parse(Request.Form["Estado"]));
            await unidadMedida.Update();
            return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            CUnidadMedida unidadMedida = null;

            foreach (CUnidadMedida um in await CUnidadMedida.Select($"WHERE U.ID={id}")) 
                unidadMedida = um;

            return View(unidadMedida);
        }

        public async Task<ActionResult> EliminarSend(int id)
        {
            await CUnidadMedida.Delete(id);
            return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            CUnidadMedida unidadMedida = new CUnidadMedida(null, Request.Form["Descripcion"], int.Parse(Request.Form["Estado"]));
            await unidadMedida.Insert();
            return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
        }

        public async Task<ActionResult> SelectShow() => View(await CUnidadMedida.Select(query));

        public ActionResult SelectShowSearch()
        {
            string query = " WHERE ";
            query += Request.Form["Columnas"].ToString() + " ";
            query += Request.Form["Operadores"].ToString() + " ";
            query += "'" + Request.Form["Criterio"].ToString() + "';";
            UnidadMedidaController.query = query;
            return RedirectToAction("SelectShow");
        }

    }
}
