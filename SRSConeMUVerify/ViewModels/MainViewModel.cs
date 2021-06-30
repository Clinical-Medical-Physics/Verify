using DVHPlot.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.ViewModels
{
   public class MainViewModel
   {
      // TODO where to these get passed in Are they ever passed in
      // TODO add a selected field view
      // TODO add a DVH view
      // TODO add a Print button
      public MainViewModel(
         IEventAggregator eventAggregator,
         NavigationViewModel navigationViewModel,
         PlanInformationViewModel planInformationViewModel,//PlaninfoVM must be before plan Nav VM for eventing.
         PlanNavigationViewModel planNavigationViewModel,
         ConfigurationViewModel configurationViewModel, // TODO do I need this one if navigationviewmodel needs it
         MUCheckViewModel muCheckViewModel,
         DVHSelectionViewModel dVHSelectionViewModel, DVHViewModel dVHViewModel)

      {
         ConfigurationViewModel = configurationViewModel;
         MUCheckViewModel = muCheckViewModel;
         NavigationViewModel = navigationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         PlanNavigationViewModel = planNavigationViewModel;
         
         
         DVHSelectionViewModel = dVHSelectionViewModel;
         DVHViewModel = dVHViewModel;

      }

      public NavigationViewModel NavigationViewModel { get; }
      public PlanNavigationViewModel PlanNavigationViewModel { get; }
      public ConfigurationViewModel ConfigurationViewModel { get; }
      public MUCheckViewModel MUCheckViewModel { get; }
      public DVHSelectionViewModel DVHSelectionViewModel { get; }
      public DVHViewModel DVHViewModel { get; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
   }
}
