using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bike.Models
{
    public class Height
    {
        [HiddenInput(DisplayValue = false)]
        public int HeightId { get; set; }

        [Required(ErrorMessage = "Укажите высоту ландшафта")]
        [Display(Name = "Высота ландшафта")]
        public int? Terrain_height { get; set; }

        [Required(ErrorMessage = "Укажите сложность прохождения")]
        [Display(Name = "Сложность прохождения")]
        public int? Complexity { get; set; }

        [Required(ErrorMessage = "Укажите прибавку к времени")]
        [Display(Name = "Прибавка к времени (мин)")]
        public int? Time { get; set; }

        public virtual ICollection<MainAddress>? MainAddress { get; set; }
    }
}
