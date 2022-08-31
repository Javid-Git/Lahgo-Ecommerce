using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.ProductVMs
{
    public class ProductCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
        public string Describtion { get; set; }
        public IFormFile MainFormImage{ get; set; }
        public List<IFormFile> DetailFormImages{ get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
    }

    public class ProductCreateVMValidator : AbstractValidator<ProductCreateVM>
    {
        public ProductCreateVMValidator()
        {
            RuleFor(x => x).Custom((x, y) =>
              {
                  if (x.Name == null)
                  {
                      y.AddFailure("", "Name is required!");
                  }
                  if (x.MainFormImage == null)
                  {
                      y.AddFailure("", "Image is Required!");
                  }
                  foreach (IFormFile file in x.DetailFormImages)
                  {
                      if (file == null)
                      {
                          y.AddFailure("", "Image is Required!");
                      }
                  }
              });
        } 
    }
}
