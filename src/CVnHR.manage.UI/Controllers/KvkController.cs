using CVnHR.Business.Kvk.Api;
using CVnHR.Business.Kvk.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class KvkController : Controller
    {
        private readonly IKvkSearchApi _kvkSearchApi;

        public KvkController(IKvkSearchApi kvkSearchApi)
        {
            _kvkSearchApi = kvkSearchApi;
        }

        [HttpGet()]
        public async Task<ActionResult> Get(string q, int startPage = 0)
        {
            var kvkSearchApiParameters = new KvkSearchApiParameters() { Q = q, StartPage = startPage };
            var result = await _kvkSearchApi.Search(kvkSearchApiParameters);
            return Json(result);
        }
    }
}
