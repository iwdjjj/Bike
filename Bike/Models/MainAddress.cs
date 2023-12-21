using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bike.Models
{
    public class MainAddress
    {
        [HiddenInput(DisplayValue = false)]
        public int MainAddressId { get; set; }

        [Required(ErrorMessage = "Укажите страну")]
        [Display(Name = "Страна")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Укажите название края/области/республики")]
        [Display(Name = "Название края/области/республики")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Укажите населенный пункт")]
        [Display(Name = "Населенный пункт")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Укажите улицу")]
        [Display(Name = "Улица")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Укажите номер дома")]
        [Display(Name = "Дом")]
        public string? House { get; set; }

        [Display(Name = "Высота ландшафта")]
        public int? HeightId { get; set; }
        [Display(Name = "Высота ландшафта")]
        public virtual Height? Height { get; set; }

        [Display(Name = "Адрес")]
        public string? FullAddress
        {
            get
            {
                return Country + ", " + State + ", " + City + ", " + Street + ", " + House;
            }
        }

        public virtual ICollection<Address>? Address { get; set; }
    }
}
