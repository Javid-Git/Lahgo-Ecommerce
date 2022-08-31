using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.ColorVMs
{
    public class ColorUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
    public class ColorUpdateVMValidator : AbstractValidator<ColorUpdateVM>
    {
        public ColorUpdateVMValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("Id Is Required");

            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Name == null)
                {
                    context.AddFailure("", "Name is required!");
                }
                else
                {
                   
                }
            });
        }
    }
}
