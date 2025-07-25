﻿using Shared.CinemaDataService.Models.Flags;
using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterStudios
    {
        // load information from db
        public const int YearSpan = 10;

        public List<int> YearsChoice = Enumerable.Range(1, 12).Select(i => DateTime.Now.Year - i * YearSpan).ToList();

        [BindProperty(SupportsGet =true, Name ="yearsbind")]
        public List<int> YearsBind { get; set; } = new List<int>();

        [BindProperty(SupportsGet =true, Name ="countrybind")]
        public Country CountryBind { get; set; }


        [BindProperty(SupportsGet = true, Name = "q")]
        public string? Search { get; set; } = default;
    }
}
