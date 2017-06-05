﻿using Castle.DynamicProxy;

namespace RuhRoh.Core
{
    public interface IAffectedMethod
    {
        string Name { get; }

        void AddAffector(IAffector affector);
        void AddTrigger(ITrigger trigger);

        IInterceptor GetInterceptor();
    }
}
