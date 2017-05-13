using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace RuhRoh.Core.ProxyGeneration
{
    internal class AffectorProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return true;
        }
    }
}