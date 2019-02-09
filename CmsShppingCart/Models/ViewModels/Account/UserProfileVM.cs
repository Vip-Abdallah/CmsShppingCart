using CmsShppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShppingCart.Models.ViewModels.Account
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {

        }
        public UserProfileVM(UserDTO oUserDTO)
        {
            Id = oUserDTO.Id;
            FirstName = oUserDTO.FirstName;
            LastName = oUserDTO.LastName;
            EmailAddress = oUserDTO.EmailAddress;
            UserName = oUserDTO.UserName;
            Password = oUserDTO.Password;
        }

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}