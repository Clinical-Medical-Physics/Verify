using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSConeMUVerify.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSConeMUVerify.Utilities.Tests
{
   [TestClass()]
   public class CalculationsTests
   {
      [TestMethod()]
      public void LinearInterpolationTest_OnPoint()
      {
         //Arrange
         double x1 = 1;
         double x2 = 2;
         double f1 = 1;
         double f2 = 2;
         double x = 1.5;
         Calculations.InterpretedValue expected = new Calculations.InterpretedValue(1.5,true);
         //Act
         var Calculations = new Calculations();
         var output = Calculations.LinearInterpolation(x1, f1, x2, f2, x);

         //Assert
         Assert.AreEqual(expected.value, output.value);
      }
      [TestMethod()]
      public void LinearInterpolationTest_OutSideRange()
      {
         //Arrange
         double x1 = 1;
         double x2 = 2;
         double f1 = 1;
         double f2 = 2;
         double x = 0.5;
         Calculations.InterpretedValue expected = new Calculations.InterpretedValue(0, false);
         //Act
         var Calculations = new Calculations();
         var output = Calculations.LinearInterpolation(x1, f1, x2, f2, x);

         //Assert
         Assert.AreEqual(expected.isValidInput, output.isValidInput);
         Assert.AreEqual(expected.value, output.value);
      }
   }
}