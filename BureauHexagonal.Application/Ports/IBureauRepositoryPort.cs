using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Enums;

namespace BureauHexagonal.Core.Ports
{
    public interface IBureauRepositoryPort
    {
        Task<IOperation<BureauEntity>> GetByCodeAsync(string code);
        Task<IOperation<BureauEntity>> GetByCodeAndProviderAsync(string code, ProviderType providerType);
        Task<IOperation<IEnumerable<BureauEntity>>> GetByProviderAsync(ProviderType providerType);
        Task<IOperation<IEnumerable<BureauEntity>>> GetByBureautypeAsync(BureauType bureauType);
        Task<IOperation> CreateAsync(BureauEntity entity);
        Task<IOperation> DeleteAsync(Guid id);
    }
}
