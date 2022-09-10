using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CategoryVMs
{
    public class CategoryUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile FormImage { get; set; }
    }

    public class CategoryPutVMValidator : AbstractValidator<CategoryUpdateVM>
    {
        public CategoryPutVMValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("Id Is Required");

            //RuleFor(r => r.Name)
            //    .NotEmpty().WithMessage("Name Is Required")
            //    .MaximumLength(25).WithMessage("Name Must Be Maximum Length 25")
            //    .MinimumLength(5).WithMessage("Name Must Be Minimum Length 25");


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
                    if (r.Name.Length < 3)
                    {
                        context.AddFailure("", "Minimum length of name is 3!");
                    }
                }
            });
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.FormImage == null)
                {

                }
                else
                {
                    if ((r.FormImage.Length / 1024) > 150)
                    {
                        context.AddFailure("", "File size must be maximum 150kb");
                    }

                    if (r.FormImage.ContentType != "image/jpeg" && r.FormImage.ContentType != "image/webp")
                    {
                        context.AddFailure("", "File type must be .jpg or .jpeg or webp");
                    }
                }
            });
        }
    }
}
