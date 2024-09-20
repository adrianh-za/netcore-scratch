namespace Basic.Api;

public class GuidGenerator
{
    public string GenerateGuid() => Guid.NewGuid().ToString();
}