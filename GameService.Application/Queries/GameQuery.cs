using System;
using System.Collections.Generic;
using GameService.Domain.Entity;

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
        
        public bool IsCharacterActive(Guid id)
        {
            return _game.FindCharacterOrDefault(id) != null;
        }
        
        public List<Character> GetAllActiveCharacters()
        {
            return _game.GetAllActiveCharacters();
        }
    }
}