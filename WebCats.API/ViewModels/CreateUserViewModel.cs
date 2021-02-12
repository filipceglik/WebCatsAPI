using FluentValidation;

namespace WebCats.ViewModels
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class CreateUserViewModelValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserViewModelValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(40);
            RuleFor(x => x.Role).Matches("(Admin|ReadOnly)");
        }
    }
}