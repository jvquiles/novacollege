using FluentValidation;
using Novacollege.WebApi.Dtos;

namespace Novacollege.WebApi.Validators;

public class CreateEstudianteRequestValidator : AbstractValidator<CreateEstudianteRequest>
{
    public CreateEstudianteRequestValidator()
    {
        RuleFor(x => x.ApelEst)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100);

        RuleFor(x => x.NombEst)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100);

        RuleFor(x => x.FnacEst)
            .Must(x => !x.HasValue || x.Value < DateOnly.FromDateTime(DateTime.Now)).WithMessage("La fecha de nacimiento debe ser anterior a hoy")
            .When(x => x.FnacEst.HasValue);

        RuleFor(x => x.SexoEst)
            .MaximumLength(1)
            .Must(x => x == null || x == "M" || x == "F")
            .WithMessage("El sexo debe ser 'M' o 'F'");

        RuleFor(x => x.DireEst)
            .MaximumLength(200);

        RuleFor(x => x.TcolEst)
            .MaximumLength(30);

        RuleFor(x => x.GinsEst)
            .Must(x => !x.HasValue || x.Value <= DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura")
            .When(x => x.GinsEst.HasValue);

        RuleFor(x => x.IdDistrito)
            .GreaterThan(0).WithMessage("El distrito es requerido");
    }
}