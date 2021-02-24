const PredefinedListDetailControlador = function (htmlTableId, baseUrl) {
    var self = this;
    this.htmlTable = $('#' + htmlTableId);
    this.baseUrl = baseUrl;
    this.dataTable = {};
    this.initModal = function () {

    }
    this.init = function () {
        self.dataTable = this.htmlTable.DataTable({
            language: { url: new URL('/Scripts/custom/dataTableslang.es_MX.json', window.location.origin) },
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": this.baseUrl,
            orderMulti: false,
            searching: false,
            ordering: false,
            columns: [
                { data: 'Id', title: "Id", visible: false },
                { data: 'Sku', title: "SKU" },
                { data: 'Product', title: "Producto" },
                { data: 'Amount', title: "Cantidad" },
                { data: 'IsStrategic', title: "Estratégico", render: renderBoolToText },
                { data: 'IsPrioritary', title: "Prioritario", render: renderBoolToText },
                { data: 'IsTactic', title: "Táctico", render: renderBoolToText }
            ],
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "sSortColumn", "value": this.fnSettings().aoColumns[this.fnSettings().aaSorting[0][0]].orderName });
                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });
    }

  function renderBoolToText(value) {
    return value ? 'Sí' : 'No';
  }
}