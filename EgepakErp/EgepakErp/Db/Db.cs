using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EgePakErp.Custom;

namespace EgePakErp
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using EgePakErp.Migrations;
    using EgePakErp.Models;
    using EgePakErp.Custom;
    using System.Data.Entity.Infrastructure;
    using System.ComponentModel.DataAnnotations;
    using EgePakErp.Models.Audit;
    using Newtonsoft.Json;

    public partial class Db : DbContext
    {

        public virtual DbSet<Personel> Personel { get; set; }
        public virtual DbSet<PersonelTip> PersonelTip { get; set; }
        public virtual DbSet<Departman> Departman { get; set; }
        public virtual DbSet<PersonelKisitlamaRelation> PersonelKisitlamaRelation { get; set; }
        public virtual DbSet<Kisitlama> Kisitlama { get; set; }
        public virtual DbSet<KisitlamaControllerAction> KisitlamaControllerAction { get; set; }
        public virtual DbSet<ArayuzKisitlama> ArayuzKisitlama { get; set; }
        public virtual DbSet<PersonelArayuzKisitlama> PersonelArayuzKisitlama { get; set; }
        public virtual DbSet<Cari> Cari { get; set; }
        public virtual DbSet<Kisi> Kisi { get; set; }
        public virtual DbSet<CariGrup> CariGrup { get; set; }
        public virtual DbSet<Ulke> Ulke { get; set; }
        public virtual DbSet<Il> Il { get; set; }
        public virtual DbSet<Ilce> Ilce { get; set; }
        public virtual DbSet<Gorusme> Gorusme { get; set; }
        public virtual DbSet<GorusmeTip> GorusmeTip { get; set; }
        public virtual DbSet<Not> Not { get; set; }
        public virtual DbSet<Hatirlatici> Hatirlatici { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<HamUrunGrup> HamUrunGrup { get; set; }

        #region Custom
        public virtual DbSet<HammaddeCinsi> HammaddeCinsi { get; set; }
        public virtual DbSet<Urun> Urun { get; set; }
        public virtual DbSet<UrunCinsi> UrunCinsi{ get; set; }
        public virtual DbSet<Kalip> Kalip{ get; set; }
        public virtual DbSet<KalipHammaddeRelation> KalipHammaddeRelation { get; set; }
        public virtual DbSet<KalipUrunRelation> KalipUrunRelation { get; set; }
        public virtual DbSet<UretimTeminSekli> UretimTeminSekli { get; set; }

        #endregion

        public Db() : base("name=Model1")
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Db, Configuration>("Model1"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

        public void Update<T>(T entity, params string[] exlude) where T : class
        {
            var entry = Entry<T>(entity);
            entry.State = EntityState.Modified;
            if (exlude != null)
            {
                foreach (var name in exlude)
                {
                    if (string.IsNullOrEmpty(name)) continue;
                    entry.Property(name.Trim()).IsModified = false;
                }
            }
        }
        public void Update<T>(T entity, List<string> include ) where T : class
        {


            var entry = Entry<T>(entity);
            entry.State = EntityState.Modified;
            if (include.Count > 0)
            {
                foreach (var name in entry.CurrentValues.PropertyNames)
                {
                    if (include.Contains(name))
                    {
                        entry.Property(name.Trim()).IsModified = true;
                    }
                    else
                    {
                        entry.Property(name.Trim()).IsModified = false;
                    }

                }
            }
        }
        public void Update<T>(T entity, List<string> include,string key) where T : class
        {

            //context.Blogs.Attach(existingBlog);

            var entry = Entry<T>(entity);
            entry.State = EntityState.Modified;
            if (include.Count > 0)
            {
                foreach (var name in entry.CurrentValues.PropertyNames)
                {
                    if (name == key) continue;
                    if (include.Contains(name))
                    {
                        entry.Property(name.Trim()).IsModified = true;
                    }
                    else
                    {
                        entry.Property(name.Trim()).IsModified = false;
                    }

                }
            }
        }
        public void Update<T>(T entity, TypeOfEditEntityProperty typeOfEditEntityProperty, params string[] properties)
        {
            if (typeOfEditEntityProperty == TypeOfEditEntityProperty.Exclude)
            {
                foreach (var item in entity.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty))
                {
                    if (!item.CanRead || !item.CanWrite)
                        continue;
                    if (properties.Contains(item.Name))
                        continue;
                    item.SetValue(entity, item.GetValue(entity, null), null);
                }
            }
            else if (typeOfEditEntityProperty == TypeOfEditEntityProperty.Include)
            {
                foreach (var item in entity.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty))
                {
                    if (!item.CanRead || !item.CanWrite)
                        continue;
                    if (!properties.Contains(item.Name))
                        continue;
                    item.SetValue(entity, item.GetValue(entity, null), null);
                }
            }


        }
        public void Insert<T>(T entity) where T : class
        {
            Set<T>().Add(entity);

        }
        public void Delete<T>(T entity) where T : class
        {
            if (entity != null)
            {
                Set<T>().Remove(entity);

            }
        }
        public int SaveChanges(int personelId)
        {
            var referance = Guid.NewGuid();
            // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                // For each changed record, get the audit record entries and add them
                foreach (AuditLog x in GetAuditRecordsForChange(ent, personelId))
                {
                    x.Referance = referance;
                    this.AuditLog.Add(x);
                }
            }

            // Call the original SaveChanges(), which will save both the changes made and the audit records
            return base.SaveChanges();
        }
        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, int userId)
        {
            List<AuditLog> result = new List<AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute, if one exists
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

            if (dbEntry.State == EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                result.Add(
                    new AuditLog()
                    {
                        AuditLogID = Guid.NewGuid(),
                        PersonelId = userId,
                        EventDateUTC = changeTime,
                        EventType = (int)EventType.Yeni, // Added
                        TableName = tableName,
                        RecordID = dbEntry.CurrentValues.GetValue<int>(keyName),  // Again, adjust this if you have a multi-column key
                        ColumnName = "*ALL",    // Or make it nullable, whatever you want
                        NewJson = JsonConvert.SerializeObject(dbEntry.CurrentValues.ToObject()),
                        OriginalJson = ""
                    }
                    );
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                result.Add(
                    new AuditLog()
                    {
                        AuditLogID = Guid.NewGuid(),
                        PersonelId = userId,
                        EventDateUTC = changeTime,
                        EventType = (int)EventType.Silme, // Added
                        TableName = tableName,
                        RecordID = dbEntry.OriginalValues.GetValue<int>(keyName),
                        ColumnName = "*ALL",
                        NewJson ="", //JsonConvert.SerializeObject(dbEntry.CurrentValues.ToObject()),
                        OriginalJson = JsonConvert.SerializeObject(dbEntry.OriginalValues.ToObject())
                    }
                    );
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                var columnNames = new List<string>();
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
                   
                    // For updates, we only want to capture the columns that actually changed
                    if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        columnNames.Add(propertyName);
                    }
                    

                }
                result.Add(new AuditLog()
                {
                    AuditLogID = Guid.NewGuid(),
                    PersonelId = userId,
                    EventDateUTC = changeTime,
                    EventType = (int)EventType.Guncelleme, // Added
                    TableName = tableName,
                    RecordID = dbEntry.OriginalValues.GetValue<int>(keyName),
                    ColumnName = columnNames.Aggregate((a, b) => a + "," + b),
                    NewJson = JsonConvert.SerializeObject(dbEntry.CurrentValues.ToObject()),
                    OriginalJson = JsonConvert.SerializeObject(dbEntry.OriginalValues.ToObject())
                }
                            );
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities

            return result;
        }

    }


}