namespace Aimrank.Cluster.Core.Exceptions
{
    public class NotFoundException : ClusterException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}