using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage ="Length of the name cannot exceed 20 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage ="Invalid Email Property. Kindly Use in these format Example@.com")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public string PhotoPath { get; set; }

        [MaxLength(10, ErrorMessage ="Please Enter a Valid Mobile Number")]
        //[RegularExpression(@"[0-9]{3}-[0-9]{3}-[0-9]{4}$", ErrorMessage = "Please enter the phone number as XXX-XXX-XXXX")]
        //[DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        [Phone]
        public string Mobile { get; set; }



    }
}
