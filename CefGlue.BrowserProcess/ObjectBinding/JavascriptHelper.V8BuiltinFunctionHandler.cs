using System;
using System.Threading.Tasks;

namespace Xilium.CefGlue.BrowserProcess.ObjectBinding
{
    partial class JavascriptHelper
    {
        private class V8BuiltinFunctionHandler : CefV8Handler
        {
            private readonly INativeObjectRegistry _nativeObjectRegistry;

            public V8BuiltinFunctionHandler(INativeObjectRegistry nativeObjectRegistry)
            {
                _nativeObjectRegistry = nativeObjectRegistry;
            }

            protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
            {
                returnValue = null;
                exception = null;
                
                switch (name)
                {
                    case BindNativeFunctionName:
                        {
                            var (valid, argException) = CheckArguments(arguments, a => a.IsString);
                            if (valid)
                            {
                                var objectName = arguments[0].GetStringValue();
                                using (var context = CefV8Context.GetCurrentContext().EnterOrFail(shallDispose: false)) // context will be released when promise is resolved
                                {
                                    var resultingPromise = context.V8Context.CreatePromise();
                                    returnValue = resultingPromise.Promise;

                                    var boundQueryTask = _nativeObjectRegistry.Bind(objectName);

                                    boundQueryTask.ContinueWith(t =>
                                    {
                                        using (resultingPromise)
                                        //using (context.V8Context.EnterOrFail())
                                        {
                                            resultingPromise.ResolveOrReject((resolve, reject) =>
                                            {
                                                if (t.IsFaulted)
                                                {
                                                    using (var exceptionMsg = CefV8Value.CreateString(t.Exception.Message))
                                                    {
                                                        reject(exceptionMsg);
                                                    }
                                                }
                                                else
                                                {
                                                    using (var result = CefV8Value.CreateBool(t.Result))
                                                    {
                                                        resolve(result);
                                                    }
                                                }
                                            });
                                        }
                                    }, TaskContinuationOptions.ExecuteSynchronously);
                                }
                            }
                            else
                            {
                                exception = argException;
                            }

                            break;
                        }

                    case UnbindNativeFunctionName:
                        {
                            var (valid, argException) = CheckArguments(arguments, a => a.IsString);
                            if (valid)
                            {
                                var objectName = arguments[0].GetStringValue();
                                _nativeObjectRegistry.Unbind(objectName);
                            }

                            break;
                        }

                    default:

                        exception = $"Unknown function '{name}'";
                        break;
                }
                
                return true;
            }
        }

        private static (bool, string) CheckArguments(CefV8Value[] arguments, params Func<CefV8Value, bool>[] argsValidators)
        {
            if (arguments.Length != argsValidators.Length)
            {
                return (false, $"Expected {argsValidators.Length} arguments but got {arguments.Length}");
            }

            for(var i = 0; i < arguments.Length; i++)
            {
                if (!argsValidators[i](arguments[i]))
                {
                    return (false, $"Unexpected argument type in position {i+1}");
                }
            }

            return (true, "");
        }
    }
}
