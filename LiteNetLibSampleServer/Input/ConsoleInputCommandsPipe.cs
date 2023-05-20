using System;
using System.Collections.Generic;
using PoorMansECS.Systems;

namespace LiteNetLibSampleServer.Input {
    public class ConsoleInputCommandsPipe : IUpdateable {
        private readonly HashSet<IInputCommandsReceiver> _receivers = new();

        public ConsoleInputCommandsPipe(IEnumerable<IInputCommandsReceiver> receivers) {
            foreach (var receiver in receivers)
                _receivers.Add(receiver);
        }

        public ConsoleInputCommandsPipe() { }

        public void Update(float delta) {
            if (Console.KeyAvailable) {
                var key = Console.ReadKey(true);
                foreach (var receiver in _receivers) {
                    receiver.ReceiveInputCommand(new InputCommand(key));
                }
            }
        }

        public void AddReceiver(IInputCommandsReceiver receiver) {
            _receivers.Add(receiver);
        }
    }

    public interface IInputCommandsReceiver {
        public void ReceiveInputCommand(InputCommand command);
    }

    public readonly struct InputCommand {
        public ConsoleKeyInfo KeyInfo { get; }
        
        public InputCommand(ConsoleKeyInfo keyInfo) {
            KeyInfo = keyInfo;
        }
    }
}
