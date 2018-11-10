using System;
using System.Threading.Tasks;
using Quartz;

namespace MUtils.Quartz
{
    public abstract class DisposableJob : IJob,IDisposable
    {
        private IJobExecutionContext _context;
        private bool _disposed;
        public virtual async Task Execute(IJobExecutionContext context)
        {
            _context = context;
        }

        protected abstract void DisposeServices();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (!disposing) return;

            DisposeServices();
            _disposed = true;
        }
    }
}