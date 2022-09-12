using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace LAHGO.Service.ViewModels.AccountVMs
{
    public class LoginVM
    {

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
    public class LoginVMValidator : AbstractValidator<LoginVM>
    {
        public LoginVMValidator()
        {
            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email is required!").NotEmpty().WithMessage("Email is required!")
                .MaximumLength(255).WithMessage("Maximum length is 255").EmailAddress();
            RuleFor(r => r.Password)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
           
        }
    }
}