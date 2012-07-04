namespace Domain
{
    using System;

    using Infrastructure;

    using Magnum.Reflection;

    public static class AggregateRootEx
    {
        internal static void Raise<TAr, T>(this TAr ar, T evt)
            where TAr : IAggregateRoot, IEventAccessor
            where T : class, IDomainEvent
        {
            try
            {
                ar.Events.RaiseEvent(evt);
            }
            catch (Exception e)
            {
                var x = e.Message;
                throw;
            }
        }

        internal static void Raise<TAr, T>(this TAr ar, object anonymousDictionary)
            where TAr : IAggregateRoot, IEventAccessor
            where T : class, IDomainEvent
        {
            try
            {
                var evt = InterfaceImplementationExtensions.InitializeProxy<T>(anonymousDictionary);
                ar.Events.RaiseEvent(evt);
            }
            catch (Exception e)
            {
                var x = e.Message;
                throw;
            }
        }
    }
}