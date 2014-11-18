using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLS;

namespace AseguradoraFL.Web.Tests
{
    [TestClass]
    public class FLSTest
    {
        [TestMethod]
        public void Test1()
        {
            LinguisticVariable water = new LinguisticVariable("Water");
            var cold = water.MembershipFunctions.AddTrapezoid("Cold", 0, 0, 20, 40);
            var warm = water.MembershipFunctions.AddTriangle("Warm", 30, 50, 70);
            var hot = water.MembershipFunctions.AddTrapezoid("Hot", 50, 80, 100, 100);

            LinguisticVariable power = new LinguisticVariable("Power");
            var low = power.MembershipFunctions.AddTriangle("Low", 0, 25, 50);
            var high = power.MembershipFunctions.AddTriangle("High", 25, 50, 75);

            IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Default();

            fuzzyEngine.Rules.If(water.Is(cold).Or(water.Is(warm))).Then(power.Is(high));
            fuzzyEngine.Rules.If(water.Is(hot)).Then(power.Is(low));

            var result = fuzzyEngine.Defuzzify(new { water = 60 });
           
            
        }

        [TestMethod]
        public void Test2() {
            LinguisticVariable enfermedadesActuales = new LinguisticVariable("EnfermedadesActuales");
            var sano = enfermedadesActuales.MembershipFunctions.AddTrapezoid("Sano", 0, 0, 20, 40);
            var regular = enfermedadesActuales.MembershipFunctions.AddTriangle("Regular", 30, 50, 70);
            var malo = enfermedadesActuales.MembershipFunctions.AddTrapezoid("Malo", 50, 80, 100, 100);

            LinguisticVariable enfermedadesHistoricas = new LinguisticVariable("EnfermedadesHistoricas");
            var bajas = enfermedadesHistoricas.MembershipFunctions.AddTrapezoid("Bajas", 0, 0, 20, 40);
            var normales = enfermedadesHistoricas.MembershipFunctions.AddTriangle("Normales", 30, 50, 70);
            var altas = enfermedadesHistoricas.MembershipFunctions.AddTrapezoid("Altas", 50, 80, 100, 100);

            LinguisticVariable estiloVida = new LinguisticVariable("EstiloVida");
            var tranquilo = estiloVida.MembershipFunctions.AddTrapezoid("Tranquilo", 0, 0, 20, 40);
            var moderado = estiloVida.MembershipFunctions.AddTriangle("Moderado", 30, 50, 70);
            var reisgozo = estiloVida.MembershipFunctions.AddTrapezoid("Riesgozo", 50, 80, 100, 100);

            LinguisticVariable ocupacion = new LinguisticVariable("Ocupacion");
            var ocupacionNormal = ocupacion.MembershipFunctions.AddTrapezoid("Normal", 0, 0, 20, 40);
            var ocupacionBajoRiezgo = ocupacion.MembershipFunctions.AddTriangle("Riesgoza", 30, 50, 70);
            var ocupacionAltoRiezgo = ocupacion.MembershipFunctions.AddTrapezoid("AltoRiezgo", 50, 80, 100, 100);

            LinguisticVariable estadoAseguramiento = new LinguisticVariable("EstadoAseguramiento");
            var coberturaTotal = estadoAseguramiento.MembershipFunctions.AddTrapezoid("Tranquilo", 0, 0, 17.5, 30);
            var coberturaModerada = estadoAseguramiento.MembershipFunctions.AddTriangle("Moderado", 20, 37.5, 55);
            var coberturaBaja = estadoAseguramiento.MembershipFunctions.AddTriangle("Riesgozo", 45, 62.5, 80);
            var coberturaNula = estadoAseguramiento.MembershipFunctions.AddTrapezoid("Riesgozo", 70, 87.5, 100, 100); 

            IFuzzyEngine fuzzyEngine =new FuzzyEngineFactory().Create(new CoGDefuzzification());

            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(reisgozo)).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(reisgozo)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionAltoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaNula));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(moderado)).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                  .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(tranquilo)).And(ocupacion.Is(ocupacionBajoRiezgo)))
                  .Then(estadoAseguramiento.Is(coberturaBaja));
            fuzzyEngine.Rules
                  .If(enfermedadesActuales.Is(malo).Or(enfermedadesHistoricas.Is(altas)).And(estiloVida.Is(tranquilo)).And(ocupacion.Is(ocupacionNormal)))
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
                .If(enfermedadesActuales.Is(sano).Or(enfermedadesHistoricas.Is(bajas)).And(estiloVida.Is(tranquilo)).And(ocupacion.Is(ocupacionNormal)))
                .Then(estadoAseguramiento.Is(coberturaTotal));

            var result = fuzzyEngine.Defuzzify(new { enfermedadesActuales = 60, enfermedadesHistoricas = 60, estiloVida = 60, ocupacion = 60 });
            System.Diagnostics.Trace.WriteLine(result);

        }

        [TestMethod]
        public void Test3() {
            var result = AseguradoraFL.Web.Models.CalculadorPoliza.CalcularPoliza(60, 60, 60, 60);
            System.Diagnostics.Trace.WriteLine(result);
        }
    }
}
