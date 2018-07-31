using System;
using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.Model;
using EncompassLoadTest.DataInitialization.Results;
using Monad;
using NLog;

namespace EncompassLoadTest.DataInitialization.Creators
{
    public class LoanCreator : BaseCreator<LoanContract>
    {
        private readonly Logger _logger;

        public LoanCreator(IEncompassClient client, LoanContract data, string instanceId) 
            : base(client, data, instanceId)
        {
            _logger = LogManager.GetLogger(nameof(LoanCreator));
        }
        
        public override Try<IResult> Create(string parentId)
        {
            VerifyData();

            return () =>
            {
                try
                {
                    var loanId = Client.LoanService.CreateLoan(Data);
                    _logger.Info($"[{parentId}] Loan created {loanId}.");
                    return new LoanResult(loanId, parentId);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error creating loan for instance {parentId}");
                    throw;
                }
            };
        }
    }
}