using System.Collections.Generic;
using PoorMansECS.Components;
using Server.Game.Entities;

namespace Server.Game.Components {
    public class JoinedPlayersComponent : IComponentData {
        public Dictionary<int, Player> JoinedPlayers { get; } = new();
    }
}