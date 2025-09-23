using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Code IS Required!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name IS Required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "CreateAt IS Required!")]
        public DateTime CreateAt { get; set; }
    }
}
