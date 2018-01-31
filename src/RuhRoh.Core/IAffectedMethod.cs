using Castle.DynamicProxy;

namespace RuhRoh.Core
{
    public interface IAffectedMethod
    {
        string Name { get; }

        IAffector AddAffector(IAffector affector);
        void AddTrigger(IAffector affector, ITrigger trigger);

        IInterceptor GetInterceptor();
    }
}
