using PoorMansECS.Systems;
using Server.Input;
using PoorMansECS;
using Game.Systems;
using ServerShared.Shared.Network;
using Server.Shared.Network;

namespace Server.Game
{
    public class GameModel : IUpdateable {
        public World World { get; }

        public GameModel(IncomingPacketsPipe incomingPacketsPipe, OutgoingPacketsPipe outgoingPacketsPipe, ConsoleInputCommandsPipe inputCommandsPipe) {
            World = new World();

            var systemsBuilder = new SystemsBuilder(World, incomingPacketsPipe, outgoingPacketsPipe);
            systemsBuilder.Build();

            var entitiesBuilder = new InitialEntitiesBuilder(World);
            entitiesBuilder.Build();
        }

        public void Start() {
            World.Start();
        }

        public void Update(float delta) {
            World.Update(delta);
        }
    }
}
