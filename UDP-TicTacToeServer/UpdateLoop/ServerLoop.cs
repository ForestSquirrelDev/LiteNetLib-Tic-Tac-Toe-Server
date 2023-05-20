using System;
using System.Collections.Generic;
using System.Threading;
using PoorMansECS.Systems;
using Server.Input;

namespace Server.UpdateLoop {
    public class ServerLoop : IInputCommandsReceiver {
        private readonly List<IUpdateable> _updateables;
        private bool _isRunning;

        public ServerLoop(IEnumerable<IUpdateable> updateables) {
            _updateables = new List<IUpdateable>();
            _updateables.AddRange(updateables);
        }

        public void RunMainLoop() {
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

        public void AddUpdateable(IUpdateable updateable) {
            _updateables.Add(updateable);
        }
    }
}
