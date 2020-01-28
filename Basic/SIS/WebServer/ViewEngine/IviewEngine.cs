namespace SIS.MvcFramework.ViewEngine
{
    public interface IviewEngine
    {
        string GetHtml<T>(string viewContent, T model);
    }
}
