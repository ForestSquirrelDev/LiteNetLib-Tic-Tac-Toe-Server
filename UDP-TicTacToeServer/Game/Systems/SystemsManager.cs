using PoorMansECS.Systems;
using Server.Shared.Network;

namespace Server.Game.Systems {
    public class SystemsManager {
        private readonly PoorMansECS.Systems.Systems _systems;
        private readonly SystemsEventBus _eventBus;

        public SystemsManager(PacketsPipe packetsPipe, PoorMansECS.Entities.Entities entities) {
            _eventBus = new SystemsEventBus();
            var context = new SystemsContext(packetsPipe, entities, _eventBus);
            var systemsList = new List<ISystem> {
                new RoomCreationSystem(context), new RoomJoinHandlerSystem(context)
            };
            _systems = new PoorMansECS.Systems.Systems(systemsList);
        }

        public void Start() => _systems.Start();
        public void Update(float delta) => _systems.Update(delta);
    }
}