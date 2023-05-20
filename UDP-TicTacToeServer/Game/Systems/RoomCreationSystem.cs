using System.Collections.Generic;
using PoorMansECS.Components;
using Server.Game.Components;
using Server.Game.Entities;

namespace Server.Game.Systems {
    public class RoomCreationSystem : SystemBase {
        public RoomCreationSystem(SystemsContext context) : base(context) { }

        protected override void OnStart() {
            var room = new Room(new List<IComponentData> {new JoinedPlayersComponent()});
            _context.Entities.Add(room);
        }

        protected override void OnUpdate(float delta) { }
    }
}