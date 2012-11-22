namespace AppDomainToolkit
{
    using System;

    /// <summary>
    /// Executes an action in another application domain.
    /// </summary>
    public class RemoteAction : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteAction class.
        /// </summary>
        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes an action remotely.
        /// </summary>
        /// <param name="domain">
        /// The domain to execute the action in.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <typeparam name="T1">
        /// First argument type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to execute the action in.
        /// </param>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <typeparam name="T1">
        /// First argument type.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Second argument type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to execute the action in.
        /// </param>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <typeparam name="T1">
        /// First argument type.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Second argument type.
        /// </typeparam>
        /// <typeparam name="T3">
        /// Third argument type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to execute the action in.
        /// </param>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="arg3">
        /// The third argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <typeparam name="T1">
        /// First argument type.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Second argument type.
        /// </typeparam>
        /// <typeparam name="T3">
        /// Third argument type.
        /// </typeparam>
        /// <typeparam name="T4">
        /// Fourth argument type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to execute the action in.
        /// </param>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="arg3">
        /// The third argument.
        /// </param>
        /// <param name="arg4">
        /// The fourth argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

    /// <summary>
    /// Executes an action in another application domain.
    /// </summary>
    /// <typeparam name="T">
    /// First argument type.
    /// </typeparam>
    public class RemoteAction<T> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteAction class.
        /// </summary>
        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

    /// <summary>
    /// Executes an action in another application domain.
    /// </summary>
    /// <typeparam name="T1">
    /// First argument type.
    /// </typeparam>
    /// <typeparam name="T2">
    /// Second argument type.
    /// </typeparam>
    public class RemoteAction<T1, T2> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteAction class.
        /// </summary>
        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

    /// <summary>
    /// Executes an action in another application domain.
    /// </summary>
    /// <typeparam name="T1">
    /// First argument type.
    /// </typeparam>
    /// <typeparam name="T2">
    /// Second argument type.
    /// </typeparam>
    /// <typeparam name="T3">
    /// Third argument type.
    /// </typeparam>
    public class RemoteAction<T1, T2, T3> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteAction class.
        /// </summary>
        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="arg3">
        /// The third argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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

    /// <summary>
    /// Executes an action in another application domain.
    /// </summary>
    /// <typeparam name="T1">
    /// First argument type.
    /// </typeparam>
    /// <typeparam name="T2">
    /// Second argument type.
    /// </typeparam>
    /// <typeparam name="T3">
    /// Third argument type.
    /// </typeparam>
    /// <typeparam name="T4">
    /// Fourth argument type.
    /// </typeparam>
    public class RemoteAction<T1, T2, T3, T4> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteAction class.
        /// </summary>
        public RemoteAction()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target action.
        /// </summary>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="arg3">
        /// The third argument.
        /// </param>
        /// <param name="arg4">
        /// The fourth argument.
        /// </param>
        /// <param name="toInvoke">
        /// The action to invoke.
        /// </param>
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
