using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bike.Models
{
    public class Address
    {
        [HiddenInput(DisplayValue = false)]
        public int AddressId { get; set; }

        [Display(Name = "Ключевой адрес")]
        public int? MainAddressId { get; set; }
        [Display(Name = "Ключевой адрес")]
        public virtual MainAddress? MainAddress { get; set; }

        [Required(ErrorMessage = "Укажите улицу")]
        [Display(Name = "Улица")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Укажите номер дома")]
        [Display(Name = "Дом")]
        public string? House { get; set; }

        [Display(Name = "Адрес")]
        public string? FullAddress
        {
            get
            {
                return Street + ", " + House;
            }
        }

        public virtual ICollection<Route> Route1 { get; set; }
        public virtual ICollection<Route> Route2 { get; set; }
    }
}
