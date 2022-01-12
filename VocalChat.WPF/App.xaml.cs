using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VocalChat.WPF.ViewModels;
using VocalChat.WPF.Views;

namespace VocalChat.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<HomeView>();

            services.AddScoped<HomeViewModel>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(e);
            HomeView? homeView = _serviceProvider.GetService<HomeView>();
            if (homeView is null) throw new Exception("Application cannot start!");
            homeView.DataContext = _serviceProvider.GetService<HomeViewModel>();
            homeView.Show();
        }
    }
}
