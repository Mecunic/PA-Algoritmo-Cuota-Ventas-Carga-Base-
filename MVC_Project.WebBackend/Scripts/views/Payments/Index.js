var PaymentsIndexControlador = function (baseUrl, columns) {
    var self = this;
    this.htmlTable = $('#tblResults');
    this.baseUrl = baseUrl;
    this.init = function () {
        //INICIAMOS DATATABLE
        self.dataTable = this.htmlTable.DataTable({
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": this.baseUrl,
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
    }
}