using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Policy;

namespace ProfileService.Api.Models.Utils
{
    [HtmlTargetElement("form-popup", Attributes ="id")]
    public class FormPopupTagHelper:TagHelper
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Size { get; set; } 
        public bool Lock { get; set; }
        public bool Scroll { get; set; }

        [HtmlAttributeName("save-close")]
        public bool SaveClose { get; set; }

        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.PreContent.SetHtmlContent(
                $@"<div class=""modal fade"" id=""{Id}"" {(Lock ? @"data-bs-backdrop=""static"" data-bs-keyboard=""false""" : " ")} tabindex=""-1"" aria-hidden=""true"">
                            <div class=""modal-dialog modal-dialog-centered {(Scroll ? "modal-dialog-scrollable" : "")} {(Size != null ? "modal-"+Size : "")}"">
                                <div class=""modal-content"">
                                    <div class=""modal-header"">
                                    <h5 class=""modal-title"">{Title??" "}</h5>
                                    <button type=""button"" class=""btn-close cancel"" data-bs-dismiss=""modal"" data-bs-target=""#{Id}"" aria-label=""Close"">
                                    </button>
                                </div>
                                <div id=""inner-{Id}"" class=""modal-body"">"
            );

            output.Content.AppendHtml((await output.GetChildContentAsync()));

            output.PostContent.SetHtmlContent(
                $@"</div>
                          <div class=""modal-footer"">
                            <button type=""submit"" class=""btn btn-secondary"" form=""form-{Id}"" data-bs-target=""{Id}"" {(SaveClose ? @"data-bs-dismiss=""modal""" : "")}>
                                Save
                            </button>
                          </div>
                        </div>
                    </div>
                </div>"
            );

            output.Attributes.Clear();
        }
    }
}
