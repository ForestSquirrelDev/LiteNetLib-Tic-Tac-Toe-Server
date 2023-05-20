using System;
using System.Collections.Generic;
using System.Threading;
using LiteNetLibSampleServer.Input;
using PoorMansECS.Systems;

namespace LiteNetLibSampleServer.UpdateLoop {
    public class ServerLoop : IInputCommandsReceiver {
        private readonly List<IUpdateable> _updateables;
        private bool _isRunning;

        public ServerLoop(IEnumerable<IUpdateable> updateables) {
            _updateables = new List<IUpdateable>();
            _updateables.AddRange(updateables);
        }

        public void Run() {
            _isRunning = true;
            const int delta = 15;
            while (_isRunning) {
                _updateables.ForEach(u => u.Update(delta));
                Thread.Sleep(delta);
            }
        }

        public void ReceiveInputCommand(InputCommand command) {
            if (command.KeyInfo.Key == ConsoleKey.E) {
                _isRunning = false;
            }
        }
    }
}
