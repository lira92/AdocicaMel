using System.Threading.Tasks;

namespace AdocicaMel.Core.Domain.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }
}
