using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AseguradoraFL.Web.Models
{
    public class FactoresCalculador
    {
        public double EstadoActual { get; set; }
        public double HistorialClinico { get; set; }
        public double EstiloVida { get; set; }
        public double Ocupacion { get; set; }
    }
}