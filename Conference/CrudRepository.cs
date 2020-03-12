using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Conference.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conference
{
    public abstract class CrudRepository<TEntity, TModel> : CrudRepository<TEntity, TModel, PagedQuery>
        where TEntity : class, IEntity
        where TModel : EntityModel
    {
        protected CrudRepository(ConferenceContext context) : base(context)
        {
        }
    }

    public abstract class CrudRepository<TEntity, TModel, TQuery>
        where TEntity : class, IEntity
        where TModel : EntityModel
        where TQuery : PagedQuery
    {
        protected CrudRepository(ConferenceContext context)
        {
            Context = context;
        }

        public DbSet<TEntity> DbSet => Context.Set<TEntity>();

        protected ConferenceContext Context { get; }

        public Task<PageModel<TModel>> GetPaged(TQuery query, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return GetPaged(Filter(DbSet, query).Where(predicate), query, cancellationToken);
        }

        public Task<PageModel<TModel>> GetPaged(TQuery query, CancellationToken cancellationToken)
        {
            return GetPaged(Filter(DbSet, query), query, cancellationToken);
        }

        public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        {
            return DbSet.AnyAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public virtual Task<TModel> GetById(Guid id, CancellationToken cancellationToken)
        {
            return MapInclude(DbSet).SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public virtual async Task<TModel> Create(TEntity entity, CancellationToken cancellationToken)
        {
            await Context.AddAsync(entity, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);

            return Map(entity);
        }

        public Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            Context.Attach(entity).State = EntityState.Modified;

            return Context.SaveChangesAsync(cancellationToken);
        }

        protected virtual IQueryable<TEntity> Filter(DbSet<TEntity> source, TQuery query)
        {
            return source;
        }

        protected virtual IOrderedQueryable<TEntity> OrderBy(IQueryable<TEntity> source, string? field)
        {
            return source.OrderBy(x => x.Id);
        }

        protected virtual IQueryable<TEntity> Include(IQueryable<TEntity> source)
        {
            return source;
        }

        protected abstract IQueryable<TModel> Map(IQueryable<TEntity> query);

        private Task<PageModel<TModel>> GetPaged(IQueryable<TEntity> source, TQuery query, CancellationToken cancellationToken)
        {
            return MapInclude(OrderBy(source, query.OrderBy)).PagedAsync(query, cancellationToken);
        }

        private IQueryable<TModel> MapInclude(IQueryable<TEntity> source) => Map(Include(source));

        private TModel Map(TEntity entity) => Map(new[] { entity }.AsQueryable()).Single();
    }
}
