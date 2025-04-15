﻿using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using MongoDB.Driver;
using System.Linq.Expressions;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Infrastructure.Repositories.Implementations
{
    public class StudioRepository:EntityRepository<Studio>, IStudioRepository
    {
        public StudioRepository(CinemaDataDb db)
        {
            _collection = db.Studios;
        }
        public async Task<CinemaRecord?> AddToFilmography(string studioId, CinemaRecord cinema, CancellationToken ct = default)
        {
            return await AddRecord( s => s.Id == studioId,
                                    s => s.Filmography,
                                    cinema,
                                    c => c.Id == cinema.Id,
                                    ct
                );
        }
        public async Task<List<Studio>?> FindByYear(int year, Pagination.Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find( s => s.FoundDate.Year == year,
                                pg,
                                st,
                                ct
                );
        }
        public async Task<List<Studio>?> FindByCountry(Country country, Pagination.Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find(s => s.Country == country,
                    pg,
                    st,
                    ct
    );
        }
        public override async Task<Studio?> Update(Studio entity, CancellationToken ct = default)
        {
            UpdateDefinitionBuilder<Studio> builder = Builders<Studio>.Update;

            UpdateDefinition<Studio> Set<TField>(Expression<Func<Studio, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Studio>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.FoundDate, entity.FoundDate),
                    Set(e => e.Country, entity.Country),
                    Set(e => e.Picture, entity.Picture),
                    Set(e => e.Filmography, entity.Filmography),
                    Set(e => e.PresidentName, entity.PresidentName),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Studio>.Filter.Where(e => e.Id == entity.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return entity;
        }
        public async Task<CinemaRecord?> UpdateFilmography(CinemaRecord cinema, CancellationToken ct = default)
        {
            FilterDefinition<Studio> elementMatch = Builders<Studio>.Filter.ElemMatch(c => c.Filmography, s => s.Id == cinema.Id);
            UpdateDefinition<Studio> replace = Builders<Studio>.Update.Set("Starrings.$", cinema);

            var res = await _collection.UpdateManyAsync(elementMatch, replace, cancellationToken: ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }
        public async Task<CinemaRecord?> DeleteFromFilmography(string? studioId, CinemaRecord cinema, CancellationToken ct = default)
        {
            FilterDefinition<Studio> elementMatch = Builders<Studio>.Filter.ElemMatch(c => c.Filmography, s => s.Id == cinema.Id);
            UpdateDefinition<Studio> delete = Builders<Studio>.Update.PopFirst("Filmography.$");

            UpdateResult res;

            if (studioId is null)
            {
                res = await _collection.UpdateManyAsync(elementMatch, delete, cancellationToken: ct);
            }
            else
            {
                FilterDefinition<Studio> find = Builders<Studio>.Filter.Where(c => c.Id == studioId);

                res = await _collection.UpdateOneAsync(find & elementMatch, delete, cancellationToken: ct);
            }

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }
    }
}
