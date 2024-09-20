namespace MassTransitMessage;

public interface ISimpleMessage
{
     string Text { get; set; }
}

public  class SimpleMessage: ISimpleMessage
{
    public required string Text { get; set; }
}
