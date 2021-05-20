using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.ViewModels
{
   public class MainViewModel
   {
      public MainViewModel(ConfigurationViewModel configurationViewModel,
         NavigationViewModel navigationViewModel,
         PlanInformationViewModel planInformationViewModel,//PlaninfoVM must be before plan Nav VM for eventing.
         PlanNavigationViewModel planNavigationViewModel)

      {
         NavigationViewModel = navigationViewModel;
         PlanInformationViewModel = planInformationViewModel;
         PlanNavigationViewModel = planNavigationViewModel;
         ConfigurationViewModel = configurationViewModel;
      }

      public NavigationViewModel NavigationViewModel { get; }
      public PlanNavigationViewModel PlanNavigationViewModel { get; }
      public ConfigurationViewModel ConfigurationViewModel { get; }
      public PlanInformationViewModel PlanInformationViewModel { get; }
   }
}
