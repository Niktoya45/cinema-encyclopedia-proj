using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Filter;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Test;
using EncyclopediaService.Api.Models.TestData;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.GatewayService;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas.All
{
    public class IndexModel : PageModel
    {
        protected IGatewayService _gatewayService { get; init; }

        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name ="pagen")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }


        [BindProperty(SupportsGet=true)]
        public FilterCinemas _filterOptions {  get; set; }
        public SortCinemas _sortOptions { get; init; }
        public IList<CinemaRecord> List { get; set; }

        [BindProperty(SupportsGet=true, Name ="order")]
        public bool? Order { get; set; } = null;

        [BindProperty(SupportsGet=true, Name ="by")]
        public string? By { get; set; }

        public const string currentPage = "/Encyclopedia/Cinemas/All/Index";

        public IndexModel(IGatewayService gatewayService, SortCinemas sortOptions)
        {
            _gatewayService = gatewayService;

            _sortOptions = sortOptions;

            _filterOptions = new FilterCinemas();
        }

        public async Task<IActionResult> OnGet(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.CinemasList;
                _filterOptions.StudiosChoice = TestRecords.StudiosChoice;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {    
                var response = await _gatewayService.GetCinemas(
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                await HandleResponse(response, ct);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetYears(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.CinemasList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                if (!_filterOptions.YearsBind.Any())
                {
                    return RedirectToPage(currentPage);
                }

                var response = await _gatewayService.GetCinemasByYearSpans(
                        _filterOptions.YearsBind.ToArray(),
                        FilterCinemas.YearSpan,
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                await HandleResponse(response, ct);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetGenres(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;

            if (TestRecords.Used)
            {
                List = TestRecords.CinemasList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                if (!_filterOptions.GenresBind.Any())
                {
                    return RedirectToPage(currentPage);
                }

                var response = await _gatewayService.GetCinemasByGenre(
                        _filterOptions.GenresBind.Aggregate((acc, g) => acc | g),
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                await HandleResponse(response, ct);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetLanguage(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;


            if (TestRecords.Used)
            {
                List = TestRecords.CinemasList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                if (_filterOptions.LanguageBind == 0)
                {
                    return RedirectToPage(currentPage);
                }

                var response = await _gatewayService.GetCinemasByLanguage(
                        _filterOptions.LanguageBind,
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                await HandleResponse(response, ct);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetStudios(CancellationToken ct)
        {
            if (PageNum < 1) PageNum = 1;


            if (TestRecords.Used)
            {
                List = TestRecords.CinemasList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                if (!_filterOptions.StudiosBind.Any())
                {
                    return RedirectToPage(currentPage);
                }

                var response = await _gatewayService.GetCinemasByStudioId(
                        _filterOptions.StudiosBind.FirstOrDefault(),
                        ct,
                        new SortBy(Order, By),
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                await HandleResponse(response, ct);
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
                List = TestRecords.CinemasList;

                IsEnd = List.Count < MaxPerPage;
            }
            else
            {
                var response = await _gatewayService.GetCinemasBySearchPage(
                        _filterOptions.Search,
                        ct,
                        new Pagination((PageNum - 1) * MaxPerPage, MaxPerPage)
                    );

                await HandleResponse(response, ct);
            }

            return Page();
        }

        protected async Task HandleResponse(Page<CinemasResponse>? response, CancellationToken ct)
        {
            if (response is null)
            {
                List = new List<CinemaRecord>();
                IsEnd = true;
            }
            else
            {
                List = response.Response.Select(c => new CinemaRecord
                {
                    Id = c.Id,
                    Name = c.Name,
                    Picture = c.Picture,
                    PictureUri = c.PictureUri,
                    Year = c.Year,
                    Rating = c.Rating
                }).ToArray();
                IsEnd = response.IsEnd;

                var responseStudios = await _gatewayService.GetStudios(ct, new SortBy(true, "Name"));

                if (responseStudios == null)
                    _filterOptions.StudiosChoice = new List<StudioRecord>();
                else _filterOptions.StudiosChoice = responseStudios.Response.Select(s => new StudioRecord
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            }
        }
    }
}
