using CmsShppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShppingCart.Models.ViewModels.Shop
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }
        public CategoryVM(CategoryDTO oCategoryDTO)
        {
            Id = oCategoryDTO.Id;
            Name = oCategoryDTO.Name;
            Slug = oCategoryDTO.Slug;
            Sorting = oCategoryDTO.Sorting;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}