using Autofac;
using Prism.Events;
using SRSConeMUVerify.Models;
using SRSConeMUVerify.ViewModels;
using SRSConeMUVerify.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace SRSConeMUVerify.Startup
{
   public class Bootstrapper
   {
      public IContainer Bootstrap(Patient patient, Course course, PlanSetup plan)
      {
         var container = new ContainerBuilder();
         if (patient != null)
         {
            container.RegisterInstance<Patient>(patient);
         }
         if (course != null)
         {
            container.RegisterInstance<Course>(course);
         }
         if (plan != null)
         {
            container.RegisterInstance<PlanSetup>(plan);
         }
         container.RegisterType<AppConfigModel>();
         container.RegisterType<ConfigurationViewModel>();

         container.RegisterType<MainViewModel>();
         container.RegisterType<MainView>();
         container.RegisterType<NavigationViewModel>();
         
         container.RegisterType<PlanNavigationViewModel>();
         container.RegisterType<PlanInformationViewModel>();
         container.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
         return container.Build();
      }
   }
}
