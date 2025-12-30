using Core;
using Idata.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Icomments.Repositories.Interfaces;
using Icomments.Services.Interfaces;

namespace Icomments.Services
{
    public class IcommentService : IIcommentService
    {        
        public IIcommentRepository _icommentsRepository;
        public IdataContext _dataContext;
        public IcommentService(IdataContext dataContext, IIcommentRepository icommentsRepository)
        {
            _icommentsRepository = icommentsRepository;
            _dataContext = dataContext;
        }

              
       

    }
}
