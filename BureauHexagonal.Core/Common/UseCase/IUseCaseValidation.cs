using BureauHexagonal.Core.Common.NotificationError;

namespace BureauHexagonal.Core.Common.UseCase
{
    public interface IUseCasevalidation<TInput>
    {
        NotificationErrors Validate(TInput input);
    }
}
