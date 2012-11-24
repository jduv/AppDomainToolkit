namespace AppDomainToolkit
{
    using System;

    /// <summary>
    /// Static class for executing functions in foreign application domains.
    /// </summary>
    public static class RemoteFunc
    {
        #region Public Methods

        /// <summary>
        /// Invokes the target function.
        /// </summary>
        /// <typeparam name="TResult">
        /// The result type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to invoke the function in.
        /// </param>
        /// <param name="toInvoke">
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

        /// <summary>
        /// Invokes the target function.
        /// </summary>
        /// <typeparam name="T1">
        /// First argument type.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to invoke the function in.
        /// </param>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="toInvoke">
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

        /// <summary>
        /// Invokes the target function.
        /// </summary>
        /// <typeparam name="T1">
        /// First argument type.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Second argument type.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to invoke the function in.
        /// </param>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="toInvoke">
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

        /// <summary>
        /// Invokes the target function.
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
        /// <typeparam name="TResult">
        /// The result type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to invoke the function in.
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
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

        /// <summary>
        /// Invokes the target function.
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
        /// <typeparam name="TResult">
        /// The result type.
        /// </typeparam>
        /// <param name="domain">
        /// The domain to invoke the function in.
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
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

    /// <summary>
    /// Executes a function in another application domain.
    /// </summary>
    /// <typeparam name="TResult">
    /// The result type.
    /// </typeparam>
    public class RemoteFunc<TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteFunc class.
        /// </summary>
        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target function.
        /// </summary>
        /// <param name="toInvoke">
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

    /// <summary>
    /// Executes a function in another application domain.
    /// </summary>
    /// <typeparam name="T">
    /// First argument type.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The result type.
    /// </typeparam>
    public class RemoteFunc<T, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteFunc class.
        /// </summary>
        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target function.
        /// </summary>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="toInvoke">
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public TResult Invoke(T arg, Func<T, TResult> toInvoke)
        {
            if (toInvoke == null)
            {
                throw new ArgumentNullException("toInvoke");
            }

            return toInvoke.Invoke(arg);
        }

        #endregion
    }

    /// <summary>
    /// Executes a function in another application domain.
    /// </summary>
    /// <typeparam name="T1">
    /// First argument type.
    /// </typeparam>
    /// <typeparam name="T2">
    /// Second argument type.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The result type.
    /// </typeparam>
    public class RemoteFunc<T1, T2, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteFunc class.
        /// </summary>
        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target function.
        /// </summary>
        /// <param name="arg1">
        /// The first argument.
        /// </param>
        /// <param name="arg2">
        /// The second argument.
        /// </param>
        /// <param name="toInvoke">
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

    /// <summary>
    /// Executes a function in another application domain.
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
    /// <typeparam name="TResult">
    /// The result type.
    /// </typeparam>
    public class RemoteFunc<T1, T2, T3, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteFunc class.
        /// </summary>
        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target function.
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
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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

    /// <summary>
    /// Executes a function in another application domain.
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
    /// <typeparam name="TResult">
    /// The result type.
    /// </typeparam>
    public class RemoteFunc<T1, T2, T3, T4, TResult> : MarshalByRefObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Initializes a new instance of the RemoteFunc class.
        /// </summary>
        public RemoteFunc()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the target function.
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
        /// The function to invoke.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
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
