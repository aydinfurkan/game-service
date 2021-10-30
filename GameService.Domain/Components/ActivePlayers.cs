using System;
using System.Collections.Generic;
using System.Linq;
using GameService.Domain.Entity;
using GameService.Domain.ValueObject;

namespace GameService.Domain.Components
{
    public class ActivePlayers
    {
        private static object _lockObj = new ();
        private readonly Dictionary<Guid, Player> _playerList;

        public ActivePlayers()
        {
            _playerList = new Dictionary<Guid, Player>();
        }

        public bool Add(Player player)
        {
            lock (_lockObj)
            {
                return _playerList.TryAdd(player.Id, player);
            }
        }

        public bool Delete(Player player)
        {
            lock (_lockObj)
            {
                return _playerList.Remove(player.Id);
            }
        }

        public Player FindOrDefault(Guid id)
        {
            lock (_lockObj)
            {
                var ok = _playerList.TryGetValue(id, out var player);
                return player;
            }
        }
        
        public List<Player> GetAllActivePlayers()
        {
            lock (_lockObj)
            {
                return _playerList.Select(x => x.Value).ToList();
            }
        }

        public bool ChangePlayerPosition(Guid id, Position position)
        {
            lock (_lockObj)
            {
                var ok = _playerList.TryGetValue(id, out var player);
                if (player == null) return false;
                player.Position = position;
                return true;
            }
        }
    }
}