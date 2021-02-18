using System.ComponentModel.DataAnnotations;

namespace MVC_Project.WebBackend.Models
{
    public class CedisViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Clave")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Responsable")]
        public string Manager { get; set; }
    }
}
