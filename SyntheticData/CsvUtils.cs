using CsvHelper;
using CsvHelper.Configuration;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SyntheticData
{
    class CsvUtils
    {       
        public static void SaveGeneratedDataAsCsv(string filename, Func<List<Transaction>> dataGenerator, bool isFraudulent)
        {
            int numberOfTransactions;
            var csvPath = Path.Combine(Environment.CurrentDirectory, $"{filename}.csv");
            using var streamWriter = new StreamWriter(csvPath);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            //tutaj ustawiamy, ile transakcji na osobę
            if(isFraudulent)
            {
                Normal normalDist = new Normal(3000, 1000);
                numberOfTransactions = (int)Math.Round(normalDist.Sample());
            }
            else
            {
                Normal normalDist = new Normal(1000, 300);
                numberOfTransactions = (int)Math.Round(normalDist.Sample());
            }
            
            var data = dataGenerator();
            csvWriter.Context.RegisterClassMap<TransactionClassMap>();
            csvWriter.WriteRecords(data);
        }

        public static void ConcatCsvFiles(string firstFile, string secondFile, string outFile)
        {
            var baseLines = File.ReadAllLines(firstFile).ToList();
            var newLines = File.ReadAllLines(secondFile);
            baseLines.AddRange(newLines.Skip(1));
            File.WriteAllLines(outFile, baseLines);
        }

        public static IEnumerable<Transaction> ReadCsvFile(string filename)
        {
            var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"{filename}.csv"));
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Transaction>();
        }


        //Mamy listę osób: ID, Liczby występień pierwszej cyfry, int[9] FirstDigitCounter, int[10] SecondDigitCounter
        public static void FetchDataFromFileIntoAnotherFile(string inputFilename, string outputFilename)
        {
            var records = ReadCsvFile(inputFilename);
            var peopleInfos = Person.CreatePeopleList(records);
            //Person.ShowPeopleData(peopleInfos);

            var csvPath = Path.Combine(Environment.CurrentDirectory, $"{outputFilename}.csv");
            using var streamWriter = new StreamWriter(csvPath);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.Context.RegisterClassMap<PersonClassMap>();
            csvWriter.WriteRecords(peopleInfos);
        }
    }
}