using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.BestPractices.DTOs;
using FluentValidation;
using FluentValidation.Validators;

namespace MinimalApi.BestPractices.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(c => c.Email).NotNull().Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("The format of the ‘Email’ value is incorrect.");

            RuleFor(c => c.Username).NotNull().Matches("^[a-zA-Z0-9]*$").WithMessage("The format of the ‘Username’ value is incorrect.").MinimumLength(10).WithMessage("The username must be at least 10 characters long.");

            RuleFor(c => c.Password).NotNull().Matches("^(?=.*[A-Z])(?=.*\\d).{8,}$").WithMessage("The password must be at least 8 characters long and contain one uppercase letter and one number.");
        }
    }
}