using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public required string LastName { get; set; }
        [Range(18, 100, ErrorMessage = "La edad debe estar entre 18 y 100")]
        public required int Age { get; set; }
        [Required(ErrorMessage = "El sexo es obligatorio")]
        public required string Sex { get; set; }
    }
}
