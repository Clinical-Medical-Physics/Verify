using Prism.Commands;
using Prism.Events;
using SRSConeMUVerify.Events;
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
      public IEventAggregator _eventAggregator;
      public ConfigurationView configurationView { get; set; }
      
      public NavigationViewModel(ConfigurationViewModel configurationViewModel, IEventAggregator eventAggregator)
      {
         ConfigurationViewModel = configurationViewModel;
         LaunchConfigurationCommand = new DelegateCommand(OnLaunchConfiguration);
         
         _eventAggregator = eventAggregator;
         _eventAggregator.GetEvent<ConfigViewCloseEvent>().Subscribe(Closer);
         if(ConfigurationViewModel.IsConfigured == false)
         {
            MessageBox.Show("First Run...Opening Configuration!");
            OnLaunchConfiguration();
         }
      }

      private void OnLaunchConfiguration()
      {
         bool continueAnyway = false;
         MessageBoxResult userAnswer = new MessageBoxResult();
         if (ConfigurationViewModel.IsConfigured)
         {
            userAnswer = MessageBox.Show("SRS Cone MU Verify is already configured! Do you want to Continue?",
               "WARNING Already Configured",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning);
            if (userAnswer.ToString() == "Yes")
            {
               continueAnyway = true;
            }
         }
         else
         {
            continueAnyway = true;
         }
         if (continueAnyway)
         {
            configurationView = new ConfigurationView();
            configurationView.DataContext = ConfigurationViewModel;
            configurationView.ShowDialog();
         }
      }
      public void Closer(bool close)
      {
         configurationView.Close();
         configurationView = null;
      }
   }
}
