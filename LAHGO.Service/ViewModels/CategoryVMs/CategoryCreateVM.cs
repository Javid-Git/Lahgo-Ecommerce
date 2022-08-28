using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CategoryVMs
{
    public class CategoryCreateVM
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public IFormFile FormImage { get; set; }
    }

    public class CategoryCreateVMVMValidator : AbstractValidator<CategoryCreateVM>
    {
        public CategoryCreateVMVMValidator()
        {
            //RuleFor(r => r.Name)
            //    .NotNull().WithMessage("Name is required!").NotEmpty().WithMessage("Name is required!")
            //    .MaximumLength(25).WithMessage("Maximum length is 25")
            //    .MinimumLength(5).WithMessage("Name is required!");
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Name == null)
                {
                    context.AddFailure("", "Name is required!");
                }
                else
                {
                    if (r.Name.Length > 25)
                    {
                        context.AddFailure("", "Maximum length of name is 25!");
                    }
                    if (r.Name.Length < 5)
                    {
                        context.AddFailure("", "Minimum length of name is 5!");
                    }
                }
            });
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.FormImage == null)
                {
                    context.AddFailure("", "File is required!");
                }
                else
                {
                    if ((r.FormImage.Length / 1024) > 150)
                    {
                        context.AddFailure("", "File size must be maximum 150kb");
                    }

                    if (r.FormImage.ContentType != "image/jpeg")
                    {
                        context.AddFailure("", "File type must be .jpg or .jpeg");
                    }
                }
            });
        }
    }
}
