﻿using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsIdQuery:PersonsQuery
    {
        public PersonsIdQuery(
            string[] ids,
            SortBy? sort = null
            ):base(sort, null)
        {
            Ids = ids;
        }
        public string[] Ids { get; }
    }
}
