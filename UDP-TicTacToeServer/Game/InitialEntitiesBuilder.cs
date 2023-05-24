using Game.Components;
using Game.Entities;
using PoorMansECS;
using Server.Game.Components;
using Server.Game.Entities;

namespace Server.Game {
    public class InitialEntitiesBuilder {
        private readonly World _world;

        public InitialEntitiesBuilder(World world) {
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
        }

        private void BuildGrid(World world) {
            var grid = world.CreateEntity<Grid>();
            grid.SetComponent(new GridCellsComponent(3, 3));
            grid.SetComponent(new GridParametersComponent(3, 3));
        }
    }
}
