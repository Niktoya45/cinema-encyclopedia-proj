using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EncyclopediaService.Api.Models
{
    /*
     * vvv src/Shared/.. vvv
     */

    /**/ public enum Entities
    { 
        Cinemas, 
        Persons,
        Studios
    }
    /**/ public enum Genre
    {
        None = 0b_0000_0000_0000_0000,
        Action = 0b_0000_0000_0000_0001,
        Comedy = 0b_0000_0000_0000_0010,
        Documentary = 0b_0000_0000_0000_0100,
        Drama = 0b_0000_0000_0000_1000,
        Fantasy = 0b_0000_0000_0001_0000,
        Horror = 0b_0000_0000_0010_0000,
        Musical = 0b_0000_0000_0100_0000,
        Mystery = 0b_0000_0000_1000_0000,
        Romance = 0b_0000_0001_0000_0000,
        SciFi = 0b_0000_0010_0000_0000,
        Thriller = 0b_0000_0100_0000_0000,
        Western = 0b_0000_1000_0000_0000
    }
    /**/
    public enum Language
    {
        [Display(Name = ". . .")]
        None,
        English,
        German,
        French,
        Spanish,
        Italian
    }

    /**/ public enum RolePriority { Main, Support, Episodic }
    /**/ public record Starring { 

        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } 

        public Job Jobs; 
        public string? RoleName { get; set; }
        public RolePriority? RolePriority { get; set; }
    };

    public class Cinema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public Language? Language { get; set; }
        public double RatingScore { get; set; } = 0.0;
        public StudioRecord[]? ProductionStudios { get; set; }
        public Starring[]? Starrings { get; set; }
        public string? Description { get; set; }
    }
}
