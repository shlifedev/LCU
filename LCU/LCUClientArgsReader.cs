using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
/// <summary>
/// GET LCU Client Status
/// Thanks to sweetriverfish, this feature reference by https://github.com/sweetriverfish/LeagueAutoAccept/blob/main/Leauge%20Auto%20Accept/Program.cs
/// </summary>
public class LCUClientArgsReader
{
    private static Regex PORT_REGEX = new Regex("\"--app-port=(\\d+?)\"");
    private static Regex RIOT_CLIENT_APP_PORT = new Regex("\"--riotclient-app-port=(\\d+?)\"");
    private static Regex AUTH_TOKEN_REGEX = new Regex("\"--remoting-auth-token=(.+?)\"");
    private static Regex REGION = new Regex("\"--region=(.+?)\"");
    private static Regex LOCALE = new Regex("\"--locale=(.+?)\"");
    private static Regex REMOTE_AUTH_TOKEN = new Regex("\"--remoting-auth-token=(.+?)\"");
    private static Regex RIOT_CLIENT_PATH = new Regex("\"--output-base-dir=(.+?)\"");

    public static LCUProcessInfo GetLeagueStatus()
    {
        foreach (var p in Process.GetProcessesByName("LeagueClientUx"))
        {
            using (var mos = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + p.Id.ToString()))
            using (var moc = mos.Get())
            {
                // 프로세스의 실행정보 (command line)
                var commandLine = (string)moc.OfType<ManagementObject>().First()["CommandLine"];
                Console.WriteLine(commandLine);
                try
                {
                    return new LCUProcessInfo()
                    {
                        LCUProcess = p,
                        AuthToken = AUTH_TOKEN_REGEX.Match(commandLine).Groups[1].Value,
                        Port = PORT_REGEX.Match(commandLine).Groups[1].Value,
                        Region = REGION.Match(commandLine).Groups[1].Value,
                        Locale = LOCALE.Match(commandLine).Groups[1].Value,
                        RemoteAuthToken = REMOTE_AUTH_TOKEN.Match(commandLine).Groups[1].Value,
                        RiotAppPort = RIOT_CLIENT_APP_PORT.Match(commandLine).Groups[1].Value,
                        RiotClientPath = RIOT_CLIENT_PATH.Match(commandLine).Groups[1].Value,

                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while trying to get the status for LeagueClientUx: {e.ToString()}\n\n(CommandLine = {commandLine})");
                }
            }
        }

        return null;
    }
}