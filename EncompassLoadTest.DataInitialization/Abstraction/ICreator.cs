namespace EncompassLoadTest.DataInitialization
{
    public interface ICreator<in TData, TResult>
    {
        void LoadData(string parentId, TData data);
        Try<TResult> Create();
    }
}