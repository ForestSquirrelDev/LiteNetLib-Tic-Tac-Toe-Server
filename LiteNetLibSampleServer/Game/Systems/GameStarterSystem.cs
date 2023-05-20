using PoorMansECS.Systems;

namespace Server.Game.Systems {
    public class GameStarterSystem : SystemBase, ISystemEventListener {
        public GameStarterSystem(SystemsContext context) : base(context) {
            context.EventBus.Subscribe<RoomFilledEvent>(this);
        }

        protected override void OnStart() { }

        protected override void OnUpdate(float delta) { }

        public void ReceiveEvent<T>(T systemEvent) where T : ISystemEvent {
            if (systemEvent is RoomFilledEvent) {
                StartGame();
                BroadcastStartToPeers();
            }
        }

        private void StartGame() {
            var grid = new Grid();
            grid.AddComponent(new GridCellsComponent(3, 3));
            _context.Entities.Add(grid);
        }

        private void BroadcastStartToPeers() {

        }
    }
}