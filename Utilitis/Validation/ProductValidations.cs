using Caching_with_Redis.Controllers.Products.Request;
using FluentValidation;

namespace Caching_with_Redis.Utalitis.Validation
{
    public class ProductValidations:AbstractValidator<ProuductRequest>
    {
        public ProductValidations()
        {
            RuleFor(p => p.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull().WithMessage("Product {PropertyName} Shouldn't Be Null")
                .MinimumLength(3).WithMessage("Product {PropertyName} Shouldnn't Be less Than 3-letters")
                .MaximumLength(20).WithMessage("Product {PropertyName} Shouldn't Be More Than 20-letters")
                .Must(IsLetters).WithMessage("Product {PropertyName} Should Be Letters");

            RuleFor(p => p.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull().WithMessage("Product {PropertyName} Shouldn't Be Null")
                .MinimumLength(3).WithMessage("Product {PropertyName} Shouldnn't Be less Than 3-letters")
                .MaximumLength(50).WithMessage("Product {PropertyName} Shouldn't Be More Than 50-letters");

            RuleFor(p=>p.Category)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull().WithMessage("Product {PropertyName} Shouldn't Be Null")
                .MinimumLength(3).WithMessage("Product {PropertyName} Shouldnn't Be less Than 3-letters")
                .MaximumLength(15).WithMessage("Product {PropertyName} Shouldn't Be More Than 15-letters")
                .Must(IsLetters).WithMessage("Product {PropertyName} Should Be Letters"); ;

            RuleFor(p => p.Price)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull().WithMessage("Product {PropertyName} Shouldn't Be Null")
                .GreaterThan(5).WithMessage("Product {PropertyName} Shouldn't Be less Than $5");
                


        }
        private static bool IsLetters(string Value)
        {
            return Value.All(char.IsLetter);
        }
    }
}
