using CmsShppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShppingCart.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }
        public SidebarVM(SidebarDTO oSidebarDTO)
        {
            id = oSidebarDTO.id;
            Body = oSidebarDTO.Body;
        }
        
        public int id { get; set; }
        [Required]
        [StringLength(int.MaxValue)]
        public string Body { get; set; }
    }
}