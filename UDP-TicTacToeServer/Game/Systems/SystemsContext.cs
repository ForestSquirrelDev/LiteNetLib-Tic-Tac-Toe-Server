using PoorMansECS.Systems;
using Server.Shared.Network;

namespace Server.Game.Systems {
    public class SystemsContext {
        public PacketsPipe PacketsPipe { get; }
        public PoorMansECS.Entities.Entities Entities { get; }
        public SystemsEventBus EventBus { get; }

        public SystemsContext(PacketsPipe packetsPipe, PoorMansECS.Entities.Entities entities, SystemsEventBus eventBus) {
            PacketsPipe = packetsPipe;
            Entities = entities;
            EventBus = eventBus;
        }
    }
}