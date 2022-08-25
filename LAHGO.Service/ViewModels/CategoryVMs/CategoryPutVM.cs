using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CategoryVMs
{
    public class CategoryPutVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class CategoryPutVMValidator : AbstractValidator<CategoryPutVM>
    {
        public CategoryPutVMValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("Id Is Required");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name Is Required")
                .MaximumLength(25).WithMessage("Name Must Be Maximum Length 25")
                .MinimumLength(5).WithMessage("Name Must Be Minimum Length 25");

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
