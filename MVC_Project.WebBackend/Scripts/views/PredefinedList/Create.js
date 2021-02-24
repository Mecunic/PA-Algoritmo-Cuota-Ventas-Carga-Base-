const PredefinedListCreateControlador = function (htmlTableId, createUrl) {
  var self = this;
  this.htmlTable = $('#' + htmlTableId);
  this.createUrl = createUrl;
  this.dataTable = {};
  this.items = [];
  this.createForm = $('#create-form');
  this.addItemBtn = $('#add-item-btn');
  this.utils = new Utils();

  function renderBoolToText(value) {
    return value ? 'Sí' : 'No';
  }

  this.init = function () {
    self.dataTable = this.htmlTable.DataTable({
      language: { url: new URL('/Scripts/custom/dataTableslang.es_MX.json', window.location.origin) },
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
    });
  }

  this.addItemBtn.click(function () {
    if (self.createForm.valid()) {
      var formData = self.utils.UserInterfaceToData(self.createForm);
      if (formData.StartDate) {
        var d = formData.StartDate.split("/");
        formData.StartDate = d[2] + "-" + d[1] + "-" + d[0];
      }
      if (formData.EndDate) {
        var d = formData.EndDate.split("/");
        formData.EndDate = d[2] + "-" + d[1] + "-" + d[0];
      }
      console.log(formData);
      console.log(self.createForm.find('input[type=checkbox]:checked'));
    }
  });

  $('.start-date-picker, .end-date-picker').datepicker({
    format: 'dd/mm/yyyy',
    language: 'es',
    orientation: 'bottom',
    assumeNearbyYear: 20
  });
}