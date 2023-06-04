using PoorMansECS.Systems;
using PoorMansECS;
using Game.Systems;
using ServerShared.Shared.Network;
using Server.Shared.Network;
using Server.ConsoleInput;
using Server.Game.Entities;

namespace Server.Game {
    public class GameModel : IUpdateable {
        public World World { get; }

        public GameModel(IncomingMessagesPipe incomingMessagesPipe, OutgoingMessagesPipe outgoingMessagesPipe, ConsoleInputCommandsPipe inputCommandsPipe) {
            World = new World();

            var systemsBuilder = new SystemsBuilder(World, incomingMessagesPipe, outgoingMessagesPipe);
            systemsBuilder.Build();

            var entitiesBuilder = new EntitiesBuilder(World);
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
