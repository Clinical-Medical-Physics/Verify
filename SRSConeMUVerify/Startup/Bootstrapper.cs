﻿using Autofac;
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
         // TODO do I need these three or would they get autogenerated if called by others below it
         container.RegisterType<PlanPrescriptionModel>().SingleInstance();
         container.RegisterType<MUCheckViewModel>();
         container.RegisterType<AppConfigModel>();
         // TODO make this a single instance?
         container.RegisterType<ConfiguredModel>();
         container.RegisterType<ConfigurationViewModel>().SingleInstance();

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
