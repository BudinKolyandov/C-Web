namespace SIS.HTTP.Sessions
{
    public interface IHttpSession
    {
        string Id { get; }

        object GetParamenter(string name);

        bool ContainsParameter(string name);

        void AddParameter(string name, object parameter);

        void ClearParameters();

    }
}
