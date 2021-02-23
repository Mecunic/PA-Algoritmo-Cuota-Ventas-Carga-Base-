const InOutDetailControlador = function (htmlTableId, baseUrl) {
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
                { data: 'Sku', title: "SKU" },
                { data: 'Description', title: "Descripción" },
                { data: 'Cedis', title: "CEDIS" },
                {
                    data: null,
                    //className: 'personal-options',
                    title: 'Acciones',
                    render: function (data) {
                        const buttons = `<div class="btn-group" role="group" aria-label="Opciones">
                                <div class="btn-group" role="group" aria-label="Opciones">
                                    <button type="button" class="btn btn-light btn-view" title="Detalle"><span class="fa fa-eye"></span></button>
                                </div>
                            `;
                        return buttons;
                    }
                }
            ],
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push({ "name": "sSortColumn", "value": this.fnSettings().aoColumns[this.fnSettings().aaSorting[0][0]].orderName });
                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });
    }
}