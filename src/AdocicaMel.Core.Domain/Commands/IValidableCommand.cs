namespace AdocicaMel.Core.Domain.Commands
{
    public interface IValidableCommand : ICommand
    {
        void Validate();
        bool Valid { get; }
        bool Invalid { get; }
    }
}
