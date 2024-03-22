using LaboratoryPanelWPF.Model;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public interface IAuthService
    {
        public Task<Laboratory?> Login(string email, string password);

        public Task<Laboratory?> GetCurrentUser();

        public void Logout();

    }
}
