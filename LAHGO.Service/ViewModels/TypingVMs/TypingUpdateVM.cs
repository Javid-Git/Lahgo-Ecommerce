using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.TypingVMs
{
    public class TypingUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TypingUpdateVMValidator : AbstractValidator<TypingUpdateVM>
    {
        public TypingUpdateVMValidator()
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
