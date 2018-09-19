namespace AdocicaMel.Core.Domain.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }
}
