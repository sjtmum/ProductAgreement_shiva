using AgMg_SandeepTrivedi.Data.Repository;
using AgMg_SandeepTrivedi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace AgMg_SandeepTrivedi.Data
{
    public interface IDbHelper
    {
        ProductGroupRepository ProductGroups { get; }
        Repository<Product> Products { get; }
        Repository<Agreement> Agreements { get; }
        //public T Get<T>(bool onlyActive);
        DbConnection Database { get; }
        void BeginTransaction();
        void Commit();
        void Dispose();
        void Rollback();
        int Save();
    }
    public class DbHelper : IDbHelper, IDisposable
    {
        private readonly ApplicationDbContext dbContext;

        private ProductGroupRepository  productGroups = null;
        private Repository<Product> products = null;
        private Repository<Agreement> agreements = null;

        IDbContextTransaction dbContextTransaction;
        public DbConnection Database => dbContext?.Database.GetDbConnection();
        public ProductGroupRepository ProductGroups => productGroups ??= new ProductGroupRepository(dbContext);
        public Repository<Product> Products => products ??= new Repository<Product>(dbContext);
        public Repository<Agreement> Agreements => agreements ??= new Repository<Agreement>(dbContext);

        public DbHelper(ApplicationDbContext context)
        {
            dbContext = context;
        }
        
        public int Save()
        {
            return dbContext.SaveChanges();
        }
        public void BeginTransaction()
        {
            if (dbContextTransaction == null)
                dbContextTransaction = dbContext.Database.BeginTransaction();
        }
        public void Commit()
        {
            if (dbContextTransaction != null)
                dbContextTransaction.Commit();
        }
        public void Rollback()
        {
            if (dbContextTransaction != null)
                dbContextTransaction.Rollback();
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
