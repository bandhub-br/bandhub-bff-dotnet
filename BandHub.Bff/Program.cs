using BandHub.Bff.Common;
using BandHub.Bff.Features.Accounts.Login;
using BandHub.Bff.Features.Accounts.RegisterBand;
using BandHub.Bff.Features.Accounts.RegisterUser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBff(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapRegisterBandEndpoint();
app.MapRegisterUserEndpoint();
app.MapRegisterLoginEndpoint();

app.Run();