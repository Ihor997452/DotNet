namespace DotNet.CommandService.EvenProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}