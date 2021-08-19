namespace DotNet.Testcontainers.Containers
{
  public class ExecResult
  {
    public ExecResult(string stdOut, string stdErr, long exitCode)
    {
      this.StdOut = stdOut;
      this.StdErr = stdErr;
      this.ExitCode = exitCode;
    }

    public string StdOut { get; }

    public string StdErr { get; }

    public long ExitCode { get; }
  }
}
