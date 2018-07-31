using System.Threading.Tasks;

namespace EncompassLoadTest.DataInitialization
{
    public interface ICreationBlock
    {
        Task<IResult> CreateAsync(IResult result, string parentId);
    }
}