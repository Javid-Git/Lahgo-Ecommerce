using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CategoryVMs
{
    public class CategoryPostVM
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class CategoryPostVMValidator : AbstractValidator<CategoryPostVM>
    {
        public CategoryPostVMValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name is required!")
                .MaximumLength(25).WithMessage("Maximum length is 25")
                .MinimumLength(5).WithMessage("Name is required!");

            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.File == null)
                {
                    context.AddFailure("File", "File Is Reuired");
                }

                if ((r.File.Length / 1024) > 30)
                {
                    context.AddFailure("File", "File Size Must Be Maximum 30kb");
                }

                if (r.File.ContentType != "image/jpeg")
                {
                    context.AddFailure("File", "File Type Must Be .jpg or .jpeg");
                }
            });
        }
    }
}
