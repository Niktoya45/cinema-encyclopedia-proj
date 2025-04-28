using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace EncyclopediaService.Api.Views
{
    [IgnoreAntiforgeryToken]
    public class ErrorModel:PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ExceptionMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature?.Path == "/")
            {
                string ln = "\n";

                ExceptionMessage ??= string.Empty;
                ExceptionMessage += "Message: " + exceptionHandlerPathFeature.Error?.Message + ln;
                ExceptionMessage += "Page: " + exceptionHandlerPathFeature.Path + ln;
                ExceptionMessage += "Route Values: " + ln;

                foreach (var routeValue in exceptionHandlerPathFeature.RouteValues)
                {
                    ExceptionMessage += "\t" + routeValue.Key + " : " + routeValue.Value + ln;
                }
            }
            else
            {
                ExceptionMessage = "Model state was invalid.";
            }

            return Page();
        }
    }
}
