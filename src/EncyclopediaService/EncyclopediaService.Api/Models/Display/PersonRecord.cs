﻿using Shared.CinemaDataService.Models.Flags;

namespace EncyclopediaService.Api.Models.Display
{
    public class PersonRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }
        public Job Jobs { get; set; }
    }
}
