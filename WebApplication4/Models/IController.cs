using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    interface IController
    {
        public ActionResult InsertarOpen();
        public Task<ActionResult> EliminarOpen(int id);
        public ActionResult ActualizarOpen(int id);
        public Task<ActionResult> InsertarSend();
        public Task<ActionResult> EliminarSend(int id);
        public Task<ActionResult> ActualizarSend(int id);
        public Task<ActionResult> SelectShow();
        public ActionResult SelectShowSearch();
        public ActionResult Exportar(IEnumerable<CEntidad>entidades);
    }
}
