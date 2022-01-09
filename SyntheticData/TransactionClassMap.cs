using CsvHelper.Configuration;

namespace SyntheticData
{
    public class TransactionClassMap : ClassMap<Transaction>
    {
        public TransactionClassMap()
        {
            Map(r => r.PersonId).Name("PersonId");
            Map(r => r.TransactionAmount).Name("TransactionAmount");
            Map(r => r.TransactionCounter).Name("TransactionCounter");
            Map(r => r.IsFraudulent).Name("IsFraudulent");           
        }
    }
}
