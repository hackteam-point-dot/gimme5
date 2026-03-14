namespace Widget.Api.Configuration;

public class MongoDbSettings
{
    public const string SectionName = "MongoDb";

    public string DatabaseName { get; set; } = "WidgetDb";
}
