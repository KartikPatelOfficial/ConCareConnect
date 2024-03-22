using LaboratoryPanelWPF.Model;
using Refit;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Api;

public interface IFirebaseAuthApi
{
    [Post("/v1/accounts:signInWithPassword?key=AIzaSyA06mg-S45qgypPpjKO-APdzHylw5SSBa8")]
    Task<FirebaseLoginResponse> FirebaseLogin([Body] FirebaseLoginRequest request);

    [Post("/v1/token?key=AIzaSyA06mg-S45qgypPpjKO-APdzHylw5SSBa8")]
    Task<FirebaseRefreshTokenResponse> FirebaseRefreshToken(FirebaseRefreshTokenRequest firebaseRefreshTokenRequest);
}