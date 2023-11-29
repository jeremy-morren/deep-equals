namespace DeepEqualsGenerator.SourceGenerator.Framework;

internal interface IErrorLog
{
    void WriteError(Location location,
        string id,
        string title,
        string messageFormat,
        params object[] messageArgs);
}