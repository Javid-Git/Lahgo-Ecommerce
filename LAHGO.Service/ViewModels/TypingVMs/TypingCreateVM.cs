using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.TypingVMs
{
    public class TypingCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TypingCreateVMValidator : AbstractValidator<TypingCreateVM>
    {
        public TypingCreateVMValidator()
        {

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
