using Core.Controllers;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Icomments.Repositories.Interfaces;
using Idata.Entities.Icomments;

namespace Icomments.Controllers
{
    [Authorize]
    [Route("api/icomments/v1/icomments")]
    [ApiController]
    public class IcommentController : ControllerBase<Icomment>
    {
        public IcommentController(IIcommentRepository repositoryBase, IHttpContextAccessor currentContext, IBackgroundJobClient backgroundJobClient) : base(repositoryBase)
        {

        }

    }
}