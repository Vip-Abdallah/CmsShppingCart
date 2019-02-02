using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShppingCart.Models.Data
{
    [Table("tblUserRoles")]
    public class UserRoleDTO
    {
        [Key,Column(Order =0)]
        public int UserID { get; set; }
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        [ForeignKey("UserID")]
        public virtual UserDTO User { get; set; }

        [ForeignKey("RoleId")]
        public virtual RoleDTO Role { get; set; }
    }
}