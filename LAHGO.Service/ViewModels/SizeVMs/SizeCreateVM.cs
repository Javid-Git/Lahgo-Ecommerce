using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.SizeVMs
{
    public class SizeCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SizeCreateVMVMValidator : AbstractValidator<SizeCreateVM>
    {
        public SizeCreateVMVMValidator()
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
