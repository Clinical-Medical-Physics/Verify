using Prism.Commands;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
         
         MessageBoxResult userAnswer = new MessageBoxResult();
         if (ConfigurationViewModel.IsConfigured)
         {
            userAnswer = MessageBox.Show("SRS Cone MU Verify is already configured! Do you want to Continue?",
               "WARNING Already Configured",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning);
            if (userAnswer.ToString() == "Yes")
            {
               var configView = new ConfigurationView();
               configView.DataContext = new ConfigurationViewModel(new AppConfigModel());
               configView.ShowDialog();
            }
         }
         else
         {
            var configView = new ConfigurationView();
            configView.DataContext = new ConfigurationViewModel(new AppConfigModel());
            configView.ShowDialog();
         }
         
      }
   }
}
