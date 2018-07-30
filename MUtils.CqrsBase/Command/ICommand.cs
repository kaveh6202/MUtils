using System;

namespace MUtils.CqrsBase {
    public interface ICommand
    {
        Guid UniqueId { get; }
        Guid SessionId { get; set; }
        //string EventKey { get; } 
    }
}