namespace TibaRepoSearch;

public interface IEventPublisher<TEvent>
{
    Task PublishAsync(TEvent payload);
}