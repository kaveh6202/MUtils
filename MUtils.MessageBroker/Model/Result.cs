using System.Collections.Generic;

namespace MUtils.MessageBroker.Model
{
    public abstract class Result<T>
    {
        protected Result(T data)
        {
            Data = data;
        }

        public T Data { get; private set; }


        public abstract void Ack(bool multipel = false);

        public abstract void Nack(bool multipel = false, bool requeue = true);
    }
}