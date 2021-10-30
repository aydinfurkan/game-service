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
        
        public bool IsPlayerActive(Guid id)
        {
            return _game.FindPlayerOrDefault(id) != null;
        }
        
        public List<Player> GetAllActivePlayers()
        {
            return _game.GetAllActivePlayers();
        }
    }
}