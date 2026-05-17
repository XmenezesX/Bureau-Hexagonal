using BureauHexagonal.Core.Entities;
using BureauHexagonal.Core.Entities.Base;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities;
using BureauHexagonal.Infrastructure.DataBase.Postgres.Entities.BaseInfraPostgres;
using System.Collections.Concurrent;

namespace BureauHexagonal.Infrastructure.DataBase.Postgres.Map
{
    public static class DataBaseToDomainMap
    {
        private static IDictionary<Type, Func<BaseInfraEntityPostgres, BaseEntity>> _map = new ConcurrentDictionary<Type, Func<BaseInfraEntityPostgres, BaseEntity>>();

        static DataBaseToDomainMap()
        {
            _map.Add(typeof(BureauInfraEntity), (baseInfraEntity) =>
            {
                var infraEntity = baseInfraEntity as BureauInfraEntity;
                return BureauEntity.Restore(
                    infraEntity!.Id,
                    infraEntity.Code,
                    (ProviderType)infraEntity.ProviderType,
                    (BureauType)infraEntity.BureauType,
                    infraEntity.ResponseProviderJson,
                    infraEntity.DataBureau,
                    infraEntity.Synchronized,
                    infraEntity.CreatedAt,
                    infraEntity.UpdatedAt,
                    infraEntity.DeletedAt
                );
            });
        }

        public static T Map<T>(BaseInfraEntityPostgres baseInfraEntity) where T : BaseEntity
        {
            return (T)_map[baseInfraEntity.GetType()](baseInfraEntity);
        }
    }
}
