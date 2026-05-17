using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Entities.Base;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Utils;

namespace BureauHexagonal.Core.Entities
{
    public sealed class BureauEntity : BaseEntity
    {
        public ProviderType ProviderType { get; private set; }
        public string ProviderTypeDescription { get; private set; }
        public BureauType BureauType { get; private set; }
        public string BureauTypeDescription { get; private set; }
        public string Code { get; private set; }
        public string ResponseProviderJson { get; private set; }
        public string DataBureau { get; private set; }
        public bool Synchronized { get; private set; }

        private BureauEntity(){}

        public static IOperation<BureauEntity> ConceptualCreate(string code, ProviderType providerType, BureauType bureauType, string responseProviderJson, string dataBureau, bool synchronized)
        {
            var entity = new BureauEntity
            {
                Id = Guid.NewGuid(),
                Code = code.OnlyNumbers(),
                ProviderType = providerType,
                ResponseProviderJson = responseProviderJson,
                DataBureau = dataBureau,
                Synchronized = synchronized,
                BureauType = bureauType,
                BureauTypeDescription = BureauTypeDescriptor.Value[bureauType],
                ProviderTypeDescription = ProviderTypeDescriptor.Value[providerType],
                CreatedAt = DateTime.UtcNow,
            };

            var notification = entity.Validate();
            if (notification.HaveError())
                return OperationFactory.CreateFail<BureauEntity>(notification, ErrorType.ValidationError);

            return entity.ToSuccess();
        }

        public static BureauEntity Restore(
            in Guid id,
            string code,
            in ProviderType providerType,
            in BureauType bureauType,
            string responseProviderJson,
            string dataBureau,
            in bool synchronized,
            DateTimeOffset createdAt,
            DateTimeOffset? updatedAt,
            DateTimeOffset? deletedAt
        )
        {
            return new BureauEntity
            {
                Id = id,
                Code = code,
                ProviderType = providerType,
                ResponseProviderJson = responseProviderJson,
                DataBureau = dataBureau,
                Synchronized = synchronized,
                BureauType = bureauType,
                BureauTypeDescription = BureauTypeDescriptor.Value[bureauType],
                ProviderTypeDescription = ProviderTypeDescriptor.Value[providerType],
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                DeletedAt = deletedAt
            };
        }

        public override NotificationErrors Validate()
        {
            var notification = NotificationErrors.Empty;

            if (string.IsNullOrWhiteSpace(Code))
                notification.AddError(nameof(Code), DefaultMessagesErrors.FieldIsRequired(nameof(Code)), "É necessário informar um código para a busca no bureuau");

            if (string.IsNullOrWhiteSpace(ResponseProviderJson))
                notification.AddError(nameof(ResponseProviderJson), DefaultMessagesErrors.FieldIsRequired(nameof(ResponseProviderJson)), "É necessário a resposta do provedor");

            if (string.IsNullOrWhiteSpace(DataBureau))
                notification.AddError(nameof(DataBureau), DefaultMessagesErrors.FieldIsRequired(nameof(DataBureau)), "É necessário a resposta interna");

            if (!EnumUtils.IsValidEnum(ProviderType))
                notification.AddError(nameof(ProviderType), DefaultMessagesErrors.EnumIsInvalid, "É necessário informar um provedor válido");

            if (!EnumUtils.IsValidEnum(BureauType))
                notification.AddError(nameof(BureauType), DefaultMessagesErrors.EnumIsInvalid, "É necessário informar um bureua válido");

            return notification;
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetDeletedAt()
        {
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}
