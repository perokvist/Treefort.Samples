using Treefort.Commanding;

namespace RPS.Api
{
    public class CommandAdapter<T> :  ICommand
    {
        public CommandAdapter(T innerCommand)
        {
            
        }
        public System.Guid AggregateId
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Guid CorrelationId
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}