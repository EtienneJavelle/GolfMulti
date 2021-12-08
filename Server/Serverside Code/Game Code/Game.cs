using PlayerIO.GameLibrary;
using System;
using System.Linq;

namespace Server {
    public class Player : BasePlayer
    {
        public bool Done = false; // Player complete level
        public int Index = 0;
    }

    [RoomType("ChessRoom")]
    public class GameCode : Game<Player>
    {
        private int RoomSize = 4;
        private int GameSize = 0;
        private int Turn = 0;

        public override void GameStarted() => Console.WriteLine("Game is started: " + RoomId);

        public override void GameClosed() => Console.WriteLine("RoomId: " + RoomId);

        public override void UserJoined(Player player)
        {
            if (PlayerCount > RoomSize)
                player.Disconnect();
            else
            {
                player.Index = PlayerCount - 1;

                foreach (Player other in Players)
                {
                    if (other.ConnectUserId != player.ConnectUserId)
                    {
                        other.Send("PlayerJoined", player.ConnectUserId);
                        player.Send("PlayerJoined", other.ConnectUserId);
                    }
                }

                player.Send(nameof(MessageType.NextLevel));

                if (PlayerCount == 1)
                {
                    player.Send(nameof(MessageType.Ready));
                }
            }
        }

        public override void UserLeft(Player player) => Broadcast("PlayerLeft", player.ConnectUserId);

        public override void GotMessage(Player player, Message message)
        {
            switch(message.Type)
            {
                case nameof(MessageType.Shoot):
                    if (GameSize == 0)
                    {
                        GameSize = PlayerCount;
                        Console.WriteLine("Game locked with a size of " + GameSize);
                    }   

                    Turn = (Turn + 1) % GameSize;

                    foreach (Player other in Players)
                        if (other.Index == Turn)
                            other.Send(nameof(MessageType.Ready));

                    break;

                case nameof(MessageType.Update):
                    Console.WriteLine(player.Index);
                    message.Add(player.Index);
                    SendMessage(player, message);

                    break;

                case nameof(MessageType.Done):
                    player.Done = true;

                    if (AllDone())
                        foreach (Player other in Players)
                            other.Done = false;
                    
                    Broadcast(nameof(MessageType.NextLevel));

                    break;
            }
        }

        private void SendMessage(Player from, Message message)
        {
            foreach (Player other in Players)
                if (other.ConnectUserId != from.ConnectUserId)
                    other.Send(message);
        }

        private void SendMessage(Player from, string type, params object[] parameters)
        {
            foreach (Player other in Players)
                if (other.ConnectUserId != from.ConnectUserId)
                    other.Send(type, parameters);
        }

        private bool AllDone()
        {
            foreach (Player player in Players)
                if (!player.Done)
                    return false;

            return true;
        }
    }
}