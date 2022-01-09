using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyntheticData
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public double[] FirstDigitCounter { get; set; }
        public double[] SecondDigitCounter { get; set; }
        public double TransactionCounter { get; set; }
        public double ActivityTime { get; set; }
        public double FirstDigitCorrelationCoefficient { get; set; }
        public double SecondDigitCorrelationCoefficient { get; set; }
        public double FirstDigit_M2_Test { get; set; }
        public double SecondDigit_M2_Test { get; set; }
        public double FirstDigit_M3_Test { get; set; }
        public double SecondDigit_M3_Test { get; set; }
        public double FirstDigit_D_parameter { get; set; }
        public double SecondDigit_D_parameter { get; set; }
        public double FirstDigitSmirnovTest { get; set; }
        public double SecondDigitSmirnovTest { get; set; }
        public double FirstDigitChiSquareTest { get; set; }
        public double SecondDigitChiSquareTest { get; set; }

        public bool IsFraudulent { get; set; }

        public double Z_1_1 { get; set; }
        public double Z_1_2 { get; set; }
        public double Z_1_3 { get; set; }
        public double Z_1_4 { get; set; }
        public double Z_1_5 { get; set; }
        public double Z_1_6 { get; set; }
        public double Z_1_7 { get; set; }
        public double Z_1_8 { get; set; }
        public double Z_1_9 { get; set; }
        public double Z_2_0 { get; set; }
        public double Z_2_1 { get; set; }
        public double Z_2_2 { get; set; }
        public double Z_2_3 { get; set; }
        public double Z_2_4 { get; set; }
        public double Z_2_5 { get; set; }
        public double Z_2_6 { get; set; }
        public double Z_2_7 { get; set; }
        public double Z_2_8 { get; set; }
        public double Z_2_9 { get; set; }

        List<double> FirstDigit_Z_Test_Array { get; set; }
        List<double> SecondDigit_Z_Test_Array { get; set; }
       
        List<double> EmpiricalFirstDigitF_Array = new List<double>();

        List<double> TheoreticalFirstDigitF_Array = new List<double>();

        List<double> EmpiricalSecondDigitF_Array = new List<double>();

        List<double> TheoreticalSecondDigitF_Array = new List<double>();

        List<double> TheoreticalFirstDigitOccurancesArray = new List<double>();

        List<double> TheoreticalSecondDigitOccurancesArray = new List<double>();

        List<double> EmpiricalFirstDigitP_Array = new List<double>();

        List<double> EmpiricalSecondDigitP_Array = new List<double>();

        List<double> TheoreticalFirstDigitP_Array = new List<double>();

        List<double> TheoreticalSecondDigitP_Array = new List<double>();





        public Person(Guid PersonId)
        {
            this.PersonId = PersonId;
            FirstDigitCounter = new double[9];
            SecondDigitCounter = new double[10];
            FirstDigit_Z_Test_Array = new List<double>();
            SecondDigit_Z_Test_Array = new List<double>();          
        }

        
        public static List<Person> CreatePeopleList(IEnumerable<Transaction> records)
        {
            List<Person> PeopleInfos = new List<Person>();
            GetInitialDataForEachPersonId(records, PeopleInfos);
            CalculateParameterForTests(PeopleInfos);
            CalculateTests(PeopleInfos);
            AssignValuesOf_Z_Test_ToSingleProperties(PeopleInfos);
            return PeopleInfos;
        }

        //poniżej obliczamy potrzebne parametry i testy dla każdej unikalnej osoby. Część z tych parametrów stanowi wejście do uczenia maszynowego
        private static void CalculateTests(List<Person> PeopleInfos)
        {
            Calculate_Z_Test(PeopleInfos);
            CalculateChiSquareTest(PeopleInfos);
            CalculateKolmogorovSmirnovTest(PeopleInfos);
            CalculateFirstDigit_M2_Test(PeopleInfos);
            CalculateFirstDigit_M3_Test(PeopleInfos);
            CalculateSecondDigit_M2_Test(PeopleInfos);
            CalculateSecondDigit_M3_Test(PeopleInfos);
            CalculateFirstDigitCorrelationCoefficient(PeopleInfos);
            CalculateSecondDigitCorrelationCoefficient(PeopleInfos);
        }

        private static void CalculateParameterForTests(List<Person> PeopleInfos)
        {
            Calculate_Theoretical_Amounts_Array(PeopleInfos);
            Calculate_P_Lists(PeopleInfos);
            Calculate_FirstDigit_F_Parameters(PeopleInfos);
            Calculate_SecondDigit_F_Parameters(PeopleInfos);
            CalculateFirstDigit_D_Parameter(PeopleInfos);
            CalculateSecondDigit_D_Parameter(PeopleInfos);
        }

        //Wyciągamy dane dla poszczególnych osób: wystąpienia poszczególnych pierwszych cyfr, drugich cyfr i liczebność transakcji
        private static void GetInitialDataForEachPersonId(IEnumerable<Transaction> records, List<Person> PeopleInfos)
        {
            records.GroupBy(g => g.PersonId).ToList().ForEach(e =>
            {
                PeopleInfos.Add(new Person(e.Key));
                GetIsFraudulent(e, PeopleInfos);
                //GetTransactionCounter(e, PeopleInfos);
                GetActivityTime(e, PeopleInfos);
                CountFirstDigitOfUniquePerson(e, PeopleInfos);
                CountSecondDigitOfUniquePerson(e, PeopleInfos);
                CountTransactions(e, PeopleInfos);
            });
        }

        private static void CountSecondDigitOfUniquePerson(IGrouping<Guid, Transaction> e, List<Person> PeopleInfos)
        {
            for (int i = 0; i < 10; i++)
            {
                PeopleInfos.First(n => n.PersonId == e.Key).SecondDigitCounter[i] = e.Count(s => s.TransactionAmount.ToString().ElementAt(1).ToString() == (i).ToString());
            }
        }

        private static void CountFirstDigitOfUniquePerson(IGrouping<Guid, Transaction> e, List<Person> PeopleInfos)
        {
            for (int i = 0; i < 9; i++)
            {
                PeopleInfos.First(n => n.PersonId == e.Key).FirstDigitCounter[i] = e.Count(s => s.TransactionAmount.ToString().ElementAt(0).ToString() == (i + 1).ToString());
            }
        }

        private static void CountTransactions(IGrouping<Guid, Transaction> e, List<Person> PeopleInfos)
        {
                PeopleInfos.First(n => n.PersonId == e.Key).TransactionCounter = e.First(s => s == s).TransactionCounter;
        }
        private static void GetIsFraudulent(IGrouping<Guid, Transaction> e, List<Person> PeopleInfos)
        {
            PeopleInfos.First(n => n.PersonId == e.Key).IsFraudulent = e.Any(s => s.IsFraudulent == true);
        }
        

        private static void GetActivityTime(IGrouping<Guid, Transaction> e, List<Person> PeopleInfos)
        {
            Random r = new Random();
            Normal normalDist;


            if (PeopleInfos.First(n => n.PersonId == e.Key).IsFraudulent == true)
            {
                int random = r.Next(0, 100);

                //70% probability to generate lower activity time, to make the task harder for ML model
                if(random < 70)
                {
                    normalDist = new Normal(600, 150);
                }
                else
                {
                    normalDist = new Normal(800, 200);
                }                             
                double normalNumberInRange = Math.Round(normalDist.Sample());
                PeopleInfos.First(n => n.PersonId == e.Key).ActivityTime = normalNumberInRange;
            }
            else
            {
                normalDist = new Normal(800, 200);
                double normalNumberInRange = Math.Round(normalDist.Sample());
                PeopleInfos.First(n => n.PersonId == e.Key).ActivityTime = normalNumberInRange;
            }
        }

        private static void CalculateFirstDigitCorrelationCoefficient(List<Person> PeopleInfos)
        {
            double empiricalMeanAverage = 0;
            double theoreticalMeanAverage = 0;
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;

            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.FirstDigitCounter.Length; i++)
                {
                    empiricalMeanAverage += person.FirstDigitCounter[i];
                    theoreticalMeanAverage += person.TheoreticalFirstDigitOccurancesArray[i];
                }

                empiricalMeanAverage /= person.FirstDigitCounter.Length;
                theoreticalMeanAverage /= person.TheoreticalFirstDigitOccurancesArray.Count;

                for (int i = 0; i < person.FirstDigitCounter.Length; i++)
                {
                    sum1 += (person.FirstDigitCounter[i] - empiricalMeanAverage) * (person.TheoreticalFirstDigitOccurancesArray[i] - theoreticalMeanAverage);
                }
                for (int i = 0; i < person.FirstDigitCounter.Length; i++)
                {
                    sum2 += Math.Pow(person.FirstDigitCounter[i] - empiricalMeanAverage, 2);
                }
                for (int i = 0; i < person.TheoreticalFirstDigitOccurancesArray.Count; i++)
                {
                    sum3 +=  Math.Pow(person.TheoreticalFirstDigitOccurancesArray[i] - theoreticalMeanAverage, 2);
                }
                person.FirstDigitCorrelationCoefficient = Convert.ToDouble((sum1 / Math.Sqrt(sum2 * sum3)).ToString("N2"));
            }
        }

        private static void CalculateSecondDigitCorrelationCoefficient(List<Person> PeopleInfos)
        {
            double empiricalMeanAverage = 0;
            double theoreticalMeanAverage = 0;
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;

            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.SecondDigitCounter.Length; i++)
                {
                    empiricalMeanAverage += person.SecondDigitCounter[i];
                    theoreticalMeanAverage += person.TheoreticalSecondDigitOccurancesArray[i];
                }

                empiricalMeanAverage /= person.SecondDigitCounter.Length;
                theoreticalMeanAverage /= person.TheoreticalSecondDigitOccurancesArray.Count;

                for (int i = 0; i < person.SecondDigitCounter.Length; i++)
                {
                    sum1 +=
                      (person.SecondDigitCounter[i] - empiricalMeanAverage) *
                      (person.TheoreticalSecondDigitOccurancesArray[i] - theoreticalMeanAverage);
                }
                for (int i = 0; i < person.SecondDigitCounter.Length; i++)
                {
                    sum2 += Math.Pow((person.SecondDigitCounter[i] - empiricalMeanAverage), 2);
                }
                for (int i = 0; i < person.TheoreticalSecondDigitOccurancesArray.Count; i++)
                {
                    sum3 += Math.Pow((person.TheoreticalSecondDigitOccurancesArray[i] - theoreticalMeanAverage), 2);
                }
                person.SecondDigitCorrelationCoefficient = Convert.ToDouble((sum1 / Math.Sqrt(sum2 * sum3)).ToString("N2"));
            }
        }

        private static void CalculateFirstDigit_M2_Test(List<Person> PeopleInfos)
        {
            double sum = 0;
            foreach (Person person in PeopleInfos)
            {               
                for(int i = 0; i < person.EmpiricalFirstDigitP_Array.Count; i++)
                {
                    sum += Math.Pow(100 * person.EmpiricalFirstDigitP_Array[i] - 100 * person.TheoreticalFirstDigitP_Array[i], 2);
                }
                person.FirstDigit_M2_Test = Convert.ToDouble((1.0 / person.EmpiricalFirstDigitP_Array.Count * Math.Sqrt(sum)).ToString("N2"));
                sum = 0;
            }
        }

        private static void CalculateFirstDigit_M3_Test(List<Person> PeopleInfos)
        {
            double sum = 0;
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalFirstDigitP_Array.Count; i++)
                {
                    sum += Math.Pow(100 * person.EmpiricalFirstDigitP_Array[i] - 100 * person.TheoreticalFirstDigitP_Array[i], 2);
                }
                person.FirstDigit_M3_Test = Convert.ToDouble(Math.Sqrt(sum / person.EmpiricalFirstDigitP_Array.Count).ToString("N2"));
                sum = 0;
            }                                     
        }

        private static void CalculateSecondDigit_M2_Test(List<Person> PeopleInfos)
        {
            double sum = 0;
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalSecondDigitP_Array.Count; i++)
                {
                    sum += Math.Pow(100 * person.EmpiricalSecondDigitP_Array[i] - 100 * person.TheoreticalSecondDigitP_Array[i], 2);
                }
                person.SecondDigit_M2_Test = Convert.ToDouble((1.0 / person.EmpiricalSecondDigitP_Array.Count * Math.Sqrt(sum)).ToString("N2"));
                sum = 0;
            }
        }

        private static void CalculateSecondDigit_M3_Test(List<Person> PeopleInfos)
        {
            double sum = 0;
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalSecondDigitP_Array.Count; i++)
                {
                    sum += Math.Pow(100 * person.EmpiricalSecondDigitP_Array[i] - 100 * person.TheoreticalSecondDigitP_Array[i], 2);
                }
                person.SecondDigit_M3_Test = Convert.ToDouble((Math.Sqrt(sum / person.EmpiricalSecondDigitP_Array.Count)).ToString("N2"));
                sum = 0;
            }
        }

        private static void CalculateKolmogorovSmirnovTest(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                person.FirstDigitSmirnovTest = Convert.ToDouble((person.FirstDigit_D_parameter * Math.Sqrt(Math.Pow(person.TransactionCounter, 2) / (2 * person.TransactionCounter))).ToString("N2"));
            }
            foreach (Person person in PeopleInfos)
            {
                person.SecondDigitSmirnovTest = Convert.ToDouble((person.SecondDigit_D_parameter * Math.Sqrt(Math.Pow(person.TransactionCounter, 2) / (2 * person.TransactionCounter))).ToString("N2"));
            }
        }
 
        private static void CalculateFirstDigit_D_Parameter(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                List<double> ArrayOfDifferences = new List<double>();
                for (int i = 0; i < person.EmpiricalFirstDigitF_Array.Count; i++)
                {
                    ArrayOfDifferences.Add(Math.Abs(person.EmpiricalFirstDigitF_Array[i] - person.TheoreticalFirstDigitF_Array[i]));
                }
                person.FirstDigit_D_parameter = ArrayOfDifferences.Max();
            }                
        }

        private static void CalculateSecondDigit_D_Parameter(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                List<double> ArrayOfDifferences = new List<double>();
                for (int i = 0; i < person.EmpiricalSecondDigitF_Array.Count; i++)
                {
                    ArrayOfDifferences.Add(Math.Abs(person.EmpiricalSecondDigitF_Array[i] - person.TheoreticalSecondDigitF_Array[i]));
                }
                person.SecondDigit_D_parameter = ArrayOfDifferences.Max();
            }
        }

        private static void Calculate_FirstDigit_F_Parameters(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalFirstDigitP_Array.Count; i++)
                {
                    if (i == 0) person.EmpiricalFirstDigitF_Array.Add(person.EmpiricalFirstDigitP_Array.ElementAt(i));
                    else
                    {
                        person.EmpiricalFirstDigitF_Array.Add(person.EmpiricalFirstDigitP_Array.ElementAt(i) + 
                            person.EmpiricalFirstDigitF_Array.ElementAt(i - 1));
                    }
                }

                for (int i = 0; i < person.TheoreticalFirstDigitP_Array.Count; i++)
                {
                    if (i == 0) person.TheoreticalFirstDigitF_Array.Add(person.TheoreticalFirstDigitP_Array.ElementAt(i));
                    else
                    {
                        person.TheoreticalFirstDigitF_Array.Add(person.TheoreticalFirstDigitP_Array.ElementAt(i) +
                            person.TheoreticalFirstDigitF_Array.ElementAt(i - 1));
                    }
                }
            }            
        }

        private static void Calculate_SecondDigit_F_Parameters(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalSecondDigitP_Array.Count; i++)
                {
                    if (i == 0) person.EmpiricalSecondDigitF_Array.Add(person.EmpiricalSecondDigitP_Array.ElementAt(i));
                    else
                    {
                        person.EmpiricalSecondDigitF_Array.Add(person.EmpiricalSecondDigitP_Array.ElementAt(i) +
                            person.EmpiricalSecondDigitF_Array.ElementAt(i - 1));
                    }
                }

                for (int i = 0; i < person.TheoreticalSecondDigitP_Array.Count; i++)
                {
                    if (i == 0) person.TheoreticalSecondDigitF_Array.Add(person.TheoreticalSecondDigitP_Array.ElementAt(i));
                    else
                    {
                        person.TheoreticalSecondDigitF_Array.Add(person.TheoreticalSecondDigitP_Array.ElementAt(i) +
                            person.TheoreticalSecondDigitF_Array.ElementAt(i - 1));
                    }
                }
            }
        }

        private static void CalculateChiSquareTest(List<Person> PeopleInfos)
        {
            double sum = 0;
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalFirstDigitP_Array.Count; i++)
                {
                    sum += (Math.Pow((person.EmpiricalFirstDigitP_Array[i] - person.TheoreticalFirstDigitP_Array[i]), 2) / person.TheoreticalFirstDigitP_Array[i]);
                }
                person.FirstDigitChiSquareTest = Convert.ToDouble((person.TransactionCounter * sum).ToString("N2"));

                sum = 0;

                for (int i = 0; i < person.EmpiricalSecondDigitP_Array.Count; i++)
                {
                    sum += Math.Pow(person.EmpiricalSecondDigitP_Array[i] - person.TheoreticalSecondDigitP_Array[i], 2) / person.TheoreticalSecondDigitP_Array[i];
                }
                person.SecondDigitChiSquareTest = Convert.ToDouble((person.TransactionCounter * sum).ToString("N2"));
            }                                   
        }

        private static void Calculate_Z_Test(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                for (int i = 0; i < person.EmpiricalFirstDigitP_Array.Count; i++)
                {
                    person.FirstDigit_Z_Test_Array.Add(Convert.ToDouble(((person.EmpiricalFirstDigitP_Array[i] - person.TheoreticalFirstDigitP_Array[i]) /
                        Math.Sqrt((person.TheoreticalFirstDigitP_Array[i] * (1 - person.TheoreticalFirstDigitP_Array[i])) / person.TransactionCounter)).ToString("N2")));
                }
                for (int i = 0; i < person.EmpiricalSecondDigitP_Array.Count; i++)
                {
                    person.SecondDigit_Z_Test_Array.Add(Convert.ToDouble(((person.EmpiricalSecondDigitP_Array[i] - person.TheoreticalSecondDigitP_Array[i]) /
                        Math.Sqrt((person.TheoreticalSecondDigitP_Array[i] * (1 - person.TheoreticalSecondDigitP_Array[i])) / person.TransactionCounter)).ToString("N2")));
                }
            }
        }

        private static void AssignValuesOf_Z_Test_ToSingleProperties(List<Person> PeopleInfos)
        {
            foreach (Person person in PeopleInfos)
            {
                person.Z_1_1 = person.FirstDigit_Z_Test_Array.ElementAt(0);
                person.Z_1_2 = person.FirstDigit_Z_Test_Array.ElementAt(1);
                person.Z_1_3 = person.FirstDigit_Z_Test_Array.ElementAt(2);
                person.Z_1_4 = person.FirstDigit_Z_Test_Array.ElementAt(3);
                person.Z_1_5 = person.FirstDigit_Z_Test_Array.ElementAt(4);
                person.Z_1_6 = person.FirstDigit_Z_Test_Array.ElementAt(5);
                person.Z_1_7 = person.FirstDigit_Z_Test_Array.ElementAt(6);
                person.Z_1_8 = person.FirstDigit_Z_Test_Array.ElementAt(7);
                person.Z_1_9 = person.FirstDigit_Z_Test_Array.ElementAt(8);
                person.Z_2_0 = person.SecondDigit_Z_Test_Array.ElementAt(0);
                person.Z_2_1 = person.SecondDigit_Z_Test_Array.ElementAt(1);
                person.Z_2_2 = person.SecondDigit_Z_Test_Array.ElementAt(2);
                person.Z_2_3 = person.SecondDigit_Z_Test_Array.ElementAt(3);
                person.Z_2_4 = person.SecondDigit_Z_Test_Array.ElementAt(4);
                person.Z_2_5 = person.SecondDigit_Z_Test_Array.ElementAt(5);
                person.Z_2_6 = person.SecondDigit_Z_Test_Array.ElementAt(6);
                person.Z_2_7 = person.SecondDigit_Z_Test_Array.ElementAt(7);
                person.Z_2_8 = person.SecondDigit_Z_Test_Array.ElementAt(8);
                person.Z_2_9 = person.SecondDigit_Z_Test_Array.ElementAt(9);
            }
        }

        public static void Calculate_P_Lists(List<Person> PeopleInfos)
        {
            foreach (var person in PeopleInfos)
            {
                for (int i = 0; i < person.FirstDigitCounter.Length; i++)
                {
                    person.EmpiricalFirstDigitP_Array.Add(person.FirstDigitCounter[i] / person.TransactionCounter);
                    person.TheoreticalFirstDigitP_Array.Add(person.TheoreticalFirstDigitOccurancesArray[i] / person.TransactionCounter);
                }
                for (int i = 0; i < person.SecondDigitCounter.Length; i++)
                {
                    person.EmpiricalSecondDigitP_Array.Add(person.SecondDigitCounter[i] / person.TransactionCounter);
                    person.TheoreticalSecondDigitP_Array.Add(person.TheoreticalSecondDigitOccurancesArray[i] / person.TransactionCounter);
                }
            }
        }   

        private static void Calculate_Theoretical_Amounts_Array(List<Person> PeopleInfos)
        {
            var firstDigitBenfordValues = new double[9] { 30.1, 17.7, 12.5, 9.7, 7.9, 6.7, 5.8, 5.1, 4.6 };
            var secondDigitBenfordValues = new double[10] { 11.9, 11.3, 10.9, 10.4, 10.0, 9.7, 9.3, 9.0, 8.8, 8.6 };
            
            foreach (var person in PeopleInfos)
            {
                for (int i = 0; i < firstDigitBenfordValues.Length; i++)
                {
                    person.TheoreticalFirstDigitOccurancesArray.Add(Math.Round(firstDigitBenfordValues[i] * person.TransactionCounter / 100));
                }
                for (int i = 0; i < secondDigitBenfordValues.Length; i++)
                {
                    person.TheoreticalSecondDigitOccurancesArray.Add(Math.Round(secondDigitBenfordValues[i] * person.TransactionCounter / 100));
                }
            }           
        }

        public static void ShowPeopleData(List<Person> PeopleInfos)
        {
            foreach (var item in PeopleInfos)
            {
                Console.WriteLine("ID osoby " + item.PersonId);
                Console.WriteLine("Liczba transakcji " + item.TransactionCounter);

                for (int q = 0; q < item.FirstDigitCounter.Length; q++)
                {
                    Console.WriteLine($"Wystąpienia pierwszej cyfry równej {q + 1}:  " + item.FirstDigitCounter[q]);
                }
                for (int q = 0; q < item.SecondDigitCounter.Length; q++)
                {
                    Console.WriteLine($"Wystąpienia drugiej cyfry równej {q}:  " + item.SecondDigitCounter[q]);
                }
                for (int i = 0; i < item.TheoreticalFirstDigitP_Array.Count; i++)
                {
                    Console.WriteLine("theoretical first digit P " + item.TheoreticalFirstDigitP_Array[i]);
                }
                for (int i = 0; i < item.TheoreticalSecondDigitP_Array.Count; i++)
                {
                    Console.WriteLine("theoretical second digit P " + item.TheoreticalSecondDigitP_Array[i]);
                }
                for (int i = 0; i < item.EmpiricalFirstDigitP_Array.Count; i++)
                {
                    Console.WriteLine("empirical first digit P " + item.EmpiricalFirstDigitP_Array[i]);
                }
                for (int i = 0; i < item.EmpiricalSecondDigitP_Array.Count; i++)
                {
                    Console.WriteLine("empirical second digit P " + item.EmpiricalSecondDigitP_Array[i]);
                }
                for (int i = 0; i < item.TheoreticalFirstDigitOccurancesArray.Count; i++)
                {
                    Console.WriteLine("theoretical first digit occurrances " + item.TheoreticalFirstDigitOccurancesArray[i]);
                }
                for (int i = 0; i < item.TheoreticalSecondDigitOccurancesArray.Count; i++)
                {
                    Console.WriteLine("theoretical second digit occurrances " + item.TheoreticalSecondDigitOccurancesArray[i]);
                }
                for (int i = 0; i < item.FirstDigit_Z_Test_Array.Count; i++)
                {
                    Console.WriteLine("first digit Z " + item.FirstDigit_Z_Test_Array[i]);
                }
                for (int i = 0; i < item.SecondDigit_Z_Test_Array.Count; i++)
                {
                    Console.WriteLine("second digit Z " + item.SecondDigit_Z_Test_Array[i]);
                }              
                for (int i = 0; i < item.EmpiricalFirstDigitF_Array.Count; i++)
                {
                    Console.WriteLine("Wspolczynniki kumulanty f empiryczne 1 cyfra " + item.EmpiricalFirstDigitF_Array[i]);
                }
                for (int i = 0; i < item.TheoreticalFirstDigitF_Array.Count; i++)
                {
                    Console.WriteLine("Wspolczynniki kumulanty f teoretyczne 1 cyfra " + item.TheoreticalFirstDigitF_Array[i]);
                }
                for (int i = 0; i < item.EmpiricalSecondDigitF_Array.Count; i++)
                {
                    Console.WriteLine("Wspolczynniki kumulanty f empiryczne 2 cyfra " + item.EmpiricalSecondDigitF_Array[i]);
                }
                for (int i = 0; i < item.TheoreticalSecondDigitF_Array.Count; i++)
                {
                    Console.WriteLine("Wspolczynniki kumulanty f teoretyczne 2 cyfra " + item.TheoreticalSecondDigitF_Array[i]);
                }
                Console.WriteLine();
                Console.WriteLine("TESTY");
                Console.WriteLine();

                Console.WriteLine("First digit D parameter " + item.FirstDigit_D_parameter);
                Console.WriteLine("Second digit D parameter " + item.SecondDigit_D_parameter);

                Console.WriteLine("first digit Chi Square " + item.FirstDigitChiSquareTest);
                Console.WriteLine("second digit Chi Square " + item.SecondDigitChiSquareTest);

                Console.WriteLine("First digit Smirnov " + item.FirstDigitSmirnovTest);
                Console.WriteLine("Second digit Smirnov " + item.SecondDigitSmirnovTest);

                Console.WriteLine("First digit M2 " + item.FirstDigit_M2_Test);
                Console.WriteLine("First digit M3 " + item.FirstDigit_M3_Test);

                Console.WriteLine("Second digit M2 " + item.SecondDigit_M2_Test);
                Console.WriteLine("Second digit M3 " + item.SecondDigit_M3_Test);

                Console.WriteLine("First digit correlaction coefficient " + item.FirstDigitCorrelationCoefficient);
                Console.WriteLine("Second digit correlaction coefficient " + item.SecondDigitCorrelationCoefficient);

                Console.WriteLine("Is fraudulent " + item.IsFraudulent);


                Console.WriteLine();
            }
        }

    }
}

//mamy 9 plików toAnalyse. Robimy magię w jupyterze (SMOTE, WEKA, MODELE)

//wyszly niezgodne testy w mixed_random_100, ale czy wyjdą w większych próbach?




