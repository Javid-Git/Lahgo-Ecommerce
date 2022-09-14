﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.SettingVMs
{
    public class SettingUpdateVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class SettingUpdateVMValidator : AbstractValidator<SettingUpdateVM>
    {
        public SettingUpdateVMValidator()
        {

            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Key == null)
                {
                    context.AddFailure("", "Key is required!");
                }
                else
                {

                }
                if (r.Value == null)
                {
                    context.AddFailure("", "Value is required!");
                }
                else
                {

                }
            });
        }
    }
}