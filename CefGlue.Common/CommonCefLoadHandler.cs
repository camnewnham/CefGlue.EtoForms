namespace Xilium.CefGlue.Common
{
    public class CommonCefLoadHandler : CefLoadHandler
    {
        private readonly ICefBrowserHost _owner;

        public CommonCefLoadHandler(ICefBrowserHost owner)
        {
            _owner = owner;
        }

        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            _owner.HandleLoadingStateChange(new LoadingStateChangeEventArgs(isLoading, canGoBack, canGoForward));
        }

        protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            _owner.HandleLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl));
        }

        protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
        {
            _owner.HandleLoadStart(new LoadStartEventArgs(frame));
        }

        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            _owner.HandleLoadEnd(new LoadEndEventArgs(frame, httpStatusCode));
        }
    }
}
