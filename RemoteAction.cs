namespace AppDomainToolkit
{
    using System;

    public class RemoteAction : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        public static void Invoke(AppDomain domain, Action toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteAction>.CreateProxy(domain);
            proxy.RemoteObject.Invoke(toInvoke);
        }

        public static void Invoke<T>(AppDomain domain, T arg, Action<T> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteAction<T>>.CreateProxy(domain);
            proxy.RemoteObject.Invoke(arg, toInvoke);
        }

        public static void Invoke<T1, T2>(AppDomain domain, T1 arg1, T2 arg2, Action<T1, T2> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteAction<T1, T2>>.CreateProxy(domain);
            proxy.RemoteObject.Invoke(arg1, arg2, toInvoke);
        }

        public static void Invoke<T1, T2, T3>(AppDomain domain, T1 arg1, T2 arg2, T3 arg3, Action<T1, T2, T3> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteAction<T1, T2, T3>>.CreateProxy(domain);
            proxy.RemoteObject.Invoke(arg1, arg2, arg3, toInvoke);
        }

        public static void Invoke<T1, T2, T3, T4>(AppDomain domain, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<T1, T2, T3, T4> toInvoke)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            var proxy = Remote<RemoteAction<T1, T2, T3, T4>>.CreateProxy(domain);
            proxy.RemoteObject.Invoke(arg1, arg2, arg3, arg4, toInvoke);
        }

        public void Invoke(Action toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            toInvoke.Invoke();
        }

        #endregion
    }

    public class RemoteAction<T> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        public void Invoke(T arg1, Action<T> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            toInvoke.Invoke(arg1);
        }

        #endregion
    }

    public class RemoteAction<T1, T2> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        public void Invoke(T1 arg1, T2 arg2, Action<T1, T2> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            toInvoke.Invoke(arg1, arg2);
        }

        #endregion
    }

    public class RemoteAction<T1, T2, T3> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, Action<T1, T2, T3> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            toInvoke.Invoke(arg1, arg2, arg3);
        }

        #endregion
    }

    public class RemoteAction<T1, T2, T3, T4> : MarshalByRefObject
    {
        #region Constructors & Destructors

        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<T1, T2, T3, T4> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            toInvoke.Invoke(arg1, arg2, arg3, arg4);
        }

        #endregion
    }
}
