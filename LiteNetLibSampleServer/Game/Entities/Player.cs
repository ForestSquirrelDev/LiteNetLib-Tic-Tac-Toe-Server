using System.Collections.Generic;
using PoorMansECS.Components;
using PoorMansECS.Entities;

namespace Server.Game.Entities {
    public class Player : Entity {
        public Player(IEnumerable<IComponentData> components) : base(components) { }
    }
}