/*!
 Controlador Generico para Inicializar un Reporte, con una tabla.
 Los nombres de los objetos son fijos:
 tblResults: ID de la tabla
 searchForm: ID del form de filtros
 btnSearchForm: ID del boton de Buscar
 btnClearForm: ID del boton de Limpiar
 tblButtonsResults: ID del contenedor de Botones
*/
var ReporteIndexControlador = function (columns,buttons) {

    var searchForm = $('form#searchForm');

    this.init = function () {

        var myTable = $('#tblResults').DataTable({
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
        
        if (buttons) {

            const arrayButtons = [];

            if (buttons.copy && buttons.copy === true) {
                arrayButtons.push({
                    extend: 'copy',
                    text: '<i class="fa fa-files-o"></i> Copy',
                    titleAttr: 'Copy',
                    className: 'btn btn-sm btn-primary'
                });
            }

            if (buttons.csv && buttons.csv === true) {
                arrayButtons.push({
                    extend: 'csv',
                    text: '<i class="fa fa-files-o"></i> Copy',
                    titleAttr: 'CSV',
                    className: 'btn btn-sm btn-primary',
                    exportOptions: {
                        columns: ':visible'
                        
                    }
                });
            }

            if (buttons.excel && buttons.excel === true) {
                arrayButtons.push({
                    extend: 'excel',
                    text: '<i class="fa fa-files-o"></i> Excel',
                    titleAttr: 'Excel',
                    className: 'btn btn-sm btn-primary',
                    exportOptions: {
                        columns: ':visible',
                        
                    }
                });
            }

            if (buttons.pdf && buttons.pdf === true) {
                arrayButtons.push({
                    extend: 'pdf',
                    text: '<i class="fa fa-file-pdf-o"></i> PDF',
                    titleAttr: 'PDF',
                    className: 'btn btn-sm btn-primary',
                    exportOptions: {
                        columns: ':visible'
                    }
                });
            }

            if (buttons.print && buttons.print === true) {
                arrayButtons.push({
                    extend: 'print',
                    text: '<i class="fa fa-print"></i> Print',
                    titleAttr: 'Imprimir',
                    className: 'btn btn-sm btn-primary',
                    exportOptions: {
                        columns: ':visible'
                    }
                });
            }
            
            new $.fn.dataTable.Buttons(myTable, {
                buttons: arrayButtons
            });

            myTable.buttons().container().appendTo("#tblButtonsResults");
        }

        
        

    }
}