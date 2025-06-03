using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CinemaDataService.Infrastructure.Repositories.Implementations
{
    public class CinemaRepository : EntityRepository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(CinemaDataDb db)
        {
            _collection = db.Cinemas;
        }
        public async Task<StudioRecord?> AddProductionStudio(string cinemaId, StudioRecord studio, CancellationToken ct)
        {
            return await AddRecord( c => c.Id == cinemaId,
                                    c => c.ProductionStudios,
                                    studio,
                                    s => s.Id == studio.Id,
                                    ct
                                   );
        }
        public async Task<Starring?> AddStarring(string? cinemaId, Starring starring, CancellationToken ct)
        {
            return await AddRecord(c => c.Id == cinemaId,
                                   c => c.Starrings,
                                    starring,
                                  s => s.Id == starring.Id,
                                    ct
                                   );
        }
        public async Task<List<Cinema>?> FindByYear(int year, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find(c => c.ReleaseDate.Year == year,
                              pg,
                              st,
                              ct
                              );
        }
        public async Task<List<Cinema>?> FindByGenres(Genre genre, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find(c => (c.Genres & genre) != 0,
                               pg,
                               st,
                               ct);
        }
        public async Task<List<Cinema>?> FindByLanguage(Language language, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find(c => c.Language == language,
                              pg,
                              st,
                              ct
                );
        }
        public async Task<List<Cinema>?> FindByStudioId(string studioId, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {

            FilterDefinition<Cinema> condition = Builders<Cinema>.Filter.ElemMatch(c => c.ProductionStudios, s => s.Id == studioId);

            return await Find(condition, pg, st, ct);

        }

        public async Task<StudioRecord?> FindProductionStudioById(string? cinemaId, string studioId, CancellationToken ct) 
        {
            FilterDefinition<Cinema> elementMatch = cinemaId == null
                                        ? Builders<Cinema>.Filter.ElemMatch(c => c.ProductionStudios, s => s.Id == studioId)
                                        : Builders<Cinema>.Filter.ElemMatch(c => c.ProductionStudios, s => s.Id == studioId)
                                          & Builders<Cinema>.Filter.Where(c => c.Id == cinemaId);
           
            Cinema? cinema = await FindOne(elementMatch, ct);

            if (cinema is null)
            {
                return null;
            }

            return cinema.ProductionStudios!.FirstOrDefault(s => s.Id == studioId);

        }
        public async Task<Starring?> FindStarringById(string? cinemaId, string starringId, CancellationToken ct)
        {
            FilterDefinition<Cinema> elementMatch = cinemaId == null
                                        ? Builders<Cinema>.Filter.ElemMatch(c => c.Starrings, s => s.Id == starringId)
                                        : Builders<Cinema>.Filter.ElemMatch(c => c.Starrings, s => s.Id == starringId)
                                          & Builders<Cinema>.Filter.Where(c => c.Id == cinemaId);

            Cinema? cinema = await FindOne(elementMatch, ct);

            if (cinema is null)
            {
                return null;
            }

            return cinema.Starrings!.FirstOrDefault(s => s.Id == starringId);
        }

        public override async Task<Cinema?> Update(Cinema entity, CancellationToken ct)
        {
            UpdateDefinitionBuilder<Cinema> builder = Builders<Cinema>.Update;

            UpdateDefinition<Cinema> Set<TField>(Expression<Func<Cinema, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Cinema>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.ReleaseDate, entity.ReleaseDate),
                    Set(e => e.Genres, entity.Genres),
                    Set(e => e.Language, entity.Language),
                    Set(e => e.Picture, entity.Picture),
                    Set(e => e.ProductionStudios, entity.ProductionStudios),
                    Set(e => e.Starrings, entity.Starrings),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Cinema>.Filter.Where(e => e.Id == entity.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return entity;
        }
        public override async Task<Cinema?> UpdateMain(Cinema entity, CancellationToken ct)
        {
            UpdateDefinitionBuilder<Cinema> builder = Builders<Cinema>.Update;

            UpdateDefinition<Cinema> Set<TField>(Expression<Func<Cinema, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Cinema>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.ReleaseDate, entity.ReleaseDate),
                    Set(e => e.Genres, entity.Genres),
                    Set(e => e.Language, entity.Language),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Cinema>.Filter.Where(e => e.Id == entity.Id);

            Cinema? cinema = await FindOne(find, ct);

            if (cinema is null)
            {
                return null;
            }

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }
        public async Task<StudioRecord?> UpdateProductionStudio(string? cinemaId, StudioRecord studio, CancellationToken ct)
        {

            FilterDefinition<Cinema> elementMatch = cinemaId == null 
                                                    ? Builders<Cinema>.Filter.ElemMatch(c => c.ProductionStudios, s => s.Id == studio.Id)
                                                    : Builders<Cinema>.Filter.ElemMatch(c => c.ProductionStudios, s => s.Id == studio.Id)
                                                      & Builders<Cinema>.Filter.Where(c => c.Id == cinemaId);

            UpdateDefinition<Cinema> replace = Builders<Cinema>.Update.Set("ProductionStudios.$", studio);

            UpdateResult? res;

            if (cinemaId is null)
            {
                res = await _collection.UpdateManyAsync(elementMatch, replace, cancellationToken: ct);
            }
            else 
            {
                res = await _collection.UpdateOneAsync(elementMatch, replace, cancellationToken:ct);
            }


            if (!res.IsAcknowledged)
            {
                return null;
            }

            return studio;
        }
        public async Task<Starring?> UpdateStarring(string? cinemaId, Starring starring, CancellationToken ct)
        {
            FilterDefinition<Cinema> elementMatch = cinemaId == null
                                        ? Builders<Cinema>.Filter.ElemMatch(c => c.Starrings, s => s.Id == starring.Id)
                                        : Builders<Cinema>.Filter.ElemMatch(c => c.Starrings, s => s.Id == starring.Id)
                                          & Builders<Cinema>.Filter.Where(c => c.Id == cinemaId);

            UpdateDefinition<Cinema> replace = Builders<Cinema>.Update.Set("Starrings.$", starring);

            UpdateResult? res;

            if (cinemaId is null)
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

            return starring;
        }
        public async Task<Cinema?> UpdateRating(string id, double rating, CancellationToken ct)
        {
            Cinema? cinema = await FindById(id, ct);

            if (cinema is null)
            {
                return null;
            }
            if (rating == 0) 
            {
                return cinema;
            }

            cinema.Rating.Score *= cinema.Rating.N;

            if (rating < 0)
            {
                cinema.Rating.N--;
            }
            else {
                cinema.Rating.N++;
            }

            cinema.Rating.Score += rating;
            cinema.Rating.Score /= cinema.Rating.N;

            var update = Builders<Cinema>.Update.Set(e => e.Rating, cinema.Rating);
            var find = Builders<Cinema>.Filter.Where(e => e.Id == cinema.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct); ;

            return cinema;
        }
        public async Task<StudioRecord?> DeleteProductionStudio(string? cinemaId, StudioRecord studio, CancellationToken ct) 
        {
            FilterDefinition<Cinema> elementMatch = Builders<Cinema>.Filter.ElemMatch(c => c.ProductionStudios, s => s.Id == studio.Id);
            UpdateDefinition<Cinema> delete = Builders<Cinema>.Update.PullFilter(c=> c.ProductionStudios, s => s.Id == studio.Id);

            UpdateResult res;

            if (cinemaId is null)
            {
                res = await _collection.UpdateManyAsync(elementMatch, delete, cancellationToken:ct);
            }
            else
            {
                FilterDefinition<Cinema> find = Builders<Cinema>.Filter.Where(c => c.Id == cinemaId);

                res = await _collection.UpdateOneAsync(find & elementMatch, delete, cancellationToken:ct);
            }

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return studio;
        }
        public async Task<Starring?> DeleteStarring(string? cinemaId, Starring starring, CancellationToken ct)
        {
            FilterDefinition<Cinema> elementMatch = Builders<Cinema>.Filter.ElemMatch(c => c.Starrings, s => s.Id == starring.Id);
            UpdateDefinition<Cinema> delete = Builders<Cinema>.Update.PullFilter(c => c.Starrings, s => s.Id == starring.Id);

            UpdateResult res;

            if (cinemaId is null)
            {
                res = await _collection.UpdateManyAsync(elementMatch, delete, cancellationToken: ct);
            }
            else
            {
                FilterDefinition<Cinema> find = Builders<Cinema>.Filter.Where(c => c.Id == cinemaId);

                res = await _collection.UpdateOneAsync(find & elementMatch, delete, cancellationToken: ct);
            }

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return starring;
        }
    }
}
