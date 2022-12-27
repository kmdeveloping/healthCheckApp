using app.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder
  .BuildServices()
  .Run();

