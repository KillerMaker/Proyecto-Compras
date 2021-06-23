using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class DepartamentoController : Controller,IController
    {
        public ActionResult InsertarOpen() => View();
        public async Task<ActionResult> InsertarSend()
        {
            CDepartamento departamento = new CDepartamento
                 (
                    null,
                    Request.Form["Nombre"].ToString(),
                    int.Parse(Request.Form["Estado"].ToString())
                 );

            await departamento.Insert();
            return Redirect("https://localhost:44368/Departamento/SelectShow");
        }

        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }
        public async Task<ActionResult> ActualizarSend(int id)
        {
            CDepartamento departamento = new CDepartamento
                 (
                    id,
                    Request.Form["Nombre"].ToString(),
                    int.Parse(Request.Form["Estado"].ToString())
                 );

            await departamento.Update();
            return Redirect("https://localhost:44368/Departamento/SelectShow");
        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            CDepartamento departamento = null;

            foreach (CDepartamento depto in await CDepartamento.Select($"WHERE ID={id}"))
                departamento = depto;

            return View(departamento);
        }
        public async Task<ActionResult> EliminarSend(int id)
        {
            await CDepartamento.Delete(id);
            return Redirect("https://localhost:44368/Departamento/SelectShow");
        }
        public async Task<ActionResult> SelectShow() => View(await CDepartamento.Select());
    }
}
