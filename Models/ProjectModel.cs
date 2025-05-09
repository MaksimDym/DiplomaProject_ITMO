using System.ComponentModel.DataAnnotations;

namespace DiplomaProject_ITMO.Models
{
    public class ProjectModel
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Стоимость участка обязательна для заполнения.")]
        [Range(0, double.MaxValue, ErrorMessage = "Стоимость участка должна быть положительным числом.")]
        [Display(Name = "Стоимость участка")]
        public decimal LandCost { get; set; }

        [Required(ErrorMessage = "Тип материала обязателен для выбора.")]
        [Display(Name = "Тип материала")]
        public string MaterialType { get; set; }

        [Required(ErrorMessage = "Стоимость материала обязательна для заполнения.")]
        [Range(0, double.MaxValue, ErrorMessage = "Стоимость материала должна быть положительным числом.")]
        [Display(Name = "Стоимость материала")]
        public decimal MaterialCost { get; set; }

        [Required(ErrorMessage = "Длина дома обязательна для заполнения.")]
        [Range(0, double.MaxValue, ErrorMessage = "Длина дома должна быть положительным числом.")]
        [Display(Name = "Длина дома")]
        public double Length { get; set; }

        [Required(ErrorMessage = "Ширина дома обязательна для заполнения.")]
        [Range(0, double.MaxValue, ErrorMessage = "Ширина дома должна быть положительным числом.")]
        [Display(Name = "Ширина дома")]
        public double Width { get; set; }

        [Required(ErrorMessage = "Высота дома обязательна для заполнения.")]
        [Range(0, double.MaxValue, ErrorMessage = "Высота дома должна быть положительным числом.")]
        [Display(Name = "Высота дома")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Тип фундамента обязателен для выбора.")]
        [Display(Name = "Тип фундамента")]
        public string FoundationType { get; set; }

        [Required(ErrorMessage = "Количество окон обязательно для заполнения.")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество окон должно быть положительным числом.")]
        [Display(Name = "Количество окон")]
        public int WindowCount { get; set; }

        [Display(Name = "Сауна ")]
        public bool HasSauna { get; set; }

        [Display(Name = "Забор")]
        public bool HasFence { get; set; }

        [Display(Name = "Подвод электричества")]
        public bool NeedsElectricity { get; set; }

        [Display(Name = "Подвод водопровода")]
        public bool HasWaterSupply { get; set; }

        [Display(Name = "Общая стоимость проекта")]
        public decimal TotalCost { get; set; } 
    }
}