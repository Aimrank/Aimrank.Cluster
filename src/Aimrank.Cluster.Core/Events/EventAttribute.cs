using System;

namespace Aimrank.Cluster.Core.Events
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventAttribute : Attribute
    {
        public string Service { get; }

        public EventAttribute(string service)
        {
            Service = service;
        }
    }
}