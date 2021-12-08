using PlayerIO.GameLibrary;
using System;

namespace Chess.Server {
    public class Player : BasePlayer {
    }

    [RoomType("ChessRoom")]
    public class GameCode : Game<Player> {

        public override void GameStarted() =>

            Console.WriteLine("Game is started: " + RoomId);

        public override void GameClosed() => Console.WriteLine("RoomId: " + RoomId);

        public override void UserJoined(Player player) {
            foreach(Player pl in Players) {
                if(pl.ConnectUserId != player.ConnectUserId) {
                    pl.Send("PlayerJoined", player.ConnectUserId);
                    player.Send("PlayerJoined", pl.ConnectUserId);
                }
            }
        }

        public override void UserLeft(Player player) => Broadcast("PlayerLeft", player.ConnectUserId);

        public override void GotMessage(Player player, Message m) {
            switch(m.Type) {
                case nameof(MessageType.Debug):
                    Broadcast("Debug");
                    Console.WriteLine($"Debug {m.GetString(0)}");
                    break;
            }
        }
    }
}