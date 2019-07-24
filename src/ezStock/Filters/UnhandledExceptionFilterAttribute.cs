using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace ezStock.Filters
{
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ConcurrentDictionary<Type, HttpStatusCode> _statusHandlers = new ConcurrentDictionary<Type, HttpStatusCode>();
        private readonly ConcurrentDictionary<Type, Func<Exception, HttpRequest, HttpResponse>> _delegateHandlers = new ConcurrentDictionary<Type, Func<Exception, HttpRequest, HttpResponse>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionFilterAttribute"/> class.
        /// </summary>
        public UnhandledExceptionFilterAttribute()
        {
        }

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(ExceptionContext context)
        {
            if (context == null || context.Exception == null)
            {
                return;
            }

            var exception = context.Exception.GetBaseException();
            var request = context.HttpContext.Request;
            var type = context.Exception.GetType();

            if (_delegateHandlers.TryGetValue(type, out var handler))
            {
                context.HttpContext.Response.StatusCode = handler(exception, request).StatusCode;
                context.HttpContext.Response.Body = handler(exception, request).Body;
            }
            else if (_statusHandlers.TryGetValue(type, out var statusCode))
            {
                context.HttpContext.Response.StatusCode = (int)statusCode;
            }
            // other exception will handle by System.Web.Http.ExceptionHandling.DefaultExceptionHandler
            // https://github.com/aspnet/AspNetWebStack/blob/master/src/System.Web.Http/ExceptionHandling/DefaultExceptionHandler.cs
        }

        /// <summary>
        /// Registers an exception handler that returns the specified status code for exceptions of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of exception to register a handler for.</typeparam>
        /// <param name="statusCode">The HTTP status code to return for exceptions of type <typeparamref name="TException"/>.</param>
        /// <returns>
        /// This <see cref="UnhandledExceptionFilterAttribute"/> after the exception handler has been added.
        /// </returns>
        public UnhandledExceptionFilterAttribute Register<TException>(HttpStatusCode statusCode)
            where TException : Exception
        {
            _statusHandlers.AddOrUpdate(typeof(TException), statusCode, (key, oldValue) => statusCode);

            return this;
        }

        /// <summary>
        /// Registers the specified exception <paramref name="handler"/> for exceptions of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of exception to register the <paramref name="handler"/> for.</typeparam>
        /// <param name="handler">The exception handler responsible for exceptions of type <typeparamref name="TException"/>.</param>
        /// <returns>
        /// This <see cref="UnhandledExceptionFilterAttribute"/> after the exception <paramref name="handler"/>
        /// has been added.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="handler"/> is <see langword="null"/>.</exception>
        public UnhandledExceptionFilterAttribute Register<TException>(Func<Exception, HttpRequest, HttpResponse> handler)
            where TException : Exception
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _delegateHandlers.AddOrUpdate(typeof(TException), handler, (key, oldValue) => handler);

            return this;
        }

        /// <summary>
        /// Unregisters the exception handler for exceptions of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of exception to unregister handlers for.</typeparam>
        /// <returns>
        /// This <see cref="UnhandledExceptionFilterAttribute"/> after the exception handler
        /// for exceptions of type <typeparamref name="TException"/> has been removed.
        /// </returns>
        public UnhandledExceptionFilterAttribute Unregister<TException>()
            where TException : Exception
        {
            _statusHandlers.TryRemove(typeof(TException), out var statusCode);
            _delegateHandlers.TryRemove(typeof(TException), out var handler);
            return this;
        }
    }
}