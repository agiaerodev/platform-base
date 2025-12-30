using Core.Factory;
using Core.Repositories;
using Idata.Entities.Icomments;
using Icomments.Repositories.Interfaces;

namespace Icomments.Repositories
{
    public class IcommentRepository : RepositoryBase<Icomment>, IIcommentRepository
    {
        public IcommentRepository(RepositoryFactory<Icomment> dependenciesContainer)
        {
            _dependenciesContainer = dependenciesContainer;
        }
    }
}
