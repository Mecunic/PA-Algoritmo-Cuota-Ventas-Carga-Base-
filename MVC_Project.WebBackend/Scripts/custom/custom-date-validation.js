(function ($) {
  $.validator.methods.date = function (value, element) {
    return this.optional(element) || moment(value, 'dd/mm/yyyy').isValid();
  }
})(jQuery);