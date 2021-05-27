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
      public MainViewModel(ConfigurationViewModel configurationViewModel, // TODO do I need this one if navigationviewmodel needs it
         NavigationViewModel navigationViewModel,
         PlanInformationViewModel planInformationViewModel,//PlaninfoVM must be before plan Nav VM for eventing.
         PlanNavigationViewModel planNavigationViewModel,
         MUCheckViewModel muCheckViewModel)

      {
         NavigationViewModel = navigationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         PlanNavigationViewModel = planNavigationViewModel;
         ConfigurationViewModel = configurationViewModel;
         MUCheckViewModel = muCheckViewModel;

      }

      public NavigationViewModel NavigationViewModel { get; }
      public PlanNavigationViewModel PlanNavigationViewModel { get; }
      public ConfigurationViewModel ConfigurationViewModel { get; }
      public MUCheckViewModel MUCheckViewModel { get; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
   }
}
