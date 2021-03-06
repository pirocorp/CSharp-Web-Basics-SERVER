﻿namespace SIS.MvcFramework
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using HTTP;
    using HTTP.Responses;

    public abstract class Controller
    {
        public HttpRequest Request { get; set; }

        public string User => this.Request.SessionData.ContainsKey("UserId")
            ? this.Request.SessionData["UserId"]
            : null;

        protected HttpResponse View<T>(T viewModel = null, 
            [CallerMemberName] string viewName = null)
            where T : class
        {
            var controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            var viewPath = "Views/" + controllerName + "/" + viewName + ".html";

            return this.ViewByName<T>(viewPath, viewModel);
        }

        protected HttpResponse View([CallerMemberName] string viewName = null)
        {
            return this.View<object>(null, viewName);
        }

        protected HttpResponse Error(string error)
        {
            var errorModel = new ErrorViewModel
            {
                Error = error,
            };

            return this.ViewByName<ErrorViewModel>("Views/Shared/Error.html", errorModel);
        }

        protected HttpResponse Redirect(string url)
        {
            return new RedirectResponse(url);
        }

        protected void SignIn(string userId)
        {
            this.Request.SessionData["UserId"] = userId;
        }

        protected void SignOut()
        {
            this.Request.SessionData["UserId"] = null;
        }

        protected bool IsUserLoggedIn()
            => this.User != null;

        private HttpResponse ViewByName<T>(string viewPath, object viewModel)
        {
            IViewEngine viewEngine = new ViewEngine();

            var templateHtml = File.ReadAllText(viewPath);
            var html = viewEngine.GetHtml(templateHtml, viewModel, this.User);

            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var page = layout.Replace("@RenderBody()", html);

            page = viewEngine.GetHtml(page, viewModel, this.User);

            return new HtmlResponse(page);
        }
    }
}
