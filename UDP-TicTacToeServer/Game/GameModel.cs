using Server.Connection;
using PoorMansECS.Systems;
using Server.Game.Systems;
using Server.Input;
using Server.Shared.Network;

namespace Server.Game {
    public class GameModel : IUpdateable {
        private readonly PoorMansECS.Entities.Entities _entities;
        private readonly SystemsManager _systemsManager;

        public GameModel(PacketsPipe packetsPipe, ConsoleInputCommandsPipe inputCommandsPipe) {
            _entities = new PoorMansECS.Entities.Entities();
            _systemsManager = new SystemsManager(packetsPipe, _entities);
        }

        public void Start() {
            _systemsManager.Start();
        }

        public void Update(float delta) {
            _systemsManager.Update(delta);
        }
    }
}
