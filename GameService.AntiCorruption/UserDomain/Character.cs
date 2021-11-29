﻿using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GameService.AntiCorruption.UserDomain
{
    public class Character
    {
        public Guid CharacterId { get; set; }
        public string CharacterName { get; set; }
    }
}