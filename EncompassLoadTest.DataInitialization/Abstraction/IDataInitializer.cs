using System.Collections.Generic;

namespace EncompassLoadTest.DataInitialization
{
    public interface IDataInitializer
    {
        List<IResult> InitializeData();
    }
}