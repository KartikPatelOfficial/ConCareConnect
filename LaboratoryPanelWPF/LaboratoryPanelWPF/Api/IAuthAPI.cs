using LaboratoryPanelWPF.Model;
using Refit;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Api
{
    public interface IAuthApi
    {

        [Headers("Authorization: Bearer")]
        [Get("/laboratories/login")]
        Task<Laboratory> Login();

    }
}
