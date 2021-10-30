using System;
using System.Collections.Generic;
using GameService.Domain.Components;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Entity
{
    public class Game
    {
        private Guid _id;
        private readonly ActivePlayers _activePlayers;

        public Game()
        {
            _id = Guid.NewGuid();
            _activePlayers = new ActivePlayers();
        }

        public bool AddPlayer(Player player)
        {
            return _activePlayers.Add(player);
        }
        
        public bool DeletePlayer(Player player)
        {
            return _activePlayers.Delete(player);
        }
        
        public Player FindPlayerOrDefault(Guid id)
        {
            return _activePlayers.FindOrDefault(id);
        }
        
        public List<Player> GetAllActivePlayers()
        {
            return _activePlayers.GetAllActivePlayers();
        }

        public bool ChangePlayerPosition(Guid id, Position position)
        {
            return _activePlayers.ChangePlayerPosition(id, position);
        }
    }
}