using VisualVid.Encoder;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.Configure<EncoderSettings>(context.Configuration.GetSection("Encoder"));
    services.AddHostedService<VideoEncoderWorker>();
});

var host = builder.Build();
host.Run();
