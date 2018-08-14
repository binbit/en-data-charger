using CsvHelper.Configuration;

namespace EncompassLoadTest.DataAnalysis
{
    public sealed class InitResultMap : ClassMap<InitResult>
    {
        public InitResultMap()
        {
            AutoMap();
            Map(m => m.LoanId).Name("LoanId");
            Map(m => m.LoanCreateDateUtc).Name("LoanCreateDateUtc");
            Map(m => m.DocumentId).Name("DocumentId");
            Map(m => m.DocumentCreateDateUtc).Name("DocumentCreateDateUtc");
            Map(m => m.AttachmentId).Name("AttachmentId");
            Map(m => m.AttachmentCreateDateUtc).Name("AttachmentCreateDateUtc");
        }
    }
}