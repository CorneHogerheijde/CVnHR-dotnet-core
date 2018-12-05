using CVnHR.Business.Kvk.Api.Entities;
using System.Threading.Tasks;

namespace CVnHR.Business.Kvk.Api
{
    public interface IKvkSearchApi
    {
        Task<KvkSearchApiResult> Search(KvkSearchApiParameters parameters);
    }
}
