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
        private static List<CUnidadMedida> lista;
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
                CUnidadMedida unidadMedida = new CUnidadMedida(id, Request.Form["Descripcion"], int.Parse(Request.Form["Estado"]));
                await unidadMedida.Update();
                return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/UnidadMedida/ErrorView");
            }
            
        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            try
            {
                ViewData["id"] = id;
                CUnidadMedida unidadMedida = null;

                foreach (CUnidadMedida um in await CUnidadMedida.Select($"WHERE U.ID={id}"))
                    unidadMedida = um;

                return View(unidadMedida);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/UnidadMedida/ErrorView");
            }

        }

        public async Task<ActionResult> EliminarSend(int id)
        {
            try
            {
                await CUnidadMedida.Delete(id);
                return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("https://localhost:44368/UnidadMedida/ErrorView");
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
                return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
            }
            catch (Exception e) 
            { 
                errorMessage = e.Message; return Redirect("https://localhost:44368/UnidadMedida/ErrorView"); 
            }
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            try
            {
                CUnidadMedida unidadMedida = new CUnidadMedida(null, Request.Form["Descripcion"], int.Parse(Request.Form["Estado"]));
                await unidadMedida.Insert();
                return Redirect("https://localhost:44368/UnidadMedida/SelectShow");
            }
            catch (Exception e) 
            { 
                errorMessage = e.Message; return Redirect("https://localhost:44368/UnidadMedida/ErrorView");
            }
        }

        public async Task<ActionResult> SelectShow()
        {
            try
            {
                lista = await CUnidadMedida.Select(query);
                return View(lista);
            }
            catch (Exception e) 
            {
                errorMessage = e.Message; return Redirect("https://localhost:44368/UnidadMedida/ErrorView");
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
                UnidadMedidaController.query = query;
                return RedirectToAction("SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message; return Redirect("https://localhost:44368/UnidadMedida/ErrorView"); 
            }
        }

    }
}
