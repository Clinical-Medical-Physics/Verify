using Prism.Commands;
using SRSConeMUVerify.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.ViewModels
{
    public class NavigationViewModel
    {
        public ConfigurationViewModel ConfigurationViewModel { get; set; }
        public DelegateCommand LaunchConfigurationCommand { get; private set; }
        public NavigationViewModel(ConfigurationViewModel configurationViewModel)
        {
            ConfigurationViewModel = configurationViewModel;
            LaunchConfigurationCommand = new DelegateCommand(OnLaunchConfiguration);
        }

        private void OnLaunchConfiguration()
        {
            var configView = new ConfigurationView();
            configView.DataContext = ConfigurationViewModel;
            configView.ShowDialog();
        }
    }
}
