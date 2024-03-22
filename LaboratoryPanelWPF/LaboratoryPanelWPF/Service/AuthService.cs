using LaboratoryPanelWPF.Api;
using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Utils;
using System;
using System.Threading.Tasks;

namespace LaboratoryPanelWPF.Service
{
    public class AuthService : IAuthService
    {

        private Laboratory? _laboratory;
        private readonly IFirebaseAuthApi _firebaseAuthAPI = RetrofitClient.Instance.CreateFirebaseAuthApi();
        private readonly IAuthApi _authAPI = RetrofitClient.Instance.Create<IAuthApi>();

        public async Task<Laboratory> GetCurrentUser()
        {
            if (_laboratory == null)
            {
                try
                {
                    _laboratory = await _authAPI.Login();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (_laboratory == null)
                {
                    await RefreshToken();
                    _laboratory = await _authAPI.Login();
                }
            }
            return _laboratory;
        }

        public async Task<Laboratory?> Login(string email, string password)
        {

            FirebaseLoginResponse firebaseLoginResponse;
            try
            {
                var firebaseLoginRequest = new FirebaseLoginRequest() { Email = email, Password = password };

                firebaseLoginResponse = await _firebaseAuthAPI.FirebaseLogin(firebaseLoginRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("Firebase login failed", ex);
            }

            Properties.Settings.Default.Token = firebaseLoginResponse.IdToken;
            Properties.Settings.Default.RefreshToken = firebaseLoginResponse.RefreshToken;
            Properties.Settings.Default.Save();

            try
            {
                _laboratory = await _authAPI.Login();
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed", ex);
            }
            return _laboratory;
        }

        public void Logout()
        {

            Properties.Settings.Default.Token = "";
            Properties.Settings.Default.RefreshToken = "";
            Properties.Settings.Default.Save();

            _laboratory = null;
        }

        private async Task RefreshToken()
        {
            var refreshToken = Properties.Settings.Default.RefreshToken;

            if (string.IsNullOrEmpty(refreshToken)) throw new Exception("Refresh token is empty");

            try
            {
                var firebaseRefreshTokenRequest = new FirebaseRefreshTokenRequest(refreshToken);

                var firebaseRefreshTokenResponse = await _firebaseAuthAPI.FirebaseRefreshToken(firebaseRefreshTokenRequest) ?? throw new Exception("Firebase refresh token failed");

                Properties.Settings.Default.Token = firebaseRefreshTokenResponse.AccessToken;
                Properties.Settings.Default.RefreshToken = firebaseRefreshTokenResponse.RefreshToken;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Firebase refresh token failed", ex);
            }

        }

    }
}
