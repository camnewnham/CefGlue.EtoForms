using System;
using Xilium.CefGlue.Common.Helpers.Logger;

namespace CefGlue.Tests.Helpers
{
    internal class DummyLogger : ILogger
    {
        public bool IsTraceEnabled => throw new NotImplementedException();

        public bool IsDebugEnabled => throw new NotImplementedException();

        public bool IsErrorEnabled => throw new NotImplementedException();

        public bool IsFatalEnabled => throw new NotImplementedException();

        public bool IsInfoEnabled => throw new NotImplementedException();

        public bool IsWarnEnabled => throw new NotImplementedException();

        public void Debug(string message)
        {
        }

        public void DebugException(string message, Exception exception)
        {
        }

        public void Error(string message)
        {
        }

        public void ErrorException(string message, Exception exception)
        {
        }

        public void Fatal(string message)
        {
        }

        public void FatalException(string message, Exception exception)
        {
        }

        public void Info(string message)
        {
        }

        public void InfoException(string message, Exception exception)
        {
        }

        public void Trace(string message)
        {
        }

        public void TraceException(string message, Exception exception)
        {
        }

        public void Warn(string message)
        {
        }

        public void WarnException(string message, Exception exception)
        {
        }
    }
}
