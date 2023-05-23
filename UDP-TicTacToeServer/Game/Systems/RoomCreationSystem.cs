using PoorMansECS.Components;
using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;

namespace Server.Game.Systems {
    public class RoomCreationSystem : SystemBase {
        public RoomCreationSystem(SystemsContext context) : base(context) { }

        protected override void OnStart() {
            var room = new Room();
            room.SetComponent(new JoinedPlayersComponent());
            room.SetComponent(new NextTurnComponent());
            _context.Entities.Add(room);
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }
    }
}