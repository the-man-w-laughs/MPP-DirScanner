using System.Collections.Concurrent;

namespace Model.Services
{
    public class TaskQueue
    {
        private ConcurrentQueue<Task> _taskQueue;
		private ConcurrentQueue<Task> _waitQueue;        
        private readonly SemaphoreSlim _semaphore;
        private readonly CancellationToken _token;
        private readonly Task _startNext;
        private readonly Task _waitNext;
        public TaskQueue(ushort maxThreadCount, CancellationTokenSource tokenSource)
        {
            _token = tokenSource.Token;                        
            _startNext = new Task(() => StartNext(), _token);
            _waitNext = new Task(() => WaitNext(), _token);
            _semaphore = new SemaphoreSlim(maxThreadCount);
            _taskQueue = new ConcurrentQueue<Task>();
			_waitQueue = new ConcurrentQueue<Task>();           
        }

        public void Enqueue(Task task)
        {
            _waitQueue.Enqueue(task);
            _taskQueue.Enqueue(task);            
        }

        public void StartAndWaitAll()
        {
            _startNext.Start();
            _waitNext.Start();
            try
            {
                _startNext.Wait(_token);
                _waitNext.Wait(_token);
            }
            catch(OperationCanceledException)
            {
                return;
            }
        }

        private void StartNext()
        {
            Task? task;
            while(!_waitNext.IsCompleted && !_token.IsCancellationRequested)
            {
                bool res = _taskQueue.TryDequeue(out task);
                if (task != null)
                {
                    try
                    {
                        _semaphore.Wait(_token); // block current thread
                        task.Start(); // start task
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }

        private void WaitNext()
        {
            Task? task;
            while (!_waitQueue.IsEmpty && !_token.IsCancellationRequested)
            {
                bool res = _waitQueue.TryPeek(out task);
                if (res && task != null)
                {
                    try
                    {
                        task.Wait(_token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    finally
                    {
                        _semaphore.Release();
                        _waitQueue.TryDequeue(out _);
                    }
                }
            }
        }
    }
}