using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Connect : Etienne.Singleton<Connect> {
    private const string PlayerIOMessage = "<color=yellow><b>PlayerIO :</b></color> ";

    [SerializeField] private ConnectionUIHandeler uIHandeler;
    [SerializeField] private string gameId, userName;

    private static Connection s_connection;
    private List<Message> msgList = new List<Message>();
    string userid;

    private Ball ball;
    List<Transform> balls = new List<Transform>();

    private void Start()
    {
        Application.runInBackground = true;

        DontDestroyOnLoad(gameObject);

        userid = $"{userName}_{UnityEngine.Random.Range(0, 10001)}";
        Debug.Log($"{PlayerIOMessage}Connecting with: {userid}");
        uIHandeler.text.text = "Connecting...";
        Dictionary<string, string> authenticationArguments = new Dictionary<string, string> { { "userId", userid } };
        PlayerIO.Authenticate(gameId, "public", authenticationArguments, null, AuthenticateSucessCallback, ErrorCallback);
    }

    private void AuthenticateSucessCallback(Client client)
    {
        Debug.Log($"{PlayerIOMessage}<color=green>Successfully authenticated</color>");
        OnConnection();

        Debug.Log($"{PlayerIOMessage}Create ServerEndpoint");
        client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

        Debug.Log($"{PlayerIOMessage}CreateJoinRoom");
        client.Multiplayer.CreateJoinRoom($"Room", "ChessRoom", true, null, null, CreateJoinRoomCallback, ErrorCallback);
    }

    private void OnConnection() {

    }

    private void CreateJoinRoomCallback(Connection connection)
    {
        s_connection = connection;
        s_connection.OnMessage += handlemessage;
        Debug.Log($"{PlayerIOMessage}<color=green>Joined Room.</color>");
        uIHandeler.gameObject.SetActive(false);
    }

    private void handlemessage(object sender, Message m)
    {
        Debug.Log($"{PlayerIOMessage} Message Recieved : {m}");
        msgList.Add(m);
    }

    private void ErrorCallback(PlayerIOError error)
    {
        Debug.Log($"{PlayerIOMessage}<color=red>Error connecting:</color> {Environment.NewLine}{error}");
        uIHandeler.text.text = "Can't connect !";
    }

    public static void Send(string type, params object[] parameters)
    {
        StringBuilder message = new StringBuilder();
        message.Append($"{PlayerIOMessage} Message Sent :");
        foreach(object parameter in parameters) {
            message.Append($"{parameter} ");
        }
        Debug.Log(message.ToString());
        s_connection.Send(type, parameters);
    }

    private void Update()
    {
        foreach(Message message in msgList)
        {
            switch(message.Type)
            {
                case nameof(MessageType.Ready):
                    // TODO : Player peut switcher de camera et tirer
                    // TODO : Quand il tire envoyer MessageType.Shoot au serveur

                    break;

                case nameof(MessageType.Update):
                    // TODO : Update la balle balls[i] avec i = message.GetInt(0), si elle n'existe pas if faut la créer

                    break;
            }
        }

        msgList.Clear();
    }
}
