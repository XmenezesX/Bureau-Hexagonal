using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Entities.Base;
using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities;
using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities.BaseInfraPostgres;
using System.Collections.Concurrent;

namespace BureauHexagonal.Infrastructure.DataBase.Postgres.Map
{
    public static class DomainToDataBaseMap
    {
        private static IDictionary<Type, Func<BaseEntity, BaseInfraEntityPostgres>> _map = new ConcurrentDictionary<Type, Func<BaseEntity, BaseInfraEntityPostgres>>();

        static DomainToDataBaseMap()
        {
            _map.Add(typeof(BureauEntity), (domainEntity) =>
            {
                var bureau = (BureauEntity)domainEntity;

                return new BureauInfraEntity
                {
                    Id = bureau.Id,
                    Code = bureau.Code,
                    ProviderType = bureau.ProviderType.GetHashCode(), 
                    BureauType = bureau.BureauType.GetHashCode(),
                    ProviderTypeDescription = bureau.ProviderTypeDescription,
                    BureauTypeDescription = bureau.BureauTypeDescription,
                    ResponseProviderJson = bureau.ResponseProviderJson,
                    DataBureau = bureau.DataBureau,
                    Synchronized = bureau.Synchronized,
                    CreatedAt = bureau.CreatedAt,
                    UpdatedAt = bureau.UpdatedAt,
                    DeletedAt = bureau.DeletedAt
                };
            });
        }

        public static T Map<T>(BaseEntity baseEntity) where T : BaseInfraEntityPostgres
        {
            return (T)_map[baseEntity.GetType()](baseEntity);
        }
    }
}
