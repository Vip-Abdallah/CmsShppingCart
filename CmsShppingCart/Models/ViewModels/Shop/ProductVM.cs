using CmsShppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShppingCart.Models.ViewModels.Shop
{
    public class ProductVM
    {
        public ProductVM()
        {

        }
        public ProductVM(ProductDTO oProductDTO)
        {
            Id = oProductDTO.Id;
            Name = oProductDTO.Name;
            Slug = oProductDTO.Slug;
            Description = oProductDTO.Description;
            Price = oProductDTO.Price;
            CategoryName = oProductDTO.CategoryName;
            CategoryId = oProductDTO.CategoryId;
            ImageName = oProductDTO.ImageName;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GallaryImage { get; set; }

    }
}