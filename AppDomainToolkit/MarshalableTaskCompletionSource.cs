namespace AppDomainToolkit
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Marshalable TaskCompletionSource
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MarshalableTaskCompletionSource<T> : MarshalByRefObject
    {
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

        public Task<T> Task
        {
            get { return tcs.Task; }
        }

        public void SetResult(T result)
        {
            this.tcs.SetResult(result);
        }

        public void SetException(Exception[] exception)
        {
            this.tcs.SetException(exception);
        }

        public void SetCanceled()
        {
            this.tcs.SetCanceled();
        }
    }
}