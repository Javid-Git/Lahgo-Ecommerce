using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Service.ViewModels.AccountVMs
{
    public class ProfileVM
    {
       
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ConfirmPasword { get; set; }
    }
    public class ProfileVMValidator : AbstractValidator<ProfileVM>
    {
        public ProfileVMValidator()
        {
            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email is required!").NotEmpty().WithMessage("Email is required!")
                .MaximumLength(255).WithMessage("Maximum length is 255").EmailAddress();
            RuleFor(r => r.FullName)
                .NotNull().WithMessage("Full Name is required!").NotEmpty().WithMessage("Full Name is required!")
                .MaximumLength(255).WithMessage("Maximum length is 255");
            RuleFor(r => r.UserName)
               .NotNull().WithMessage("Username is required!").NotEmpty().WithMessage("Username is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
            RuleFor(r => r.CurrentPassword)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
            RuleFor(r => r.NewPassword)
              .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
              .MaximumLength(255).WithMessage("Maximum length is 255");
            RuleFor(r => r.ConfirmPasword)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255");
        }
    }
}
