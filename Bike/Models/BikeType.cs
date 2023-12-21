using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bike.Models
{
    public class BikeType
    {
        [HiddenInput(DisplayValue = false)]
        public int BikeTypeId { get; set; }

        [Required(ErrorMessage = "Укажите название")]
        [Display(Name = "Название")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Укажите сложность управления")]
        [Display(Name = "Сложность управления")]
        public int? Complexity { get; set; }

        [Required(ErrorMessage = "Укажите скорость")]
        [Display(Name = "Скорость")]
        public int? Speed { get; set; }

        [Required(ErrorMessage = "Укажите прибавку к времени")]
        [Display(Name = "Прибавка к времени (мин)")]
        public int? Time { get; set; }

        public virtual ICollection<Route>? Route { get; set; }
    }
}
