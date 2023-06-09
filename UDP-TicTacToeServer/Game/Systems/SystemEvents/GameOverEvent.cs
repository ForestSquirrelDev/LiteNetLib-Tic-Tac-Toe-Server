﻿using PoorMansECS.Systems;
using ServerShared.Shared.Network;

namespace Server.Game.Systems.Events {
    public readonly struct GameOverEvent : ISystemEvent {
        public GameSide Winner { get; }

        public GameOverEvent(GameSide winner) {
            Winner = winner;
        }
    }
}
