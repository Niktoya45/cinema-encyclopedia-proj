using EncyclopediaService.Api.Models.Display;
using Shared.CinemaDataService.Models.Flags;

namespace EncyclopediaService.Api.Models.Test
{
    public static class TestEntities
    {
        public const bool Used = true;

        public static Cinema Cinema = new Cinema
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Cinema Title With Additional Length for Test Purposes",
                Picture = null,
                PictureUri = null,
                ReleaseDate = new DateOnly(2004, 12, 4),
                Genres = Genre.Western | Genre.Mystery | Genre.Thriller,
                Language = 0,
                RatingScore = 5.5,
                ProductionStudios = new ProductionStudio[] { new ProductionStudio { Id = "1" }, new ProductionStudio { Id = "2" } },
                Starrings = new Starring[] {
                    new Starring { Id = "1", Name="Name Surname 1", Jobs=Job.Actor, RoleName="Role Name", RolePriority = RolePriority.Main},
                    new Starring { Id = "2", Name="Name Surname 2", Jobs=Job.Actor, RoleName="Role Name", RolePriority = RolePriority.Main},
                    new Starring { Id = "3", Name="Name Surname 3", Jobs=Job.Actor, RoleName="Role Name", RolePriority = RolePriority.Main},
                    new Starring { Id = "4", Name="Name Surname 4", Jobs=Job.Actor, RoleName="Role Name", RolePriority = RolePriority.Main},
                    new Starring { Id = "5", Name="Name Surname 5", Jobs=Job.Director, RoleName=null},
                    new Starring { Id = "6", Name="Name Surname 6", Jobs=Job.Director, RoleName=null},
                    new Starring { Id = "7", Name="Name Surname 7", Jobs=Job.Director, RoleName=null},
                    new Starring { Id = "8", Name="Name Surname 8", Jobs=Job.Director, RoleName=null},
                },
                Description = "Cinema description goes here. This cinema is about.."
            };

        public static Person Person = new Person
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Long Long Name Long Long Long Surname",
            BirthDate = new DateOnly(1978, 1, 12),
            Country = 0,
            Jobs = Job.Actor,
            Picture = null,
            Filmography = new FilmographyRecord[] {
                    new FilmographyRecord { Id = "1", Name = "Cinema Title Long Long", Year=2000, Picture=null},
                    new FilmographyRecord { Id = "2", Name = "Cinema Title Long", Year=2000, Picture=null},
                    new FilmographyRecord { Id = "3", Name = "Cinema Title", Year=2000, Picture=null},
                    new FilmographyRecord { Id = "4", Name = "Cinema Title", Year=2000, Picture=null}
                },
            Description = "Person description goes here. A prominent actor.."
        };


        public static Studio Studio = new Studio
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Long Long Long Studio Name",
            FoundDate = new DateOnly(1938, 10, 7),
            Country = 0,
            PresidentName = "Name Name Surname",
            Picture = null,
            Filmography = new FilmographyRecord[] {
                    new FilmographyRecord { Id = "1", Name = "Cinema Title Long Long", Year=2000, Picture=null},
                    new FilmographyRecord { Id = "2", Name = "Cinema Title Long", Year=2000, Picture=null},
                    new FilmographyRecord { Id = "3", Name = "Cinema Title", Year=2000, Picture=null},
                    new FilmographyRecord { Id = "4", Name = "Cinema Title", Year=2000, Picture=null}
                },
            Description = "Studio description goes here. A studio with a long history.."
        };
    }
}
