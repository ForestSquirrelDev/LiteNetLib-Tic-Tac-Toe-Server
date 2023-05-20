using System.Collections.Generic;
using PoorMansECS.Components;
using PoorMansECS.Entities;

namespace Server.Game.Entities {
    public class Room : Entity {
        public Room(IEnumerable<IComponentData> components) : base(components) { }
    }
}