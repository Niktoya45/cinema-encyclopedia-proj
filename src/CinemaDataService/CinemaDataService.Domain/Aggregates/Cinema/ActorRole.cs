﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CinemaDataService.Domain.Aggregates.Base;

namespace CinemaDataService.Domain.Aggregates.CinemaAggregate
{
    public record ActorRole:Value
    {
        [BsonRepresentation(BsonType.Int32)]
        public RolePriority Priority { get; set; }

    }
}
