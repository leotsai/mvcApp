namespace MvcApp.Core
{
    public interface IApiResult
    {
        bool IsSuccess();
        string GetFullError();
    }
}
