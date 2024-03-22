using LaboratoryPanelWPF.Api;
using Refit;
using System;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Utils
{
    public class RetrofitClient
    {
        private static readonly Lazy<RetrofitClient> RETROFIT_CLIENT = new Lazy<RetrofitClient>(() => new RetrofitClient());

        public static RetrofitClient Instance => RETROFIT_CLIENT.Value;


#if DEBUG
        private const string BaseUrl = "https://localhost:7046/api";
#else
        private const string BaseUrl = "http://diagnostic-service.centralindia.cloudapp.azure.com/";
#endif


        private RetrofitClient()
        {
        }

        private readonly RefitSettings _settings = new()
        {
            ContentSerializer = new NewtonsoftJsonContentSerializer(),
            AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(Properties.Settings.Default.Token)
        };

        public T Create<T>()
        {
            return RestService.For<T>(BaseUrl, _settings);
        }

        public IFirebaseAuthApi CreateFirebaseAuthApi()
        {
            return RestService.For<IFirebaseAuthApi>("https://identitytoolkit.googleapis.com", _settings);
        }

    }
}
