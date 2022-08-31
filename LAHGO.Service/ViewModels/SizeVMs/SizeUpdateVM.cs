using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.SizeVMs
{
    public class SizeUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SizeUpdateVMValidator : AbstractValidator<SizeUpdateVM>
    {
        public SizeUpdateVMValidator()
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
