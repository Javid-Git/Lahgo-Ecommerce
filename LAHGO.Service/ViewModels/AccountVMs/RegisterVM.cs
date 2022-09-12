using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace LAHGO.Service.ViewModels.AccountVMs
{
    public class RegisterVM
    {
       
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
       
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPasword { get; set; }
        public bool Subscribe { get; set; }

    }
    public class RegisterVMValidator : AbstractValidator<RegisterVM>
    {
        public RegisterVMValidator()
        {
            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email is required!").NotEmpty().WithMessage("Email is required!")
                .MaximumLength(255).WithMessage("Maximum length is 255").EmailAddress();
            RuleFor(r => r.FullName)
                .NotNull().WithMessage("Full Name is required!").NotEmpty().WithMessage("Full Name is required!")
                .MaximumLength(355).WithMessage("Maximum length is 255");
            RuleFor(r => r.UserName)
               .NotNull().WithMessage("Username is required!").NotEmpty().WithMessage("Username is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
            RuleFor(r => r.Password)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
            RuleFor(r => r.ConfirmPasword)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
        }
    }
}