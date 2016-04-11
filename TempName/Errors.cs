﻿using System;

namespace TempName
{
    class Errors
    {
        public static string MasterServerNotConnetMessage { get; } = String.Format("=== Cannot reach master server ===");

        public static string HostNotConnetMessage { get; } = String.Format("======= Cannot reach host! =======");

        public static string PasswordServerMessage { get; } = String.Format("=========== Passworded! ==========");

        public static string NoPlayersFoundMessage { get; } = String.Format("======== No players found! =======");

        public static string PlayerHasNoNameMessage { get; } = String.Format("======= Player has no name! ======");

        public static string PlayerHasNoUIDMessage { get; } = String.Format("======= Player has no uid! =======");
    }
}
