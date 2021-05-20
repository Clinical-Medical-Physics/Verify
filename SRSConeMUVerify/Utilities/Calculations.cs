using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRSConeMUVerify.Utilities
{
   public class Calculations
   {
      public class InterpretedValue
      {
         public double value;
         public bool isValidInput;

         public InterpretedValue(double value = 0, bool isValidInput = false)
         {
            this.value = value;
            this.isValidInput = isValidInput;
         }

      }
      /// <summary>
      /// Returns the linear interpolation between the given points
      /// Tries to ensure valid arguments but might fail in some cases
      /// </summary>
      /// <param name="x1">
      /// The lower value of x, usually from a table
      /// </param>
      /// <param name="f1">
      /// The function value at x1, usually from a table
      /// </param>
      /// <param name="x2">
      /// The upper value of x, usually from a table
      /// </param>
      /// <param name="f2">
      /// the function value at x2, usually from a table
      /// </param>
      /// <param name="x">
      /// The value of x where you want to know f
      /// </param>
      /// <returns></returns>
      public static InterpretedValue LinearInterpolation(double x1, double f1, double x2, double f2, double x)
      {
         InterpretedValue interpretedValue = new InterpretedValue(0,true);

         if ((x1 == x2) || (x < x1 && x < x2) || (x > x1 && x > x2))
         {
            interpretedValue.isValidInput = false;
            return interpretedValue;
         }
         interpretedValue.value = ((x2 - x) / (x2 - x1)) * f1 + ((x - x1) / (x2 - x1)) * f2;
         return interpretedValue;
      }
   }
}
