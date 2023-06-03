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
        private readonly IncomingMessagesPipe _incomingMessagesPipe;
        private readonly OutgoingMessagesPipe _outgoingMessagesPipe;

        public SystemsBuilder(World world, IncomingMessagesPipe incomingMessagesPipe, OutgoingMessagesPipe outgoingMessagesPipe)
        {
            _world = world;
            _incomingMessagesPipe = incomingMessagesPipe;
            _outgoingMessagesPipe = outgoingMessagesPipe;
        }

        public void Build()
        {
            _world.CreateSystem<GameStarterSystem>().InjectDependencies(_outgoingMessagesPipe);
            _world.CreateSystem<RoomJoinHandlerSystem>().InjectDependencies(_incomingMessagesPipe, _outgoingMessagesPipe);
            _world.CreateSystem<InputHandlerSystem>().InjectDependencies(_incomingMessagesPipe);
            _world.CreateSystem<NextTurnHandlerSystem>().InjectDependencies(_outgoingMessagesPipe);
            _world.CreateSystem<GameOverHandlerSystem>().InjectDependencies(_outgoingMessagesPipe);
        }
    }
}
