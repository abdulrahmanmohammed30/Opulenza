using FluentValidation;

namespace Opulenza.Application.Features.Users.Commands.UploadImage;

public class UploadUserImageCommandValidator: AbstractValidator<UploadUserImageCommand>
{
    private const int MaxFileSize = 10485760;

    public UploadUserImageCommandValidator()
    {
        RuleFor(p => p.File)
            .Cascade(CascadeMode.Stop)
            .Must(file => file is { Length: > 0 }).WithMessage("File is required")
            .Must(file => file is { Length: < MaxFileSize }).WithMessage($"File size must be less than {MaxFileSize} bytes")
            .Must(file =>
            {
                var fileExtension = Path.GetExtension(file.FileName);
                return fileExtension is ".jpg" or ".png";
            }).WithMessage("File must be a .jpg or .png");
    }
}