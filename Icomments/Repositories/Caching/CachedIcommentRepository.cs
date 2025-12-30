using Core.Repositories;
using Ihelpers.Interfaces;
using Icomments.Repositories.Interfaces;
using Idata.Entities.Icomments;

namespace Icomments.Repositories.Caching
{
    public class CachedIcommentRepository : CacheRepositoryBase<Icomment>, IIcommentRepository
    {        
        public CachedIcommentRepository(IIcommentRepository repositoryBase, ICacheBase cacheBase) : base(repositoryBase, cacheBase)
        {
            
        }   
    }
}