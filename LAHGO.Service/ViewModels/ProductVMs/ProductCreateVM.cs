using FluentValidation;
using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.PCSVMs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.ProductVMs
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> DiscountedPrice { get; set; }
        public string Describtion { get; set; }
        public IFormFile MainFormImage{ get; set; }
        public List<IFormFile> DetailFormImages{ get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
        public List<int> Counts { get; set; }
        public int CategoryId { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }

    }

    public class ProductCreateVMValidator : AbstractValidator<ProductCreateVM>
    {
        public ProductCreateVMValidator()
        {
            RuleFor(x => x).Custom((x, y) =>
              {
                  if (x.Name == null)
                  {
                      y.AddFailure("Name", "Name is required!");
                  }
                  if (x.MainFormImage == null)
                  {
                      y.AddFailure("MainFormImage", "Image is Required!");
                  }
                  else
                  {
                      if ((x.MainFormImage.Length / 1024) > 3000)
                      {
                          y.AddFailure("MainFormImage", "File size sust be maximum 3 Mb");
                      }

                      if (x.MainFormImage.ContentType != "image/jpeg")
                      {
                          y.AddFailure("MainFormImage", "File type must be .jpg or .jpeg");
                      }
                  }
                  if (x.DetailFormImages == null)
                  {
                      y.AddFailure("DetailFormImages", "Image is Required!");

                  }
                  else
                  {
                      foreach (IFormFile file in x.DetailFormImages)
                      {
                          if (file == null)
                          {
                              y.AddFailure("DetailFormImages", "Image is Required!");
                          }
                          if ((file.Length / 1024) > 3000)
                          {
                              y.AddFailure("DetailFormImages", "File size sust be maximum 3 Mb");
                          }

                          if (file.ContentType != "image/jpeg")
                          {
                              y.AddFailure("DetailFormImages", "File type must be .jpg or .jpeg");
                          }
                      }
                  }
                  if (x.Price == null || x.DiscountedPrice == 0)
                  {
                      y.AddFailure("Price", "Price should be entered");
                      y.AddFailure("DiscountedPrice", "Price should be entered");

                  }
                  if (x.Price < 0 || x.DiscountedPrice < 0)
                  {
                      y.AddFailure("Price", "Price cant be negative");
                      y.AddFailure("DiscountedPrice", "Price cant be negative");
                  }
              });
        } 
    }
}
