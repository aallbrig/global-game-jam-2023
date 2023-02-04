using CleverCrow.Fluid.FSMs;

namespace Core
{
    public interface IStateMachineFactory
    {
        public IFsm Build();
    }
}
