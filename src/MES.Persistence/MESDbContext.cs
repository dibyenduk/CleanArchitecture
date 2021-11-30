using MES.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Persistence
{
    public class MESDbContext : DbContext
    {
        private DbContextTransaction currentTransaction;
        private readonly bool clearPoolOnException;
        public const int FatalSqlErrorStartingClass = 16;

        private readonly bool isTestGenerated;

        public MESDbContext() : base("MESDatabase")
        {
        }

        public MESDbContext(DbConnection connection) : base(connection, false)
        {
            isTestGenerated = true;
        }

        public DbSet<ProcessOrder> ProcessOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if(!isTestGenerated)
                Database.SetInitializer<MESDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Types().Configure(
               c =>
                   c.ToTable(
                       string.Format("t{0}", c.ClrType.Name), "dbo" ));
        }

        public void BeginTransaction()
        {
            if (this.currentTransaction != null)
            {
                return;
            }

            this.currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public DbContextTransaction GetCurrentTransaction()
        {
            return this.currentTransaction;
        }

        public void CloseTransaction()
        {
            CloseTransaction(exception: null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    // Ensure closed transaction
                    SafeRollbackAndDispose(true, exception);
                    return;
                }

                SaveChanges();

                if (this.currentTransaction != null)
                {
                    this.currentTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                SafeRollbackAndDispose(true, ex);
                throw;
            }
            finally
            {
                if (this.currentTransaction != null)
                {
                    this.currentTransaction.Dispose();
                    this.currentTransaction = null;
                }
            }
        }

        private void SafeRollbackAndDispose(bool isError, Exception ex)
        {
            try
            {
                if (CanRollback())
                {
                    SqlException sqlEx = ex as SqlException;

                    if (isError && ShouldClearPoolOnException(sqlEx))
                    {
                        //Log.Warning(ex,
                        //    "A Fatal Sql exception (Class >= {0}) occurred so clearing the DB Connection Pool."
                        //        .ToFormat(FatalSqlErrorStartingClass));

                        SqlConnection sqlConnection = currentTransaction.UnderlyingTransaction.Connection as SqlConnection;
                        SqlConnection.ClearPool(sqlConnection);
                    }

                    currentTransaction.Rollback();
                }
            }

            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                }
                currentTransaction = null;
            }
        }

        private bool ShouldClearPoolOnException(SqlException sqlEx)
        {
            if (!clearPoolOnException || null == sqlEx)
            {
                return false;
            }

            return sqlEx.Errors.OfType<SqlError>()
                        .Any(e => e != null &&
                                  Convert.ToInt16(e.Class) >= FatalSqlErrorStartingClass);
        }

        private bool CanRollback()
        {
            return this.currentTransaction != null && this.currentTransaction.UnderlyingTransaction.Connection != null;
        }

    }
}
