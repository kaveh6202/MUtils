﻿using System.Threading.Tasks;

namespace MUtils.Cqrs {
    public interface IQueryProcessor {
        TResult Process<TResult>(IQuery<TResult> query);
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);
    }
}