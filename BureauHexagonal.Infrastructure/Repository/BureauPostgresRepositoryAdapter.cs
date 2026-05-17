using BureauHexagonal.Core.Common.NotificationError;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Ports;
using BureauHexagonal.Core.Utils;
using BureauHexagonal.Infrastructure.DataBase.Postgres;
using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities;
using BureauHexagonal.Infrastructure.DataBase.Postgres.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BureauHexagonal.Infrastructure.Repository
{
    public sealed class BureauPostgresRepositoryAdapter(ILogger<BureauPostgresRepositoryAdapter> _logger,
                                                        BureauPostgresDbContext dbContext) : IBureauRepositoryPort
    {
        public async Task<IOperation> CreateAsync(BureauEntity entity)
        {
            try
            {
                var bureauInfra = DomainToDataBaseMap.Map<BureauInfraEntity>(entity);

                await dbContext.Bureau.AddAsync(bureauInfra);

                return OperationFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return OperationFactory.CreateFail(ex);
            }
        }

        public async Task<IOperation> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IOperation<IEnumerable<BureauEntity>>> GetByBureautypeAsync(BureauType bureauType)
        {
            var result = await dbContext.Bureau
                                    .AsNoTracking()
                                    .Where(x => x.BureauType == bureauType.GetHashCode() &&
                                                x.DeletedAt == null)
                                    .ToListAsync();

            var response = result.Select(x => DataBaseToDomainMap.Map<BureauEntity>(x));

            return response.ToSuccess();
        }

        public async Task<IOperation<BureauEntity>> GetByCodeAsync(string code)
        {
            string codeOnlyNumbers = code.OnlyNumbers();
            var result = await dbContext.Bureau
                            .AsNoTracking()
                            .Where(x => x.Code == codeOnlyNumbers &&
                                        x.DeletedAt == null)
                            .FirstOrDefaultAsync();

            if (result is null)
            {
                var notification = NotificationErrors.Create(nameof(result), DefaultMessagesErrors.EntityNotFoundMessage);
                return notification.ToFail<BureauEntity>(ErrorType.EntityNotFound);
            }

            var response = DataBaseToDomainMap.Map<BureauEntity>(result);

            return response.ToSuccess();
        }

        public async Task<IOperation<IEnumerable<BureauEntity>>> GetByProviderAsync(ProviderType providerType)
        {
            var result = await dbContext.Bureau
                                    .AsNoTracking()
                                    .Where(x => x.ProviderType == providerType.GetHashCode() &&
                                                x.DeletedAt == null)
                                    .ToListAsync();

            var response = result.Select(x => DataBaseToDomainMap.Map<BureauEntity>(x));

            return response.ToSuccess();
        }
    }
}
