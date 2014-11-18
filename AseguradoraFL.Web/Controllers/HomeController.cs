using AseguradoraFL.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AseguradoraFL.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Factors() {
            return File("~/Scripts/factors.json", "text/json");
        }

        public ActionResult CalcularPoliza(FactoresCalculador factores){
            var result =  CalculadorPoliza.CalcularPoliza(factores);
            return Json(result);
        }
    }   
}
