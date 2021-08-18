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
        private static List<CProveedor> lista;
        private static string errorMessage;

        public ActionResult InsertarOpen() => View();
        public async Task<ActionResult> InsertarSend()
        {
            try
            {
                CProveedor proveedor = new CProveedor(null, Request.Form["Cedula"].ToString(), Request.Form["NombreComercial"].ToString(), int.Parse(Request.Form["Estado"].ToString()));
                await proveedor.Insert();
                return Redirect("https://localhost:44368/Proveedor/SelectShow");
                
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("ErrorView");
            }
            
        }

        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }
        public async Task<ActionResult> ActualizarSend(int id)
        {
            try
            {
                CProveedor proveedor = new CProveedor(id, Request.Form["Cedula"].ToString(), Request.Form["NombreComercial"].ToString(), int.Parse(Request.Form["Estado"].ToString()));
                await proveedor.Update();
                return Redirect("https://localhost:44368/Proveedor/SelectShow");
               
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("ErrorView");
            }

        }

        public async  Task<ActionResult> EliminarOpen(int id)
        {
            try
            {
                ViewData["id"] = id;
                CProveedor proveedor = null;
                foreach (CProveedor prov in await CProveedor.Select($"WHERE P.ID={id}"))
                    proveedor = prov;
                return View(proveedor);
                
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("ErrorView");
            }


        }
        public async Task<ActionResult> EliminarSend(int id)
        {
            try
            {
                await CProveedor.Delete(id);
                return Redirect("https://localhost:44368/Proveedor/SelectShow");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return Redirect("ErrorView");
            }

        }
        public async Task<ActionResult> SelectShow()
        {
            try
            {
                lista = await CProveedor.Select(query);
                return View(lista);
            }
            catch (Exception e) { errorMessage = e.Message; return Redirect("ErrorView"); }
        }

        public ActionResult SelectShowSearch()
        {
            try
            {
                string query = " WHERE ";
                query += Request.Form["Columnas"].ToString() + " ";
                query += Request.Form["Operadores"].ToString() + " ";
                query += "'" + Request.Form["Criterio"].ToString() + "';";
                ProveedorController.query = query;
                return RedirectToAction("SelectShow");
            }
            catch (Exception e) { errorMessage = e.Message; return Redirect("ErrorView"); }
        }

        public ActionResult Exportar(IEnumerable<CEntidad> entidades)
        {
            try
            {
                Excel e = new Excel();
                e.Write(lista);
                return Redirect("https://localhost:44368/Proveedor/SelectShow");
            }
            catch (Exception e) { errorMessage = e.Message; return Redirect("ErrorView"); }
        }

        public ActionResult ErrorView()
        {
            throw new NotImplementedException();
        }
    }
}
