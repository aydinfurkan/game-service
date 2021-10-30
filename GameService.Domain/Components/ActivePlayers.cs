using System;
using System.Collections.Generic;
using System.Linq;
using GameService.Domain.Entity;

namespace GameService.Domain.Components
{
    public class ActivePlayers
    {
        private readonly Dictionary<Guid, Player> _playerList;

        public ActivePlayers()
        {
            _playerList = new Dictionary<Guid, Player>();
        }

        public bool Add(Player player)
        {
            return _playerList.TryAdd(player.Id, player);
        }

        public bool Delete(Player player)
        {
            return _playerList.Remove(player.Id);
        }

        public Player FindOrDefault(Guid id)
        {
            var ok = _playerList.TryGetValue(id, out var player);
            return player;
        }
        
        public List<Player> GetAllActivePlayers()
        {
            return _playerList.Select(x => x.Value).ToList();
        }
    }
}