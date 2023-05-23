using Server.Game.Systems;
using PoorMansECS;
using ServerShared.Shared.Network;
using Server.Shared.Network;

namespace Game.Systems
{
    public class SystemsBuilder
    {
        private readonly World _world;
        private readonly IncomingPacketsPipe _incomingPacketsPipe;
        private readonly OutgoingPacketsPipe _outgoingPacketsPipe;

        public SystemsBuilder(World world, IncomingPacketsPipe incomingPacketsPipe, OutgoingPacketsPipe outgoingPacketsPipe)
        {
            _world = world;
            _incomingPacketsPipe = incomingPacketsPipe;
            _outgoingPacketsPipe = outgoingPacketsPipe;
        }

        public void Build()
        {
            _world.CreateSystem<RoomCreationSystem>();
            var roomJoinHandlerSystem = _world.CreateSystem<RoomJoinHandlerSystem>();

            roomJoinHandlerSystem.InjectDependencies(_incomingPacketsPipe, _outgoingPacketsPipe);
        }
    }
}
