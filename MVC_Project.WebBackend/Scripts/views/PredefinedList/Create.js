const PredefinedListCreateControlador = function (htmlTableId, initialData) {
  var self = this;
  this.htmlTable = $('#' + htmlTableId);
  this.dataTable = {};
  this.items = initialData || [];
  this.createForm = $('#create-form');
  this.addItemBtn = $('#add-item-btn');
  this.saveBtn = $('#save-btn');
  this.utils = new Utils();

  function renderBoolToText(value) {
    return value ? 'Sí' : 'No';
  }

  this.init = function () {
    for (var i = 0; i < this.items.length; i++) {
      this.items[i].Id = i;
      this.items[i].Product = self.createForm.find('#ProductId option[value=' + this.items[i].ProductId + ']').text();
    }

    self.dataTable = this.htmlTable.DataTable({
      language: { url: new URL('/Scripts/custom/dataTableslang.es_MX.json', window.location.origin) },
      orderMulti: false,
      searching: false,
      ordering: false,
      data: self.items,
      columns: [
        { data: 'Id', title: "Id", visible: false },
        { data: 'ProductId', title: "SKU" },
        { data: 'Product', title: "Producto" },
        { data: 'Amount', title: "Cantidad", width: '120px' },
        { data: 'IsStrategic', title: "Estratégico", width: '90px', render: renderBoolToText },
        { data: 'IsPrioritary', title: "Prioritario", width: '90px', render: renderBoolToText },
        { data: 'IsTactic', title: "Táctico", width: '90px', render: renderBoolToText },
        {
          data: null,
          title: 'Acciones',
          width: '90px',
          render: function (data) {
            const buttons = `<div class="btn-group" role="group" aria-label="Opciones">
                                    <button type="button" class="btn btn-danger btn-remove" title="Eliminar"><span class="far fa-trash-alt"></span></button>
                                </div>
                            `;
            return buttons;
          }
        }
      ],
    });

    $(this.htmlTable, "tbody").on('click',
      '.btn-group .btn-remove',
      function () {
        var tr = $(this).closest('tr');
        var row = self.dataTable.row(tr);
        var id = row.data().Id;

        var indexOfData = self.items.findIndex(function (el) {
          return el.Id == id;
        });

        if (indexOfData > -1) {
          self.items.splice(indexOfData, 1);
          row.remove().draw();
        }

      });

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
        formData.Product = self.createForm.find('#ProductId option:selected').text();
        formData.Id = self.items.length;
        self.items.push(formData);
        self.dataTable.row.add(formData).draw();

        self.createForm.find('#ProductId').prop('selectedIndex', 0);
        self.createForm.find('#Amount').val(1);
        $('.i-checks').iCheck('uncheck');
        $('.i-checks').iCheck('update');
      }
    });

    this.saveBtn.click(function () {
      $('#item-inputs-container').find('input, select').prop('disabled', true);
      if (self.createForm.valid()) {
        for (var i = 0; i < self.items.length; i++) {
          var itemData = self.items[i];
          var inputProduct = '<input type="hidden" name="ProductsList[' + i + '].ProductId" value="' + itemData.ProductId + '" />';
          var inputAmount = '<input type="hidden" name="ProductsList[' + i + '].Amount" value="' + itemData.Amount + '" />';
          var inputStrategic = '<input type="hidden" name="ProductsList[' + i + '].IsStrategic" value="' + itemData.IsStrategic + '" />';
          var inputPrioritary = '<input type="hidden" name="ProductsList[' + i + '].IsPrioritary" value="' + itemData.IsPrioritary + '" />';
          var inputTactic = '<input type="hidden" name="ProductsList[' + i + '].IsTactic" value="' + itemData.IsTactic + '" />';
          self.createForm.append(inputProduct);
          self.createForm.append(inputAmount);
          self.createForm.append(inputStrategic);
          self.createForm.append(inputPrioritary);
          self.createForm.append(inputTactic);
        }
        self.createForm.submit();
      } else {
        $('#item-inputs-container').find('input, select').removeAttr('disabled');
      }
    })

    $('.start-date-picker, .end-date-picker').datepicker({
      format: 'dd/mm/yyyy',
      language: 'es',
      orientation: 'bottom',
      assumeNearbyYear: 20
    });
  }
}