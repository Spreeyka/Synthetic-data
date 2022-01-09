using Accord.Statistics.Distributions.Univariate;
using CsvHelper;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SyntheticData
{
    public class Utils
    {
        public static Random random = new Random();
        public static int randomNumber;

        public static int GenerateFairTransactionCounter()
        {
            int numberOfTransactions;
            Normal normalDist = new Normal(1000, 300);
            numberOfTransactions = (int)Math.Round(normalDist.Sample());
            return numberOfTransactions;
        }

        public static int GenerateFraudulentTransactionCounter()
        {
            Normal normalDist;
            Random r = new Random();
            int random = r.Next(0, 100);

            //70% probability to generate higher transaction counter, to make the task harder for ML model
            //we want to show the tendency of fraudulent person to generate higher number of transaction
            if (random < 70)
            {
                normalDist = new Normal(3000, 1000);
            }
            else
            {
                normalDist = new Normal(1000, 300);
            }
            int numberOfTransactions = (int)Math.Round(normalDist.Sample());
            return numberOfTransactions;
        }

        public static int GenerateAmountThatObeysBenfordsLawWithDistortion()
        {
            Random r = new Random();
            int random = r.Next(0, 100);

            if (random < 95) return GenerateAmountThatObeysBenfordsLaw();
            return GenerateFraudulentRandomAmount();
        }

        public static int GenerateAmountThatObeysBenfordsLaw()
        {
            string firstDigit = AppendFirstDigitBasedOnBenfordsLaw();
            string secondDigit = AppendSecondDigitBasedOnBenfordsLaw();
            string thirdDigit = AppendThirdDigitBasedOnBenfordsLaw();
            string fourthDigit = AppendFourthDigit();
            string fifthDigit = AppendFifthDigit(fourthDigit);
            return int.Parse(firstDigit + secondDigit + thirdDigit + fourthDigit + fifthDigit);
        }

        public static int GenerateFraudulentAmountWithDifferentMethods()
        {
            Random r = new Random();
            int random = r.Next(0, 100);

            //Not all transactions of fraudulent person are not compliant with Benford's law! Most of them actually are. 
            //I assume that only 20% is fraudulent
            if (random < 80) return GenerateAmountThatObeysBenfordsLaw();

            random = r.Next(0, 3);
            return random switch
            {
                0 => GenerateFraudulentRandomAmount(),
                1 => GenerateFraudulentGaussianAmount(),
                2 => GenerateFraudulentUShapedAmount(),
                _ => 0,
            };
        }

        //symulacja sytuacji, w której cyfry są wpisywane zupełnie losowo
        public static int GenerateFraudulentRandomAmount()
        {
            Random random = new Random();
            string firstDigit = random.Next(1, 10).ToString();
            string secondDigit = random.Next(0, 10).ToString();
            string thirdDigit = random.Next(0, 10).ToString();
            string fourthDigit = AppendFourthDigit();
            string fifthDigit = AppendFifthDigit(fourthDigit);
            return int.Parse(firstDigit + secondDigit + thirdDigit + fourthDigit + fifthDigit);
        }

        //symulacja sytuacji, w której fałszowane liczby są wpisywane "ze środka", czyli większość cyfr 3-4-5
        public static int GenerateFraudulentGaussianAmount()
        {
            Normal normalDist = new Normal(5, 0.9);
            string firstDigit = (Math.Round(normalDist.Sample())).ToString();
            normalDist = new Normal(4.5, 0.9);
            string secondDigit = (Math.Round(normalDist.Sample())).ToString();
            normalDist = new Normal(4.5, 0.9);
            string thirdDigit = (Math.Round(normalDist.Sample())).ToString();
            string fourthDigit = AppendFourthDigit();
            string fifthDigit = AppendFifthDigit(fourthDigit);

            return int.Parse(firstDigit + secondDigit + thirdDigit + fourthDigit + fifthDigit);
        }

        //symulacja sytuacji, w której najczęściej fałszowana jest pierwsza cyfra (na 70% jest to 1 albo 9, bo to graniczne cyfry). Pozostałe występują rzadziej
        //uzasadnienie - fałszując dane osoby często manipulują pierszą cyfrą znaczącą, chcąc obniżyć/podwyższyć rząd wielkości
        public static int GenerateFraudulentUShapedAmount()
        {
            string firstDigit = AppendFirstDigitBasedOnUShape();
            string secondDigit = AppendSecondDigitBasedOnBenfordsLaw();
            string thirdDigit = AppendThirdDigitBasedOnBenfordsLaw();
            string fourthDigit = AppendFourthDigit();
            string fifthDigit = AppendFifthDigit(fourthDigit);

            return int.Parse(firstDigit + secondDigit + thirdDigit + fourthDigit + fifthDigit);
        }

        //symulacja sytuacji, w której fałszowana jest pierwsza cyfra, by była jak najmniejsza (głównie 1-2). Reszta jest zgodna z prawem Benforda
        //użyto tutaj odwrócongo Gaussa, czyli Wald distribution https://www.vosesoftware.com/riskwiki/images/image2c33.gif
        //nie wiem, czy jest sens stosować, zwykle U-shaped powinno wystarczyć i mieć lepsze uzasadnienie
        public static int GenerateFraudulentInverseGaussianAmount()
        {
            string firstDigit = Math.Ceiling(InverseGaussianDistribution.Random(1, 9)).ToString();
            string secondDigit = AppendSecondDigitBasedOnBenfordsLaw();
            string thirdDigit = AppendThirdDigitBasedOnBenfordsLaw();
            string fourthDigit = AppendFourthDigit();
            string fifthDigit = AppendFifthDigit(fourthDigit);

            return int.Parse(firstDigit + secondDigit + thirdDigit + fourthDigit + fifthDigit);
        }
      
        public static string AppendFirstDigitBasedOnUShape()
        {
            randomNumber = random.Next(0, 1000);
            if (randomNumber < 350) return "1";
            if (randomNumber >= 350 && randomNumber < 700) return "9";

            if (randomNumber >= 700 && randomNumber < 800) return "2";
            if (randomNumber >= 800 && randomNumber < 900) return "8";

            if (randomNumber >= 900 && randomNumber < 930) return "3";
            if (randomNumber >= 930 && randomNumber < 960) return "7";

            if (randomNumber >= 960 && randomNumber < 975) return "4";
            if (randomNumber >= 975 && randomNumber < 990) return "6";
            else return "5";
        }

        public static string AppendFirstDigitBasedOnBenfordsLaw()
        {
            randomNumber = random.Next(0, 1000);
            if (randomNumber < 301) return "1";
            if (randomNumber >= 301 && randomNumber < 477) return "2";
            if (randomNumber >= 477 && randomNumber < 602) return "3";
            if (randomNumber >= 602 && randomNumber < 699) return "4";
            if (randomNumber >= 699 && randomNumber < 778) return "5";
            if (randomNumber >= 778 && randomNumber < 845) return "6";
            if (randomNumber >= 845 && randomNumber < 903) return "7";
            if (randomNumber >= 903 && randomNumber < 954) return "8";
            else return "9";
        }

        public static string AppendSecondDigitBasedOnBenfordsLaw()
        {
            randomNumber = random.Next(0, 1000);
            if (randomNumber < 120) return "0";
            if (randomNumber >= 120 && randomNumber < 234) return "1";
            if (randomNumber >= 234 && randomNumber < 343) return "2";
            if (randomNumber >= 343 && randomNumber < 447) return "3";
            if (randomNumber >= 447 && randomNumber < 547) return "4";
            if (randomNumber >= 547 && randomNumber < 644) return "5";
            if (randomNumber >= 644 && randomNumber < 737) return "6";
            if (randomNumber >= 737 && randomNumber < 827) return "7";
            if (randomNumber >= 827 && randomNumber < 915) return "8";
            else return "9";
        }

        public static string AppendThirdDigitBasedOnBenfordsLaw()
        {
            randomNumber = random.Next(0, 1000);
            if (randomNumber < 102) return "0";
            if (randomNumber >= 102 && randomNumber < 203) return "1";
            if (randomNumber >= 203 && randomNumber < 304) return "2";
            if (randomNumber >= 304 && randomNumber < 405) return "3";
            if (randomNumber >= 405 && randomNumber < 505) return "4";
            if (randomNumber >= 505 && randomNumber < 605) return "5";
            if (randomNumber >= 605 && randomNumber < 704) return "6";
            if (randomNumber >= 704 && randomNumber < 803) return "7";
            if (randomNumber >= 803 && randomNumber < 902) return "8";
            else return "9";
        }

        public static string AppendFourthDigit()
        {
            randomNumber = random.Next(0, 1000);
            return (randomNumber < 500) ? random.Next(0, 10).ToString() : "";
        }

        public static string AppendFifthDigit(string fourthDigit)
        {
            randomNumber = random.Next(0, 1000);
            return (randomNumber < 500 && fourthDigit != "") ? random.Next(0, 10).ToString() : "";
        }       
    }
}
 