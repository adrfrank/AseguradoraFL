///<reference path="jquery-2.1.1.js" />
;
debug = true;


(function (window, $, undefined) {
    
})(window, jQuery);

window.page = {
    factors: undefined,
    factorsUrl: "/Home/Factors",
    calcularUrl : "/Home/CalcularPoliza",
    estadoActualId: "#estadoActual",
    historialId: "#historial",
    estiloVidaId: "#estiloVida",
    ocupacionId: "#ocupacion",
    sexoId: "#lstSexo",
    resultadoId: "#lblResultado",
    init: function () {
        this.loadFactors();
        $(page.sexoId).on("change", page.loadControls);
    },
    loadFactors: function () {
        $.getJSON(page.factorsUrl).done(function (json) {
            page.factors = json;
            if (debug)
                console.log("page.factors: ", page.factors);
            page.loadControls();
        }).fail(function (err) {
            alert("Error loading factors");
            if (debug)
                console.log(err);
        });
    },
    loadControls: function () {
        if(!page.factors){
            alert("Factos not loaded");
            return;
        }
        if ($(page.sexoId).val() == "M") {
            page.loadValues(page.estadoActualId, page.factors.EnfermedadesMujer);
            page.loadValues(page.historialId, page.factors.EnfermedadesMujer);
        } else {
            page.loadValues(page.estadoActualId, page.factors.EnfermedadesHombre);
            page.loadValues(page.historialId, page.factors.EnfermedadesHombre);
        }
        page.loadValues(page.estiloVidaId, page.factors.EstiloDeVida);
        page.loadValues(page.ocupacionId, page.factors.Ocupaciones);
       
        $(".block input").on("change", page.calculate);
        page.calculate();

    },
    calculate: function (e) {
        var iEstadoActual = page.calculateInput(page.estadoActualId);
        var iHistorialClinico = page.calculateInput(page.historialId);
        var iEstiloVida = page.calculateInput(page.estiloVidaId);
        var iOcupacion = page.calculateInput(page.ocupacionId);

        var factores = {
            EstadoActual : iEstadoActual,
            HistorialClinico :  iHistorialClinico,
            EstiloVida : iEstiloVida,
            Ocupacion : iOcupacion
        };

        $.ajax({
            url: page.calcularUrl,
            type: "POST",
            dataType: "json",
           // contentType: "application/json",
            data: factores
        }).done(function (res) {
           
            page.showResult(res);
        }).fail(function (err) {
            if (debug)
                console.log("Error: ", err);
        });
        console.log(factores);
    },
    showResult: function (result) {
        var res = parseFloat(result);
        var sres = "", cssClass = "";
        if (res <= 30) {
            sres = "Asegurado normal";
            cssClass = "alert-success";
        } else if (res <= 45) {
            sres = "Asegurado por mas costo";
            cssClass = "alert-warning";
        } else {
            sres = "Imposible de asegurar";
            cssClass = "alert-danger";
        }
        if (debug)
            console.log(sres, res);
        $(page.resultadoId).html(sres).removeClass().addClass(cssClass);
    },
    calculateInput: function (blockId) {
        var sum = 0, cont = 0;
        $(blockId).find(":checked").each(function (index,item) {
            sum += parseFloat(item.value);
            cont++
        });
        var result = 0
        if (cont != 0)
            result = sum ;
        result = result > 100 ? 100 : result;
        return result;
    },
    loadValues: function (containerId, data) {
        var prefix = containerId.substr(1);
        var $container = $(containerId).find("fieldset");
        var cont = 0
        var rows = [];
        var emptyrow = $("<div>").addClass("row");
        var row = emptyrow.clone();
        var emptycell = $("<div>").addClass("col-sm-4");
        for (var i in data) {
            var item = data[i];
            if (debug)
                console.log(item.name, ": ", item.value);
            var cell = emptycell.clone();
            cell.append($("<label>").attr("for", "chk" + prefix + i)
                .append($("<input type='checkbox'>").attr("id", "chk" + prefix + i).val(item.value))
                .append(item.name));
            row.append(cell);
            if (++cont % 3 == 0) {
                rows.push(row);
                row = emptyrow.clone();
            }
        }
        if (cont % 3 != 0)
            rows.push(row);
        $container.find("div").remove();
        $container.append(rows);
    }
}
page.init();