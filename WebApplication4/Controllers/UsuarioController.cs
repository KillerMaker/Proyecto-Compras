
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class UsuarioController : Controller, IController
    {
        private static string query;
        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            Usuario usuario = new Usuario
                (
                    id,
                    Request.Form["Nombre"],
                    Request.Form["Clave"],
                    int.Parse(Request.Form["TipoUsuario"].ToString()),
                    int.Parse(Request.Form["Estado"].ToString())
                );

            await usuario.Update();
            return Redirect("https://localhost:44368/Usuario/SelectShow");
        }

        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            Usuario usuario = null;

            foreach (Usuario usr in await Usuario.Select($"WHERE U.ID ={id}"))
                usuario = usr;

            return View(usuario);
        }

        public async Task<ActionResult> EliminarSend(int id)
        {
            await Usuario.Delete(id);

            return Redirect("https://localhost:44368/Usuario/SelectShow");
        }

        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            string n = Request.Form["nombre"].ToString();
            string c = Request.Form["clave"].ToString();
            Usuario usuario = new Usuario
               (
                   null,
                   Request.Form["nombre"].ToString(),
                   Request.Form["clave"].ToString(),
                   1,
                   1
               );

            if(usuario.clave==Request.Form["ConfirmarClave"])
            {
                await usuario.Insert();
                return RedirectToAction("LoginOpen");
            }
            else
            {
              //return Content(" < script language = 'javascript' type = 'text/javascript' > alert('Claves no coinciden');</ script > ");
                return Redirect("https://localhost:44368/Usuario/InsertarOpen");
            }
        }

        public async Task<ActionResult> SelectShow() => View(await Usuario.Select(query));

        public ActionResult SelectShowSearch()
        {
            string query = " WHERE ";
            query += Request.Form["Columnas"].ToString() + " ";
            query += Request.Form["Operadores"].ToString() + " ";
            query += "'" + Request.Form["Criterio"].ToString() + "';";
            UsuarioController.query = query;
            return RedirectToAction("SelectShow");
        }

        public ActionResult LoginOpen() 
        {
            HttpContext.Session.SetString("nombre", "");
            HttpContext.Session.SetInt32("tipoUsuario", 0);

            return View();
        }
        public async Task<ActionResult>LoginSend()
        {
            string nombreUsuario = Request.Form["NombreUsuario"];
            string clave = Request.Form["Clave"];

            Usuario usuario =await Usuario.Login(nombreUsuario, clave);
            if (usuario != null)
            {
                HttpContext.Session.SetString("nombre", usuario.nombre);
                HttpContext.Session.SetInt32("tipoUsuario", usuario.tipoUsuario);

                return Redirect("https://localhost:44368/Home/Index");

            }
            else
                return Redirect("https://localhost:44368/Usuario/LoginOpen");
        }

    }
}

