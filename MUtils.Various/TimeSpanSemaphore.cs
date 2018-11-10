using System;
using System.Threading;
using System.Threading.Tasks;

namespace MUtils.Various
{
    public class TimeSpanSemaphore : IDisposable
    {
        private readonly SemaphoreSlim _pool;
        private readonly TimeSpan _resetSpan;
        private readonly long _startTime = DateTime.Now.ToUnixTimeMilliseconds();

        public TimeSpanSemaphore(int maxCount, TimeSpan resetSpan)
        {
            this._pool = new SemaphoreSlim(maxCount, maxCount);
            this._resetSpan = resetSpan;
        }

        public void Run(Action action, CancellationToken cancelToken)
        {

            try
            {
                this._pool.Wait(cancelToken);
                action();
            }
            finally
            {
                var dt = DateTime.Now.ToUnixTimeMilliseconds();
                //fire and forget
                Task.Run(() =>
                {
                    int sleepTime = (int)_resetSpan.TotalMilliseconds -
                                    (int)((dt - _startTime) % _resetSpan.TotalMilliseconds);
                    System.Threading.Thread.Sleep(sleepTime);
                    _pool.Release();
                });
            }
        }
        public async Task<TResult> RunAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancelToken)
        {

            try
            {
                this._pool.Wait(cancelToken);
                return await action();
            }
            finally
            {
                var dt = DateTime.Now.ToUnixTimeMilliseconds();
                //fire and forget
                Task.Factory.StartNew(() =>
                {
                    int sleepTime = (int) _resetSpan.TotalMilliseconds -
                                    (int) ((dt - _startTime) % _resetSpan.TotalMilliseconds);
                    System.Threading.Thread.Sleep(sleepTime);
                    _pool.Release();
                });
            }
        }

        public void Dispose()
        {
            this._pool.Dispose();
        }
    }
}