using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class EmpleadoController : Controller
    {
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

        public ActionResult EliminarOpen(int id) 
        {
            ViewData["id"] = id;
            return View();
        }
        public async Task<ActionResult>EliminarSend(int id)
        {
            await CEmpleado.Delete(id);
            return Redirect("https://localhost:44368/Empleado/SelectShow");
        }
        public async Task<ActionResult> SelectShow() => View(await CEmpleado.Select());
        
    }
}
