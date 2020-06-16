var DocumentsIndexControlador = function (baseUrl, columns) {

    this.init = function () {

        $('#tblResults').DataTable({
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": baseUrl,
            orderMulti: false,
            searching: false,
            ordering: false,
            columns: columns,
            responsive: true,
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "sSortColumn", "value": this.fnSettings().aoColumns[this.fnSettings().aaSorting[0][0]].orderName });
                aoData.push({ "name": "filtros", "value": $('form#SearchForm').serialize() });
                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });
        
        $('#FilterDateInitial').datepicker({ language: 'es', format: 'dd/mm/yyyy' });
        $('#FilterDateEnd').datepicker({ language: 'es', format: 'dd/mm/yyyy' });

        $(".btn-filter").click(function () {
            $('#tblResults').DataTable().draw();
        });

        $("#btnClearForm").click(function () {
            $("#SearchForm").each(function () {
                this.reset();
            });
            $('#tblResults').DataTable().draw();
        });

    }
}