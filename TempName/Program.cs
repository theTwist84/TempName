using System;
using System.Text;

using Newtonsoft.Json;

namespace TempName
{
    class Program
    {
        static void Main()
        {
            var text = new StringBuilder();

            while (true)
            {
                CheckServers(text);
                Helpers.Sleep();
            }
        }

        private static void CheckServers(StringBuilder text)
        {
            Helpers.Update();

            try {
                string ServerList_ = Settings.wc.DownloadString(Settings.MasterServer);
                dynamic ServerList = JsonConvert.DeserializeObject(ServerList_);

                foreach (string Host in ServerList.result.servers) {
                    try {
                        string HostJSON_ = Settings.wc.DownloadString(String.Format("http://{0}", Host));
                        dynamic HostJSON = JsonConvert.DeserializeObject(HostJSON_);
                        string ServerName = HostJSON.name;
                        int numPlayers = HostJSON.numPlayers;
                        int maxPlayers = HostJSON.maxPlayers;
                        dynamic PlayerList = HostJSON.players;

                        if (HostJSON_.Contains("passworded"))
                            Settings.IsPassworded = true;

                        if (Settings.IsUsingName_Server.Equals(true)) {
                            if (ServerName.Contains(Settings.ServerName)) {
                                if (numPlayers.Equals(0))
                                    Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})\n{4}", Host, ServerName, numPlayers, maxPlayers, Errors.NoPlayersFoundMessage));
                                else if (Settings.IsPassworded.Equals(true)) {
                                    Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})\n{4}", Host, ServerName, numPlayers, maxPlayers, Errors.PasswordServerMessage));

                                    Settings.IsPassworded = false;
                                }
                                else if(numPlayers.Equals(maxPlayers)) {
                                    Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})\n{4}", Host, ServerName, numPlayers, maxPlayers, Errors.FullServerMessage));
                                    
                                    for (int i = 0; i < numPlayers; i++) {
                                        dynamic Player = PlayerList[i];

                                        if (Player.name == null && Player.uid == null)
                                            Helpers.Output(text, String.Format("{0} or {1}", Errors.PlayerHasNoNameMessage, Errors.PlayerHasNoUIDMessage));
                                        else
                                            Helpers.Output(text, String.Format("UID: {1} Name: {0}", Player.name, Player.uid));
                                    }
                                }
                                else {
                                    Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})", Host, ServerName, numPlayers, maxPlayers));
                                    
                                    for (int i = 0; i < numPlayers; i++) {
                                        dynamic Player = PlayerList[i];

                                        if (Player.name == null && Player.uid == null) 
                                            Helpers.Output(text, String.Format("{0} or {1}", Errors.PlayerHasNoNameMessage, Errors.PlayerHasNoUIDMessage));
                                        else 
                                            Helpers.Output(text, String.Format("UID: {1} Name: {0}", Player.name, Player.uid));
                                    }
                                }
                                Helpers.Output(text, Environment.NewLine);
                            }
                        }
                        else
                        {
                            if (numPlayers.Equals(0))
                                Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})\n{4}", Host, ServerName, numPlayers, maxPlayers, Errors.NoPlayersFoundMessage));
                            else if (Settings.IsPassworded.Equals(true)) {
                                Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})\n{4}", Host, ServerName, numPlayers, maxPlayers, Errors.PasswordServerMessage));

                                Settings.IsPassworded = false;
                            }
                            else if (numPlayers.Equals(maxPlayers)) {
                                Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})\n{4}", Host, ServerName, numPlayers, maxPlayers, Errors.FullServerMessage));

                                for (int i = 0; i < numPlayers; i++) {
                                    dynamic Player = PlayerList[i];

                                    if (Player.name == null && Player.uid == null)
                                        Helpers.Output(text, String.Format("{0} or {1}", Errors.PlayerHasNoNameMessage, Errors.PlayerHasNoUIDMessage));
                                    else
                                        Helpers.Output(text, String.Format("UID: {1} Name: {0}", Player.name, Player.uid));
                                }
                            }
                            else {
                                Helpers.Output(text, String.Format("IP: {0} Host: {1} Players ({2}/{3})", Host, ServerName, numPlayers, maxPlayers));

                                for (int i = 0; i < numPlayers; i++) {
                                    dynamic Player = PlayerList[i];

                                    if (Player.name == null && Player.uid == null)
                                        Helpers.Output(text, String.Format("{0} or {1}", Errors.PlayerHasNoNameMessage, Errors.PlayerHasNoUIDMessage));
                                    else
                                        Helpers.Output(text, String.Format("UID: {1} Name: {0}", Player.name, Player.uid));
                                }
                            }
                            Helpers.Output(text, Environment.NewLine);
                        }
                    }
                    catch (Exception ex) {
                        switch (Settings.IsDEBUG.Equals(true)) {
                            case true:
                                Console.WriteLine(ex);
                                Console.Read();
                                break;

                            case false:
                                Helpers.Output(text, Errors.HostNotConnetMessage + Environment.NewLine);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) {
                switch (Settings.IsDEBUG.Equals(true)) {
                    case true:
                        Console.WriteLine(ex);
                        Console.Read();
                        break;

                    case false:
                        Helpers.Output(text, Errors.MasterServerNotConnetMessage);
                        break;
                }
            }
            finally {
                Logger.Log(text.ToString());
                text.Clear();
            }
        }
    }
}
