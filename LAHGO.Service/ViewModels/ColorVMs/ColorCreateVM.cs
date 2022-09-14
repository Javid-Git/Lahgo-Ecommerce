using FluentValidation;
using LAHGO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.ColorVMs
{
    public class ColorCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ColorCreateVMVMValidator : AbstractValidator<ColorCreateVM>
    {
        public ColorCreateVMVMValidator()
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
                if (r.Code == null)
                {
                    context.AddFailure("", "Code is required!");
                }
                else
                {

                }
            });
        }
    }
}
