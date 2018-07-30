using System;

namespace MUtils.Cqrs {
    public interface ICommand
    {
        Guid UniqueId { get; }
        Guid SessionId { get; set; }
        //string EventKey { get; } 
    }
}