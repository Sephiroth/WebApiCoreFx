using AspectCore.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace AopDLL.AspectCore
{
    public class TransactionInterceptorAttribute : AbstractInterceptorAttribute
    {
        public TransactionInterceptorAttribute(DbContext dbContext)
        {
            _dbContextType = dbContext.GetType();
        }

        private Type _dbContextType;

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            DbContext dbContext = context.ServiceProvider.GetService(_dbContextType) as DbContext;
            if (dbContext.Database.CurrentTransaction == null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await next(context);
                    await transaction.CommitAsync();
                    //dbContext.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    //dbContext.Database.RollbackTransaction();
                    throw ex;
                }
            }
            else
            {
                await next(context);
            }
        }

    }
}