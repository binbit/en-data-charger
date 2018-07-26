using DocVelocity.Integration.Encompass.API.Services;

namespace DocVelocity.Integration.Encompass.API
{
    public interface IEncompassClient
    {
        ILoanService LoanService { get; }
        IDocumentService DocumentService { get; }
        IAttachmentService AttachmentService { get; }
        ILockService LockService { get; }
        IPipelineService PipelineService { get; }
    }
}
