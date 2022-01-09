using System;
using System.Collections.Generic;

namespace SyntheticData
{
    public class Transaction
    {
        public Guid PersonId { get; set; }
        public int TransactionAmount { get; set; }
        public int TransactionCounter { get; set; }
        public bool IsFraudulent { get; set; }

        public Transaction(Guid PersonId, int TransactionAmount, int TransactionCounter, bool IsFraudulent)
        {
            this.PersonId = PersonId;
            this.TransactionAmount = TransactionAmount;
            this.TransactionCounter = TransactionCounter;
            this.IsFraudulent = IsFraudulent;    
        }        
       
        public static List<Transaction> GenerateFairTransactions()
        {
            List<Transaction> fairTransactions = new List<Transaction>();
            Guid personId;
            int transactionsCounter;



            for (int i = 0; i < 450; i++)
            {
                transactionsCounter = Utils.GenerateFairTransactionCounter();
                personId = GeneratePersonId();
                for (int j = 0; j < transactionsCounter; j++)
                {
                    fairTransactions.Add(new Transaction(personId, Utils.GenerateAmountThatObeysBenfordsLawWithDistortion(), transactionsCounter, false));
                }
                
            }

            //we populate legit with 25 fraudulent. The goal of machine learning is to detect these
            for (int i = 0; i < 25; i++)
            {
                personId = GeneratePersonId();
                transactionsCounter = Utils.GenerateFraudulentTransactionCounter();
                for (int j = 0; j < transactionsCounter; j++)
                {
                    fairTransactions.Add(new Transaction(personId, Utils.GenerateFraudulentAmountWithDifferentMethods(), transactionsCounter, false));
                }
            }
            return fairTransactions;
        }

        public static List<Transaction> GenerateFraudulentTransactions()
        {
            List<Transaction> fraudulentTransactions = new List<Transaction>();
            Guid personId;

            for (int i = 0; i < 25; i++)
            {
                personId = GeneratePersonId();
                int transactionsCounter = Utils.GenerateFraudulentTransactionCounter();               
                for (int j = 0; j < transactionsCounter; j++)
                {                   
                    fraudulentTransactions.Add(new Transaction(personId, Utils.GenerateFraudulentAmountWithDifferentMethods(), transactionsCounter, true));
                }
            }
            return fraudulentTransactions;
        }

        public static Guid GeneratePersonId()
        {
            return Guid.NewGuid();
        }       
    }
}


//**********************  OPCJONALNIE TODO ************************************************************
//
//-dla każdego unikalnego dostawcy dodać liczbę operacji ???
//-dla każdego unikalnego dostawcy zsumować liczebność wszystkich liczb(parametr n)??