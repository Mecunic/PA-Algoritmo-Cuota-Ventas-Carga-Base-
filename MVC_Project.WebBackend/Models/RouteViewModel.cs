using System.ComponentModel.DataAnnotations;

namespace MVC_Project.WebBackend.Models
{
    public class RouteViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}
