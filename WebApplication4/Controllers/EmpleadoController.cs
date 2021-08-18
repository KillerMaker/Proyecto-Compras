using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class EmpleadoController : Controller,IController
    {
        private static string query;
        private static List<CEmpleado> lista;
        private static string error="";

        public ActionResult InsertarOpen() => View();
        public async Task<ActionResult>InsertarSend()
        {
            CEmpleado empleado = new CEmpleado(
                null,
                Request.Form["Cedula"].ToString(),
                Request.Form["Nombre"].ToString(),
                int.Parse(Request.Form["Departamento"].ToString()),
                int.Parse(Request.Form["Estado"].ToString()));

            await empleado.Insert();
            return Redirect("https://localhost:44368/Empleado/SelectShow");
        }

        public ActionResult ActualizarOpen(int id) 
        {
            ViewData["id"] = id;
            return View();
        }
        public async Task<ActionResult>ActualizarSend(int id)
        {
            if (Request.Form["Cedula"].ToString().validaCedula())
            {
                CEmpleado empleado = new CEmpleado(
               //int.Parse(Request.Form["id"].ToString()),
               id,
               Request.Form["Cedula"].ToString(),
               Request.Form["Nombre"].ToString(),
               int.Parse(Request.Form["Departamento"].ToString()),
               int.Parse(Request.Form["Estado"].ToString()));

                await empleado.Update();
                return Redirect("https://localhost:44368/Empleado/SelectShow");
            }

            return Redirect("https://localhost:44368/Empleado/ErrorView");
        }

        public async Task<ActionResult> EliminarOpen(int id) 
        {
            ViewData["id"] = id;
            CEmpleado empleado = null;

            foreach (CEmpleado emp in await CEmpleado.Select($"WHERE ID={id}"))
                empleado = emp;

            return View(empleado);
        }
        public async Task<ActionResult>EliminarSend(int id)
        {
            await CEmpleado.Delete(id);
            return Redirect("https://localhost:44368/Empleado/SelectShow");
        }
        public async Task<ActionResult> SelectShow()
        {
            lista = await CEmpleado.Select(query);
            return View(lista);
        }
        public ActionResult SelectShowSearch()
        {
            string query = " WHERE ";
            query += Request.Form["Columnas"].ToString() + " ";
            query += Request.Form["Operadores"].ToString() + " ";
            query += "'" + Request.Form["Criterio"].ToString() + "';";
            EmpleadoController.query = query;
            return RedirectToAction("SelectShow");
        }

        public ActionResult Exportar(IEnumerable<CEntidad> entidades)
        {
            Excel e = new Excel();
            e.Write(lista);
            return Redirect("https://localhost:44368/Empleado/SelectShow");
        }

        public ActionResult ErrorView()
        {
            throw new NotImplementedException();
        }
    }
}
