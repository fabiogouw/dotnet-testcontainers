namespace DotNet.Testcontainers.Tests
{
  using System;
  using System.Net;
  using System.Net.Sockets;
  using System.Text;
  using Xunit;

  public sealed class RyukTest
  {
    private readonly IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, 8080);

    [Fact]
    public void Foo()
    {
      const string filter = "label=test";
      using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
      {
        client.Connect(this.endpoint);
        client.Send(new ArraySegment<byte>(Encoding.UTF8.GetBytes(filter)));
      }
    }
  }
}
