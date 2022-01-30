using System;
using System.Collections.Generic;
using GameService.Domain.Entities;

namespace GameService.Queries
{
    public class GameQuery
    {
        private Game _game;

        public GameQuery(Game game)
        {
            _game = game;
        }

        public bool IsIdExist(Guid id)
        {
            return true;
        }
        
        public Character FindCharacterOrDefault(Guid id)
        {
            return _game.FindCharacterOrDefault(id);
        }
        
        public List<Character> GetAllActiveCharacters()
        {
            return _game.GetAllActiveCharacters();
        }
    }
}