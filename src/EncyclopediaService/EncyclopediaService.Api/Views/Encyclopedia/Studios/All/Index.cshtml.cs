using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Filter;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Test;
using EncyclopediaService.Api.Models.TestData;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.GatewayService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.StudioDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Studios.All
{
    public class IndexModel : PageModel
    {
        protected IGatewayService _gatewayService { get; init; }

        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name = "pagen")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }

        [BindProperty(SupportsGet=true)]
        public FilterStudios _filterOptions { get; set; }

        public SortStudios _sortOptions { get; init; }

        [BindProperty(SupportsGet = true, Name = "order")]
        public bool? Order { get; set; }

        [BindProperty(SupportsGet = true, Name = "by")]
        public string? By { get; set; }

        public const string currentPage = "/Encyclopedia/Studios/All/Index";
        public IList<StudioRecord> List { get; set; }

        public IndexModel(IGatewayService gatewayService, SortStudios sortOptions)
        {
            _gatewayService = gatewayService;

            _sortOptions = sortOptions;

            _filterOptions = new FilterStudios();
        }
        public async Task<IActionResult> OnGet(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.StudiosList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetStudios(
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                HandleResponse(response);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetYears(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.StudiosList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetStudiosByYearSpans(
                        _filterOptions.YearsBind.ToArray(),
                        FilterStudios.YearSpan,
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                HandleResponse(response);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetCountry(CancellationToken ct) {

            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.StudiosList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetStudiosByCountry(
                        _filterOptions.CountryBind,
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                HandleResponse(response);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetSearch(CancellationToken ct)
        {
            if (_filterOptions.Search is null || _filterOptions.Search == "")
            {
                return RedirectToPage(currentPage);
            }

            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.StudiosList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetStudiosBySearchPage(
                        _filterOptions.Search,
                        ct,
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                HandleResponse(response);
            }

            return Page();
        }

        protected void HandleResponse(Page<StudiosResponse>? response)
        {
            if (response is null)
            {
                List = new List<StudioRecord>();
                IsEnd = true;
            }
            else
            {
                List = response.Response.Select(c => new StudioRecord
                {
                    Id = c.Id,
                    Name = c.Name,
                    Picture = c.Picture,
                    PictureUri = c.PictureUri
                }).ToArray();
                IsEnd = response.IsEnd;
            }
        }
    }
}
