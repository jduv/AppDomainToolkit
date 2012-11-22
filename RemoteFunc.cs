namespace AppDomainToolkit
{
    using System;

    public static class RemoteFunc
    {
        #region Public Methods

        public static TResult Invoke<TResult>(AppDomain domain, Func<TResult> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteFunc<TResult>>.CreateProxy(domain);
            return proxy.RemoteObject.Invoke(toInvoke);
        }

        public static TResult Invoke<T, TResult>(AppDomain domain, T arg1, Func<T, TResult> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteFunc<T, TResult>>.CreateProxy(domain);
            return proxy.RemoteObject.Invoke(arg1, toInvoke);
        }

        public static TResult Invoke<T1, T2, TResult>(AppDomain domain, T1 arg1, T2 arg2, Func<T1, T2, TResult> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteFunc<T1, T2, TResult>>.CreateProxy(domain);
            return proxy.RemoteObject.Invoke(arg1, arg2, toInvoke);
        }

        public static TResult Invoke<T1, T2, T3, TResult>(AppDomain domain, T1 arg1, T2 arg2, T3 arg3, Func<T1, T2, T3, TResult> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteFunc<T1, T2, T3, TResult>>.CreateProxy(domain);
            return proxy.RemoteObject.Invoke(arg1, arg2, arg3, toInvoke);
        }

        public static TResult Invoke<T1, T2, T3, T4, TResult>(AppDomain domain, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<T1, T2, T3, T4, TResult> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteFunc<T1, T2, T3, T4, TResult>>.CreateProxy(domain);
            return proxy.RemoteObject.Invoke(arg1, arg2, arg3, arg4, toInvoke);
        }

        #endregion
    }

    public class RemoteFunc<TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        public TResult Invoke(Func<TResult> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            return toInvoke.Invoke();
        }

        #endregion
    }

    public class RemoteFunc<T, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        public  TResult Invoke(T arg, Func<T, TResult> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            return toInvoke.Invoke(arg);
        }

        #endregion
    }

    public class RemoteFunc<T1, T2, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        public TResult Invoke(T1 arg1, T2 arg2, Func<T1, T2, TResult> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            return toInvoke.Invoke(arg1, arg2);
        }

        #endregion
    }

    public class RemoteFunc<T1, T2, T3, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, Func<T1, T2, T3, TResult> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            return toInvoke.Invoke(arg1, arg2, arg3);
        }

        #endregion
    }

    public class RemoteFunc <T1, T2, T3, T4, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<T1, T2, T3, T4, TResult> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            return toInvoke.Invoke(arg1, arg2, arg3, arg4);
        }

        #endregion
    }
}
