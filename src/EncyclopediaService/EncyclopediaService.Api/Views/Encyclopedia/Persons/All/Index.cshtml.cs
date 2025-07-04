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
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons.All
{
    public class IndexModel : PageModel
    {
        protected IGatewayService _gatewayService { get; init; }

        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name = "pagen")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }

        [BindProperty(SupportsGet = true)]
        public FilterPersons _filterOptions { get; set; }
        public SortPersons _sortOptions { get; init; }

        [BindProperty(SupportsGet = true, Name = "order")]
        public bool? Order { get; set; }

        [BindProperty(SupportsGet = true, Name = "by")]
        public string? By { get; set; }

        public const string currentPage = "/Encyclopedia/Persons/All/Index";
        public IList<PersonRecord> List { get; set; }

        public IndexModel(IGatewayService gatewayService, SortPersons sortOptions)
        {
            _gatewayService = gatewayService;

            _sortOptions = sortOptions;

            _filterOptions = new FilterPersons();

        }
        public async Task<IActionResult> OnGet(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.PersonsList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetPersons(
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                HandleResponse(response);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetJobs(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;


            if (TestRecords.Used)
            {
                List = TestRecords.PersonsList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                if (!_filterOptions.JobsBind.Any())
                {
                    return RedirectToPage(currentPage);
                }

                var response = await _gatewayService.GetPersonsByJobs(
                        _filterOptions.JobsBind.Aggregate((acc, j) => acc | j),
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                HandleResponse(response);
            }

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetCountry(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;


            if (TestRecords.Used)
            {
                List = TestRecords.PersonsList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetPersonsByCountry(
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
                List = TestRecords.PersonsList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetPersonsBySearchPage(
                        _filterOptions.Search,
                        ct,
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );
                HandleResponse(response);
            }

            return Page();
        }

        protected void HandleResponse(Page<PersonsResponse>? response)
        {
            if (response is null)
            {
                List = new List<PersonRecord>();
                IsEnd = true;
            }
            else
            {
                List = response.Response.Select(p => new PersonRecord
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = p.Picture,
                    PictureUri = p.PictureUri,
                    Jobs = p.Jobs
                }).ToArray();
                IsEnd = response.IsEnd;
            }
        }
    }
}
