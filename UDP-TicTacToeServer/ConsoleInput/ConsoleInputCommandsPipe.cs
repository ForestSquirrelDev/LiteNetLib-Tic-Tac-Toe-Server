using System;
using System.Collections.Generic;
using PoorMansECS.Systems;

namespace Server.ConsoleInput {
    public class ConsoleInputCommandsPipe : IUpdateable {
        private readonly HashSet<IConsoleInputCommandsReceiver> _receivers = new();

        public ConsoleInputCommandsPipe(IEnumerable<IConsoleInputCommandsReceiver> receivers) {
            foreach (var receiver in receivers)
                _receivers.Add(receiver);
        }

        public ConsoleInputCommandsPipe() { }

        public void Update(float delta) {
            if (Console.KeyAvailable) {
                var key = Console.ReadKey(true);
                foreach (var receiver in _receivers) {
                    receiver.ReceiveInputCommand(new ConsoleInputCommand(key));
                }
            }
        }

        public void AddReceiver(IConsoleInputCommandsReceiver receiver) {
            _receivers.Add(receiver);
        }
    }
}

namespace Server.ConsoleInput {
    public interface IConsoleInputCommandsReceiver {
        public void ReceiveInputCommand(ConsoleInputCommand command);
    }
}

namespace Server.ConsoleInput {
    public readonly struct ConsoleInputCommand {
        public ConsoleKeyInfo KeyInfo { get; }

        public ConsoleInputCommand(ConsoleKeyInfo keyInfo) {
            KeyInfo = keyInfo;
        }
    }
}
