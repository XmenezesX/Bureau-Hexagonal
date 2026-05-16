using BureauHexagonal.Core.Common.Operation;

namespace BureauHexagonal.Core.Common.UseCase
{
    public interface IUseCase<TInput>
    {
        Task<IOperation> ExecAsync(TInput input);
    }
}
