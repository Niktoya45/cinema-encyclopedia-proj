using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using MongoDB.Driver;
using System.Linq.Expressions;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Domain.Aggregates.PersonAggregate;

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
        public async Task<List<Studio>?> FindByYear(int year, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find( s => s.FoundDate.Year == year,
                                pg,
                                st,
                                ct
                );
        }
        public async Task<List<Studio>?> FindByCountry(Country country, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find(s => s.Country == country,
                    pg,
                    st,
                    ct
    );
        }

        public async Task<CinemaRecord?> FindFilmographyById(string? personId, string filmographyId, CancellationToken ct)
        {
            FilterDefinition<Studio> elementMatch = personId == null
                                        ? Builders<Studio>.Filter.ElemMatch(s => s.Filmography, c => c.Id == filmographyId)
                                        : Builders<Studio>.Filter.ElemMatch(s => s.Filmography, c => c.Id == filmographyId)
                                          & Builders<Studio>.Filter.Where(s => s.Id == personId);

            Studio? person = await FindOne(elementMatch, ct);

            if (person is null)
            {
                return null;
            }

            return person.Filmography!.FirstOrDefault(c => c.Id == filmographyId);
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
        public override async Task<Studio?> UpdateMain(Studio entity, CancellationToken ct = default)
        {
            UpdateDefinitionBuilder<Studio> builder = Builders<Studio>.Update;

            UpdateDefinition<Studio> Set<TField>(Expression<Func<Studio, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Studio>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.FoundDate, entity.FoundDate),
                    Set(e => e.Country, entity.Country),
                    Set(e => e.PresidentName, entity.PresidentName),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Studio>.Filter.Where(e => e.Id == entity.Id);



            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            Studio? studio = await FindOne(find, ct);

            if (studio is null)
            {
                return null;
            }

            return studio;
        }
        public async Task<CinemaRecord?> UpdateFilmography(string? studioId, CinemaRecord cinema, CancellationToken ct = default)
        {
            FilterDefinition<Studio> elementMatch = studioId == null
                                                    ? Builders<Studio>.Filter.ElemMatch(s => s.Filmography, c => c.Id == cinema.Id)
                                                    : Builders<Studio>.Filter.ElemMatch(s => s.Filmography, c => c.Id == cinema.Id)
            & Builders<Studio>.Filter.Where(s => s.Id == studioId);

            UpdateDefinition<Studio> replace = Builders<Studio>.Update.Set("Filmography.$", cinema);

            UpdateResult? res;

            if (studioId is null)
            {
                res = await _collection.UpdateManyAsync(elementMatch, replace, cancellationToken: ct);
            }
            else
            {
                res = await _collection.UpdateOneAsync(elementMatch, replace, cancellationToken: ct);
            }


            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }
        public async Task<CinemaRecord?> DeleteFromFilmography(string? studioId, CinemaRecord cinema, CancellationToken ct = default)
        {
            FilterDefinition<Studio> elementMatch = Builders<Studio>.Filter.ElemMatch(c => c.Filmography, s => s.Id == cinema.Id);
            UpdateDefinition<Studio> delete = Builders<Studio>.Update.PullFilter(c => c.Filmography, s => s.Id == cinema.Id);

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
