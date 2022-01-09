using MathNet.Numerics.Distributions;
using System;

namespace SyntheticData
{
    class Program
    {
        static void Main()
        {
            CsvUtils.SaveGeneratedDataAsCsv("fair", Transaction.GenerateFairTransactions, false);
            CsvUtils.SaveGeneratedDataAsCsv("fraudulent", Transaction.GenerateFraudulentTransactions, true);

            CsvUtils.ConcatCsvFiles("fair.csv", "fraudulent.csv", "mixed.csv");

            CsvUtils.FetchDataFromFileIntoAnotherFile("mixed", "mixed_toAnalyse");

            //CsvUtils.SaveFileAsCsv("fraudulentU-Gauss1000", Transaction.GenerateFraudulentTransactions);
            //CsvUtils.FetchDataFromFile("FairTransactions");

            //int numberOfTransactions;
            //for (int i = 0; i < 15; i++)
            //{
            //    Normal normalDist = new Normal(3000, 2000);
            //    numberOfTransactions = (int)Math.Round(normalDist.Sample());
            //    Console.WriteLine(numberOfTransactions);
            //}


        }
    }
}
