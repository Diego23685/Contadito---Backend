using Contadito.Api.Domain.DTOs;
using FluentValidation;

public class RegisterTenantRequestValidator : AbstractValidator<RegisterTenantRequest>
{
    public RegisterTenantRequestValidator()
    {
        RuleFor(x => x.TenantName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.OwnerName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.OwnerEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}
    