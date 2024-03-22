using System.IO;
using System.Reflection;
using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.ViewModels;
using LaboratoryPanelWPF.ViewModels.TestMaster;
using LaboratoryPanelWPF.Views.Pages;
using LaboratoryPanelWPF.Views.Pages.TestMaster;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Wpf.Ui;

namespace LaboratoryPanelWPF
{


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<LoginWindow>();
                services.AddScoped<AllAppointmentsPage>();
                services.AddScoped<PatientPage>();
                services.AddSingleton<HomePage>();
                services.AddScoped<AddAppointmentPage>();
                services.AddSingleton<CategoriesPage>();
                services.AddTransient<TestGroupsPage>();
                services.AddSingleton<AllTestGroupsPage>();
                services.AddSingleton<SettingsPage>();
                services.AddTransient<AppointmentDetailWindow>();
                services.AddTransient<AddResultWindow>();

                services.AddSingleton<IDBService, DBService>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<IAuthService, AuthService>();
                services.AddSingleton<IAppointmentService, AppointmentService>();
                services.AddSingleton<IPatientService, PatientService>();
                services.AddSingleton<IReferenceService, ReferenceService>();

                services.AddScoped<LoginWindowViewModel>();
                services.AddScoped<MainViewModel>();
                services.AddScoped<AllAppointmentsPageViewModel>();
                services.AddScoped<PatientViewModel>();
                services.AddScoped<AddAppointmentViewModel>();
                services.AddScoped<CategoriesViewModel>();
                services.AddTransient<TestGroupsViewModel>();
                services.AddScoped<AllTestGroupsViewModel>();
                services.AddScoped<SettingsPageViewModel>();
                services.AddScoped<TestGroupInCategoryPage>();
                services.AddScoped<TestGroupInCategoryViewModel>();

            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            var authService = _host.Services.GetService<IAuthService>();

            Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);


            Laboratory? laboratory;

            try
            {
                laboratory = await authService!.GetCurrentUser();
            }
            catch
            {
                laboratory = null;
            }

            if (laboratory == null)
            {
                var loginWindow = _host.Services.GetService<LoginWindow>();
                loginWindow?.Show();
                return;
            }

            var mainWindow = _host.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
