using app.Jobs;
using app.Services.Extensions;
using slack.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddSlackService(c =>
{
  c.WebhookUrl = configuration["SlackConfiguration:WebhookUrl"];
});

services.AddCronJob<TestJob1>(opt =>
{
  opt.CronExpression = @"* * * * *";
  opt.TimeZoneInfo = TimeZoneInfo.Local;
});

// ====================================== //

var app = builder.Build();

app.Run();