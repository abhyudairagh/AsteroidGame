using Utility;

namespace Base
{
    public interface IPoolable : IResettable
    {
        void SetActiveFromPool(bool active);
    }
}