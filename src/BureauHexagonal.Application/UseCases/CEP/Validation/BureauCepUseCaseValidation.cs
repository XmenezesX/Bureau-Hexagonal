using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Utils;

namespace BureauHexagonal.Application.UseCases.CEP.Validation
{
    public sealed class BureauCepUseCaseValidation : IBureauCepUseCaseValidation
    {
        private const int ZipCodeLength = 8;

        public NotificationErrors Validate(BureauInputDto input)
        {
            if (input is null)
                return NotificationErrors.Create(nameof(input), DefaultMessagesErrors.RequestIsNull);

            var notification = NotificationErrors.Empty;

            if (string.IsNullOrWhiteSpace(input.Code))
                notification.AddError(nameof(input.Code), DefaultMessagesErrors.FieldIsRequired(nameof(input.Code)));

            if (input.Code is not null && input.Code.Length != ZipCodeLength)
                notification.AddError(nameof(input.Code), DefaultMessagesErrors.ExactLength(nameof(input.Code), ZipCodeLength));

            if (!EnumUtils.IsValidEnum(input.ProviderType))
                notification.AddError(nameof(input.ProviderType), DefaultMessagesErrors.EnumIsInvalid, "É necessário informar um provedor válido");

            if (!EnumUtils.IsValidEnum(input.BureauType))
                notification.AddError(nameof(input.BureauType), DefaultMessagesErrors.EnumIsInvalid, "É necessário informar um bureau válido");

            if (input.BureauType != BureauType.CEP)
                notification.AddError(nameof(input.BureauType), DefaultMessagesErrors.BureauTypeInvalid);

            return notification;
        }
    }
}
