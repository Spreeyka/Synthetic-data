using System;
using System.Collections.Generic;
using CsvHelper.Configuration;


namespace SyntheticData
{
    class PersonClassMap : ClassMap<Person>
    {
        public PersonClassMap()
        {
            Map(r => r.PersonId).Name("Person_Id");
            Map(r => r.FirstDigitChiSquareTest).Name("First_Digit_Chi_Square");
            Map(r => r.SecondDigitChiSquareTest).Name("Second_Digit_Chi_Square");
            Map(r => r.FirstDigitSmirnovTest).Name("First_Digit_Smirnov");
            Map(r => r.SecondDigitSmirnovTest).Name("Second_Digit_Smirnov");
            Map(r => r.FirstDigit_M2_Test).Name("First_Digit_M2");
            Map(r => r.SecondDigit_M2_Test).Name("Second_Digit_M2");
            Map(r => r.FirstDigit_M3_Test).Name("First_Digit_M3");
            Map(r => r.SecondDigit_M3_Test).Name("Second_Digit_M3");
            Map(r => r.Z_1_1).Name("Z_1_1");
            Map(r => r.Z_1_2).Name("Z_1_2");
            Map(r => r.Z_1_3).Name("Z_1_3");
            Map(r => r.Z_1_4).Name("Z_1_4");
            Map(r => r.Z_1_5).Name("Z_1_5");
            Map(r => r.Z_1_6).Name("Z_1_6");
            Map(r => r.Z_1_7).Name("Z_1_7");
            Map(r => r.Z_1_8).Name("Z_1_8");
            Map(r => r.Z_1_9).Name("Z_1_9");
            Map(r => r.Z_2_0).Name("Z_2_0");
            Map(r => r.Z_2_1).Name("Z_2_1");
            Map(r => r.Z_2_2).Name("Z_2_2");
            Map(r => r.Z_2_3).Name("Z_2_3");
            Map(r => r.Z_2_4).Name("Z_2_4");
            Map(r => r.Z_2_5).Name("Z_2_5");
            Map(r => r.Z_2_6).Name("Z_2_6");
            Map(r => r.Z_2_7).Name("Z_2_7");
            Map(r => r.Z_2_8).Name("Z_2_8");
            Map(r => r.Z_2_9).Name("Z_2_9");
            Map(r => r.FirstDigitCorrelationCoefficient).Name("First_Digit_Correlation_Coefficient");
            Map(r => r.SecondDigitCorrelationCoefficient).Name("Second_Digit_Correlation_Coefficient");
            Map(r => r.TransactionCounter).Name("Transaction_Counter");
            Map(r => r.ActivityTime).Name("Activity_Time");
            Map(r => r.IsFraudulent).Name("Is_Fraudulent");                  
        }
    }
}
