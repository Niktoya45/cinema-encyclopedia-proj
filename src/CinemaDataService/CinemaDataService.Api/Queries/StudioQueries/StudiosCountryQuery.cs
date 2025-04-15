﻿using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosCountryQuery:StudiosQuery
    {
        public StudiosCountryQuery(Country country, SortBy? sort = null, Pagination? pagination = null) : base(sort, pagination)
        {
            Country = country;
        }
        public Country Country { get; }
    }
}
