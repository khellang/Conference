using System.Threading;
using System.Threading.Tasks;
using Conference.Entities;

namespace Conference
{
    public abstract class EventEntityRepository<TEntity, TModel> : CrudRepository<TEntity, TModel, EventQuery>
        where TEntity : EventEntity
        where TModel : EntityModel
    {
        protected EventEntityRepository(ConferenceContext context) : base(context)
        {
        }

        public Task<PageModel<TModel>> GetAllForEvent(EventQuery query, CancellationToken cancellationToken)
        {
            return GetPaged(query, x => x.EventId.Equals(query.EventId), cancellationToken);
        }
    }
}
