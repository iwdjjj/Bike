using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bike.Models
{
    public class Route
    {
        [HiddenInput(DisplayValue = false)]
        public int RouteId { get; set; }

        [Display(Name = "Отправной пункт")]
        public int? AddressId1 { get; set; }
        [Display(Name = "Отправной пункт")]
        public virtual Address? Address1 { get; set; }

        [Display(Name = "Пункт назначения")]
        public int? AddressId2 { get; set; }
        [Display(Name = "Пункт назначения")]
        public virtual Address? Address2 { get; set; }

        [Display(Name = "Тип велосипеда")]
        public int? BikeTypeId { get; set; }
        [Display(Name = "Тип велосипеда")]
        public virtual BikeType? BikeType { get; set; }

        [Required(ErrorMessage = "Укажите прибавку к времени")]
        [Display(Name = "Прибавка к времени (мин)")]
        public int? Time { get; set; }

        [Display(Name = "Примерное время поездки")]
        public int? TimeResult { get; set; }
    }
}
