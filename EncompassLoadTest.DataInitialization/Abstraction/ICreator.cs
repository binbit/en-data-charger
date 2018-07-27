namespace EncompassLoadTest.DataInitialization
{
    public interface ICreator
    {
        Try<IResult> Create();
    }
}