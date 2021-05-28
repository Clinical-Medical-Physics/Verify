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
using MahApps.Metro.Controls.Dialogs;

namespace SRSConeMUVerify.ViewModels
{
   public class NavigationViewModel
   {
      public ConfigurationViewModel ConfigurationViewModel { get; set; }
      public DelegateCommand LaunchConfigurationCommand { get; private set; }
      public IEventAggregator _eventAggregator;
      public ConfigurationView configurationView { get; set; }
      public MessageView mv { get; set; }
      public MessageViewModel mvm { get; set; }
      public NavigationViewModel(ConfigurationViewModel configurationViewModel, IEventAggregator eventAggregator)
      {
         ConfigurationViewModel = configurationViewModel;
         LaunchConfigurationCommand = new DelegateCommand(OnLaunchConfiguration);

         _eventAggregator = eventAggregator;
         mvm = new MessageViewModel(_eventAggregator);
         _eventAggregator.GetEvent<ConfigViewCloseEvent>().Subscribe(Closer);
         _eventAggregator.GetEvent<MessageViewCloseEvent>().Subscribe(MessageCloser);
         if (ConfigurationViewModel.IsConfigured == false)
         {
            ShowMessage("First Run...Opening Configuration", "Continue", "Cancel");
            if (mvm.OnRequestOkay == "Continue")
            {
               OnLaunchConfiguration();
            }
            else
            {
               App.Current.Shutdown();
            }
            
         }
      }



      private void MessageCloser(bool obj)
      {
         mv.Close();
         mv = null;
      }

      private void ShowMessage(string message, string btn1, string btn2)
      {
         mv = new MessageView();
         mvm = new MessageViewModel(_eventAggregator);
         mvm.Message = message;
         mvm.MessageButton1 = btn1;
         mvm.MessageButton2 = btn2;
         mv.DataContext = mvm;
         mv.ShowDialog();
      }
      private void OnLaunchConfiguration()
      {
         bool continueAnyway = false;

         //MessageBoxResult userAnswer = new MessageBoxResult();
         if (ConfigurationViewModel.IsConfigured)
         {
            //userAnswer = MessageBox.Show("SRS Cone MU Verify is already configured! Do you want to Continue?",
            //   "WARNING Already Configured",
            //   MessageBoxButton.YesNo,
            //   MessageBoxImage.Warning);
            // use custom message window
            ShowMessage("SRS Cone MU Verify is already configured!\nDo you want to Continue?", "Continue", "Cancel");
            if (mvm.OnRequestOkay == "Continue")
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
