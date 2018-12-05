using CVnHR.Business.Kvk;
using CVnHR.Business.Kvk.Api;
using CVnHR.Business.Kvk.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class KvkController : Controller
    {
        private readonly IKvkSearchApi _kvkSearchApi;
        private readonly IHrDataservice _hrDataservice;
        private readonly IHRDataserviceMessageParser _messageParser;

        public KvkController(IKvkSearchApi kvkSearchApi, IHrDataservice hrDataservice, IHRDataserviceMessageParser messageParser)
        {
            _kvkSearchApi = kvkSearchApi;
            _hrDataservice = hrDataservice;
            _messageParser = messageParser;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string q, int startPage = 0)
        {
            var kvkSearchApiParameters = new KvkSearchApiParameters() { Q = q, StartPage = startPage };
            var result = await _kvkSearchApi.Search(kvkSearchApiParameters);
            return Json(result);
        }

        [HttpGet("{kvkNumber}")]
        public async Task<ActionResult> Get(string kvkNumber)
        {
            Console.WriteLine("Searching for kvkNumber: ", kvkNumber);

            var result = await _hrDataservice.GetInschrijvingFromKvK(kvkNumber);
            var response = _messageParser.Parse(result);
            //var activiteit = response.product.maatschappelijkeActiviteit;
            return Json(response);
        }
    }
}
