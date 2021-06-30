using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SRSConeMUVerify.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SRSConeMUVerify.ViewModels
{
   public class MessageViewModel : BindableBase
   {
      private string _message;

      public string Message
      {
         get { return _message; }
         set { SetProperty (ref _message ,value); }
      }

      private string _messagebtn1;

      public string MessageButton1
      {
         get { return _messagebtn1; }
         set { SetProperty(ref _messagebtn1, value); }
      }
      private string _messagebtn2;

      public string MessageButton2
      {
         get { return _messagebtn2; }
         set { SetProperty(ref _messagebtn2, value); }
      }
      public bool OnRequestClose { get; set; }
      public string OnRequestOkay { get; set; }
      public DelegateCommand MessageOkPressed { get; private set;}
      public DelegateCommand MessageClosePressed { get; private set; }
      private IEventAggregator _eventAggregator { get; set; }
      public MessageViewModel(IEventAggregator eventAggregator)
      {
         Message = "Your Message Here";
         MessageButton1 = "OK";
         MessageButton2 = "Cancel";
         OnRequestClose = false;
         _eventAggregator = eventAggregator;
         MessageClosePressed = new DelegateCommand(OnMesageClosePressed);
         MessageOkPressed = new DelegateCommand(OnMessageOkPressed);
      }

      private void OnMessageOkPressed()
      {
         //MessageBox.Show("You pressed okay");
         OnRequestOkay = MessageButton1;
         OnRequestClose = true;
         _eventAggregator.GetEvent<MessageViewOkEvent>().Publish(OnRequestOkay);
         _eventAggregator.GetEvent<MessageViewCloseEvent>().Publish(OnRequestClose);
      }

      private void OnMesageClosePressed()
      {
         //MessageBox.Show("You Pressed close");
         OnRequestClose = true;
         _eventAggregator.GetEvent<MessageViewCloseEvent>().Publish(OnRequestClose);
      }
   }
}
