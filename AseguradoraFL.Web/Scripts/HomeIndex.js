///<reference path="jquery-2.1.1.js" />
;
debug = true;


(function (window, $, undefined) {
    
})(window, jQuery);

window.page = {
    factors: undefined,
    factorsUrl: "/Home/Factors",
    estadoActualId: "#estadoActual",
    historialId: "#historial",
    estiloVidaId: "#estiloVida",
    ocupacionId: "#ocupacion",
    sexoId: "#lstSexo",
    init: function () {
        this.loadFactors();
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
        page.loadValues(page.estadoActualId, page.factors.EnfermedadesHombre);
        page.loadValues(page.historialId, page.factors.EnfermedadesHombre);
        page.loadValues(page.estiloVidaId, page.factors.EstiloDeVida);
        page.loadValues(page.ocupacionId, page.factors.Ocupaciones);
        

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
            cell.append($("<input type='checkbox'>").attr("id","chk"+prefix+i).val(item.value));
            cell.append($("<label>").attr("for","chk"+prefix+i).html(item.name));
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