using System.Diagnostics;

public class LCUProcessInfo
{
    /// <summary>
    /// 프로세스 
    /// </summary>
    public Process LCUProcess { get; set; }

    /// <summary>
    /// 웹소켓 포트
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// 인증토큰
    /// </summary>
    public string AuthToken { get; set; }

    /// <summary>
    /// 지역
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// 앱포트
    /// </summary>
    public string RiotAppPort { get; set; }
    /// <summary>
    /// 언어
    /// </summary>
    public string Locale { get; set; }
    /// <summary>
    /// RemoteAuthToken
    /// </summary>
    public string RemoteAuthToken { get; set; }
    /// <summary>
    /// (Install) RiotClientPath
    /// </summary>
    public string RiotClientPath { get; set; }
}
