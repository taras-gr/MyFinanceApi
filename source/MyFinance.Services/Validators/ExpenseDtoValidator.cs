//using FluentValidation;
//using MyFinance.Services.DataTransferObjects;
//using System;
//using System.Linq;

//namespace MyFinance.Services.Validators
//{
//    public class ExpenseForCreationDtoValidator : AbstractValidator<ExpenseForCreationDto>
//    {
//        public ExpenseForCreationDtoValidator()
//        {
//            RuleFor(p => p.Title)
//                .NotEmpty().WithMessage("{PropertyName} can't be empty.")
//                .Length(2, 30).WithMessage("{PropertyName} length ({TotalLength}) is invalid. It should be from 2 to 30.")
//                .Must(BeAValidString).WithMessage("{PropertyName} contains invalid characters.");

//            RuleFor(p => p.Cost)
//                .NotEmpty().WithMessage("{PropertyName} can't be empty.")
//                .GreaterThan(0).WithMessage("{PropertyName} can't be less than zero.");

//            RuleFor(p => p.ExpenseDate)
//                .NotEmpty().WithMessage("{PropertyName} can't be empty.")
//                .LessThanOrEqualTo(DateTimeOffset.Now).WithMessage("You can't input future expenses.");

//            RuleFor(p => p.Category)
//                .NotEmpty().WithMessage("{PropertyName} can't be empty.")
//                .Length(2, 30).WithMessage("{PropertyName} length ({TotalLength}) is invalid. It should be from 2 to 30.")
//                .Must(BeAValidString).WithMessage("{PropertyName} contains invalid characters.");
//        }

//        protected bool BeAValidString(string s)
//        {
//            return s.All(char.IsLetter);
//        }
//    }

//    public class ExpenseForEditingDtoValidator : AbstractValidator<ExpenseForEditingDto>
//    {
//        public ExpenseForEditingDtoValidator()
//        {
//            Include(new ExpenseForCreationDtoValidator());
//        }
//    }
//}