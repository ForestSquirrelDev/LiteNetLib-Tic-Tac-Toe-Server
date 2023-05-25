using Server.Game.Systems;
using PoorMansECS;
using Server.Game.Systems.TurnInput;
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
            _world.CreateSystem<GameStarterSystem>().InjectDependencies(_outgoingPacketsPipe);
            _world.CreateSystem<RoomJoinHandlerSystem>().InjectDependencies(_incomingPacketsPipe, _outgoingPacketsPipe);
            _world.CreateSystem<InputHandlerSystem>().InjectDependencies(_incomingPacketsPipe);
            _world.CreateSystem<NextTurnHandlerSystem>().InjectDependencies(_outgoingPacketsPipe);
            _world.CreateSystem<TurnFinishHandlerSystem>().InjectDependencies(_outgoingPacketsPipe);
        }
    }
}
