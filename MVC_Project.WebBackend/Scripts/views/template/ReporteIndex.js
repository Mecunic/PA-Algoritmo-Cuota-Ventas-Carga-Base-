/*!
 Controlador Generico para Inicializar un Reporte, con una tabla.
 Los nombres de los objetos son fijos:
 tblResults: ID de la tabla
 searchForm: ID del form de filtros
 btnSearchForm: ID del boton de Buscar
 btnClearForm: ID del boton de Limpiar
*/
var ReporteIndexControlador = function (columns) {

    var searchForm = $('form#searchForm');

    this.init = function () {

        $('#tblResults').DataTable({
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": searchForm.attr('action'),
            orderMulti: false,
            searching: false,
            ordering: false,
            columns: columns,
            responsive: true,
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "sSortColumn", "value": this.fnSettings().aoColumns[this.fnSettings().aaSorting[0][0]].orderName });
                aoData.push({ "name": "filtros", "value": searchForm.serialize() });
                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });

        $('#FilterDateInitial').datepicker({ language: 'es', format: 'dd/mm/yyyy' });
        $('#FilterDateEnd').datepicker({ language: 'es', format: 'dd/mm/yyyy' });

        $("#btnSearchForm").click(function () {
            $('#tblResults').DataTable().draw();
        });

        $("#btnClearForm").click(function () {
            $("#searchForm").each(function () {
                this.reset();
            });
            $('#tblResults').DataTable().draw();
        });

    }
}