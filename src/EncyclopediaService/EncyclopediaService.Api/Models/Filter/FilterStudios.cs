﻿using EncyclopediaService.Api.Models.Display;
using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterStudios
    {
        // load information from db

        public List<int> YearsChoice = Enumerable.Range(1, 12).Select(i => DateTime.Now.Year - i * 10).ToList();

        [BindProperty(SupportsGet =true, Name ="yearsbind")]
        public List<int> YearsBind { get; set; } = new List<int>();

        [BindProperty(SupportsGet =true, Name ="countrybind")]
        public Country CountryBind { get; set; }
    }
}
