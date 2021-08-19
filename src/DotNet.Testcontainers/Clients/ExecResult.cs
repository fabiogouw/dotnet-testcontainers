namespace DotNet.Testcontainers.Clients
{
  public class ExecResult
  {
    public string StdOut { get; internal set; }

    public string StdErr { get; internal set; }

    public long ExitCode { get; internal set; } = long.MinValue;
  }
}
