using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      var rand = new Random();
      string quit = "";

      while (quit != "q")
      {
        var userNumber = -1;
        int hiddenNumber = rand.Next(0, 100);

        bool rightInput = true;
        bool rightAnswer = false;
        
        int numberOfTries = 0;
        Console.WriteLine("Try to guess");
       
        while (numberOfTries != 6)
        {
          numberOfTries++;
          string inUser = Console.ReadLine();

          if (!Int32.TryParse(inUser, out userNumber))
          {
              rightInput = false;
              Console.WriteLine("Bad input");
              break;
          }

          if (userNumber < hiddenNumber)
          {
            Console.WriteLine("Number is bigger");
          }

          if (userNumber > hiddenNumber)
          {
            Console.WriteLine("Number is smaller");
          }

          if (userNumber == hiddenNumber)
          {
            rightAnswer = true;
            break;
          }
        }

        if (rightInput && rightAnswer)
        {
          Console.WriteLine("You are right! Your tries " + numberOfTries);
        }
        else
        {
          Console.WriteLine("You are lost!");
        }

        Console.WriteLine("Do you want to continue? ");
        quit = Console.ReadLine();
      }

    }
  }
}
