using ApplicationCore.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email can not be empty.")]      // 必填框
        [EmailAddress(ErrorMessage = "Email address is not in a valid format.")]  // 框架会基于内部规则检查有效性
        [StringLength(100, ErrorMessage = "Email must be 100 characters or fewer.")]  // Email 长度限制在100个字符内
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can not be empty.")]
        //(?=.*[a-z]) → 至少有一个小写字母
        //(?=.*[A-Z]) → 至少有一个大写字母
        //(?=.*\d) → 至少有一个数字
        //.{8,} → 长度至少 8 位
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, and 1 number.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "First name can not be empty.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name can not be empty.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Birthday can not be empty.")]
        [YearValidation(1900, 2000)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
