using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Service.ViewModels.AccountVMs
{
    public class ResetPasswordVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPasword { get; set; }
        public string Token { get; set; }
    }
    public class ResetPasswordVMValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordVMValidator()
        {
            
            RuleFor(r => r.Password)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255"); 
            RuleFor(r => r.ConfirmPasword)
               .NotNull().WithMessage("Password is required!").NotEmpty().WithMessage("Password is required!")
               .MaximumLength(255).WithMessage("Maximum length is 255"); 
        }
    }
}
