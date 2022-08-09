using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EgepakErp.Concrete;
using EgePakErp.Custom;
using EgePakErp.Helper;

namespace EgePakErp.Controllers
{
    public class GenericBaseController<T> : BaseController where T : class
    {
        public _GenericRepository<T> BaseRepo { get; set; }
        public GenericBaseController()
        {
            BaseRepo = new _GenericRepository<T>();
        }

        public IQueryable<T> IncludeAllNavigationProperties<T>(IQueryable<T> queryable)
        {
            if (queryable == null)
                throw new ArgumentNullException("queryable");

            ObjectContext objectContext = ((IObjectContextAdapter)Db).ObjectContext;
            var metadataWorkspace = ((EntityConnection)objectContext.Connection).GetMetadataWorkspace();

            EntitySetMapping[] entitySetMappingCollection = metadataWorkspace.GetItems<EntityContainerMapping>(DataSpace.CSSpace).Single().EntitySetMappings.ToArray();

            var entitySetMappings = entitySetMappingCollection.First(o => o.EntityTypeMappings.Select(e => e.EntityType.Name).Contains(typeof(T).Name));

            var entityTypeMapping = entitySetMappings.EntityTypeMappings[0];

            foreach (var navigationProperty in entityTypeMapping.EntityType.NavigationProperties)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(navigationProperty.Name);
                if (propertyInfo == null)
                    throw new InvalidOperationException("propertyInfo == null");

                if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                }
                    

                queryable = queryable.Include(navigationProperty.Name);
            }

            return queryable;
        }
    }
}