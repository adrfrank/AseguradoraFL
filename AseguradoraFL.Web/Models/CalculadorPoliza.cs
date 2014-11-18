using FLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AseguradoraFL.Web.Models
{
    public static class CalculadorPoliza
    {
        public static double CalcularPoliza(double inputEnfermedadesActuales, double inputEnfermedadesHistoricas, double inputEstiloDeVida, double inputOcupacion) {
            LinguisticVariable enfermedadesActuales = new LinguisticVariable("EnfermedadesActuales");
            var sano = enfermedadesActuales.MembershipFunctions.AddTrapezoid("Sano", 0, 0, 20, 40);
            var regular = enfermedadesActuales.MembershipFunctions.AddTriangle("Regular", 30, 50, 70);
            var malo = enfermedadesActuales.MembershipFunctions.AddTrapezoid("Malo", 60, 80, 100, 100);

            LinguisticVariable enfermedadesHistoricas = new LinguisticVariable("EnfermedadesHistoricas");
            var bajas = enfermedadesHistoricas.MembershipFunctions.AddTrapezoid("Bajas", 0, 0, 20, 40);
            var normales = enfermedadesHistoricas.MembershipFunctions.AddTriangle("Normales", 30, 50, 70);
            var altas = enfermedadesHistoricas.MembershipFunctions.AddTrapezoid("Altas", 60, 80, 100, 100);

            LinguisticVariable estiloVida = new LinguisticVariable("EstiloVida");
            var tranquilo = estiloVida.MembershipFunctions.AddTrapezoid("Tranquilo", 0, 0, 20, 40);
            var moderado = estiloVida.MembershipFunctions.AddTriangle("Moderado", 30, 50, 70);
            var reisgozo = estiloVida.MembershipFunctions.AddTrapezoid("Riesgozo", 60, 80, 100, 100);

            LinguisticVariable ocupacion = new LinguisticVariable("Ocupacion");
            var ocupacionNormal = ocupacion.MembershipFunctions.AddTrapezoid("Normal", 0, 0, 20, 40);
            var ocupacionBajoRiezgo = ocupacion.MembershipFunctions.AddTriangle("Riesgoza", 30, 50, 70);
            var ocupacionAltoRiezgo = ocupacion.MembershipFunctions.AddTrapezoid("AltoRiezgo", 60, 80, 100, 100);

            LinguisticVariable estadoAseguramiento = new LinguisticVariable("EstadoAseguramiento");
            var coberturaTotal = estadoAseguramiento.MembershipFunctions.AddTrapezoid("Total", 0, 0, 17.5, 30);
            var coberturaModerada = estadoAseguramiento.MembershipFunctions.AddTriangle("Moderada", 20, 37.5, 55);
            var coberturaBaja = estadoAseguramiento.MembershipFunctions.AddTriangle("Baja", 45, 62.5, 80);
            var coberturaNula = estadoAseguramiento.MembershipFunctions.AddTrapezoid("Nula", 70, 87.5, 100, 100);

            IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Create(new CoGDefuzzification());

            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                .If(estiloVida.Is(reisgozo).Or(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                .If(estiloVida.Is(reisgozo).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                .If(estiloVida.Is(moderado).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                .If(estiloVida.Is(moderado).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                .If(estiloVida.Is(moderado).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                  .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                  .If(estiloVida.Is(tranquilo).And(ocupacion.Is(ocupacionBajoRiezgo)))
                  .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                  .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)))
                  .If(estiloVida.Is(tranquilo).And(ocupacion.Is(ocupacionNormal)))
                  .Then(estadoAseguramiento.Is(coberturaModerada));

            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(reisgozo)).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(reisgozo)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaModerada));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(tranquilo)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaModerada));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaModerada));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(regular).Or(enfermedadesHistoricas.Is(normales)).And(estiloVida.Is(tranquilo)).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaTotal));

            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(reisgozo)).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaModerada));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(reisgozo)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaModerada));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaTotal));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(tranquilo)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaTotal));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaTotal));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)))
                .If(estiloVida.Is(tranquilo).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaTotal));

            var result = fuzzyEngine.Defuzzify(new { 
                enfermedadesActuales = inputEnfermedadesActuales, 
                enfermedadesHistoricas = inputEnfermedadesHistoricas, 
                estiloVida = inputEstiloDeVida, 
                ocupacion = inputOcupacion 
            });

            return result;
        }

        public static double CalcularPoliza(FactoresCalculador factores) {
            return CalcularPoliza(factores.EstadoActual, factores.HistorialClinico, factores.EstiloVida, factores.Ocupacion);
        }
    }
}