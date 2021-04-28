using System;

namespace Aimrank.Cluster.Core.Exceptions
{
    public class ClusterException : Exception
    {
        public ClusterException(string message) : base(message)
        {
        }
    }
}