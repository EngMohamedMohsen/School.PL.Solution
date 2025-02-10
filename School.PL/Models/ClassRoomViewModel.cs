using System.ComponentModel.DataAnnotations;

namespace School.PL.Models
{
    public class ClassRoomViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
    }
}
