using Game.Components;
using PoorMansECS;
using Server.Game.Components;

namespace Server.Game.Entities {
    public class EntitiesBuilder {
        private readonly World _world;

        public EntitiesBuilder(World world) {
            _world = world;
        }

        public void Build() {
            BuildRoom(_world);
            BuildGrid(_world);
        }

        private void BuildRoom(World world) {
            var room = world.CreateEntity<Room>();
            room.SetComponent(new JoinedPlayersComponent());
            room.SetComponent(new NextTurnComponent());
            room.SetComponent(new GameStateComponent());
        }

        private void BuildGrid(World world) {
            var grid = world.CreateEntity<Grid>();
            grid.SetComponent(new GridCellsComponent(3, 3));
            grid.SetComponent(new GridParametersComponent(3, 3));
        }
    }
}
