using System.ComponentModel.DataAnnotations;

namespace DiplomaProject_ITMO.Models
{
    public class ProjectModel
    {
        [Key]
        public int Id { get; set; } 

        public decimal LandCost { get; set; }
        public string MaterialType { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string FoundationType { get; set; }
        public int WindowCount { get; set; }

        
    }
}
