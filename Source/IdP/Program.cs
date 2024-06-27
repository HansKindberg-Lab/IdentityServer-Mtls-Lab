using IdP;
using Serilog;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

	//builder.WebHost.ConfigureKestrel(kestrelServerOptions =>
	//{
	//	kestrelServerOptions.ConfigureHttpsDefaults(httpsConnectionAdapterOptions =>
	//	{
	//		httpsConnectionAdapterOptions.ClientCertificateValidation = (certificate, chain, errors) =>
	//		{
	//			return errors == SslPolicyErrors.None;
	//		};

	//		httpsConnectionAdapterOptions.OnAuthenticate = (_, sslServerAuthenticationOptions) =>
	//		{
	//			if (!sslServerAuthenticationOptions.ClientCertificateRequired)
	//				return;

	//			sslServerAuthenticationOptions.CertificateChainPolicy = new X509ChainPolicy
	//			{
	//				RevocationMode = X509RevocationMode.NoCheck,
	//				TrustMode = X509ChainTrustMode.System
	//			};
	//		};
	//	});
	//});

	builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}