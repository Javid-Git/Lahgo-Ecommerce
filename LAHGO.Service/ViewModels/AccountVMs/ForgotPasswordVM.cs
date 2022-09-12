using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Service.ViewModels.AccountVMs
{
    public class ForgotPasswordVM
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordVMValidator : AbstractValidator<ForgotPasswordVM>
    {
        public ForgotPasswordVMValidator()
        {
            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email is required!").NotEmpty().WithMessage("Email is required!")
                .MaximumLength(255).WithMessage("Maximum length is 255").EmailAddress();
        }
    }
}
