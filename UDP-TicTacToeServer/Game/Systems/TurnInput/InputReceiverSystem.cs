using PoorMansECS.Systems;
using Server.Game.Components;
using Server.Game.Entities;
using Server.Game.Systems.Events;
using ServerShared.Shared.Network;

namespace Server.Game.Systems.TurnInput {
    public class InputReceiverSystem : SystemBase, INetMessageListener {
        private IncomingMessagesPipe _incomingMessagesPipe;

        public InputReceiverSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingMessagesPipe incomingMessagesPipe) {
            _incomingMessagesPipe = incomingMessagesPipe;
        }

        protected override void OnStart() {
            _incomingMessagesPipe.Register(MessageType.InputMessage, this);
        }

        public void ReceiveMessage(MessageWrapper requestMessage) {
            var inputMessage = (InputMessage)requestMessage.Message;
            var associatedPlayer = _context.World.Entities.GetFirst<Player>(
                player => player.GetComponent<AssociatedPeerComponent>().Peer.Id == requestMessage.AssociatedPeer.Id);
            var gameSide = associatedPlayer.GetComponent<GameSideComponent>();
            _context.EventBus.SendEvent(new PlayerInputEvent(gameSide.GameSide, inputMessage.Row, inputMessage.Column, requestMessage));
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }
    }
}