﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SolicitudController : Controller,IController
    {
        public ActionResult ActualizarOpen(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> ActualizarSend(int id)
        {
            CSolicitud solicitud = new CSolicitud
                (
                    id,
                    int.Parse(Request.Form["Empleado"]),
                    Request.Form["Fecha"].ToString().Replace("/", "").Replace("-", ""),
                    int.Parse(Request.Form["Articulo"]),
                    int.Parse(Request.Form["Cantidad"]),
                    int.Parse(Request.Form["UnidadMedida"]),
                    int.Parse(Request.Form["Estado"])
                );

            await solicitud.Update();

            return RedirectToAction("SelectShow");
        }


        public async Task<ActionResult> EliminarOpen(int id)
        {
            ViewData["id"] = id;
            CSolicitud solicitud = null;

            foreach (CSolicitud soli in await CSolicitud.Select($"WHERE ID = {id}"))
                solicitud = soli;

            return View(solicitud);

        }
        public async Task<ActionResult> EliminarSend(int id)
        {
            await CSolicitud.Delete(id);

            return RedirectToAction("SelectShow");
        }


        public ActionResult InsertarOpen() => View();

        public async Task<ActionResult> InsertarSend()
        {
            CSolicitud solicitud = new CSolicitud
               (
                   null,
                   int.Parse(Request.Form["Empleado"]),
                   Request.Form["Fecha"].ToString().Replace("/", "").Replace("-", ""),
                   int.Parse(Request.Form["Articulo"]),
                   int.Parse(Request.Form["Cantidad"]),
                   int.Parse(Request.Form["UnidadMedida"]),
                   int.Parse(Request.Form["Estado"])
               );

            await solicitud.Insert();

            return RedirectToAction("SelectShow");
        }

        public async Task<ActionResult> SelectShow() => View(await CSolicitud.Select(null));
    }
}