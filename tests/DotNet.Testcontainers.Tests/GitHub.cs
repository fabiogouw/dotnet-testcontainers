namespace DotNet.Testcontainers.Tests
{
  using System;
  using System.IO;
  using System.Net;
  using System.Net.Sockets;
  using System.Text;
  using System.Threading.Tasks;
  using DotNet.Testcontainers.Builders;
  using DotNet.Testcontainers.Clients;
  using DotNet.Testcontainers.Configurations;
  using DotNet.Testcontainers.Containers;
  using DotNet.Testcontainers.Tests.Fixtures;
  using Xunit;

  public sealed class GitHub : IClassFixture<Ryuk>, IClassFixture<Alpine>, IAsyncLifetime
  {
    private readonly TcpClient client;

    private readonly StreamWriter tcpWriter;

    private readonly StreamReader tcpReader;

    public GitHub(Ryuk ryuk, Alpine alpine)
    {
      var endpoint = new IPEndPoint(IPAddress.Loopback, ryuk.Container.GetMappedPublicPort(8080));
      this.client = new TcpClient();
      this.client.Connect(endpoint);
      var stream = this.client.GetStream();
      this.tcpWriter = new StreamWriter(stream, Encoding.ASCII);
      this.tcpReader = new StreamReader(stream, Encoding.ASCII);
    }

    [Fact]
    public Task Issue242()
    {
      return Task.Delay(TimeSpan.FromMinutes(5));
    }

    public Task InitializeAsync()
    {
      var filter = $"label={TestcontainersClient.TestcontainersCleanUpLabel}={bool.TrueString}";
      this.tcpWriter.WriteLine(filter);
      this.tcpWriter.Flush();
      return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
      this.tcpReader.Dispose();
      this.tcpWriter.Dispose();
      this.client.Dispose();
      return Task.CompletedTask;
    }
  }

  public sealed class Ryuk : IAsyncLifetime
  {
    private const string Name = "ryuk";

    private const string Image = "testcontainers/ryuk:latest";

    public ITestcontainersContainer Container { get; }
      = new TestcontainersBuilder<TestcontainersContainer>()
        .WithName(Name)
        .WithImage(Image)
        .WithMount("/var/run/docker.sock", "/var/run/docker.sock", AccessMode.ReadOnly)
        .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
        .WithExposedPort(8080)
        .WithPortBinding(8080, true)
        .WithCleanUp(false)
        .Build();

    public Task InitializeAsync()
    {
      return this.Container.StartAsync();
    }

    public Task DisposeAsync()
    {
      return Task.CompletedTask;
      return this.Container.DisposeAsync().AsTask();
    }
  }

  public sealed class Alpine : IAsyncLifetime
  {
    private const string Name = "alpine";

    private const string Image = "alpine:latest";

    public ITestcontainersContainer Container { get; }
      = new TestcontainersBuilder<TestcontainersContainer>()
        .WithName(Name)
        .WithImage(Image)
        .WithCommand(KeepTestcontainersUpAndRunning.Command)
        .WithCleanUp(true)
        .Build();

    public Task InitializeAsync()
    {
      return this.Container.StartAsync();
    }

    public Task DisposeAsync()
    {
      return Task.CompletedTask;
      return this.Container.DisposeAsync().AsTask();
    }
  }
}
