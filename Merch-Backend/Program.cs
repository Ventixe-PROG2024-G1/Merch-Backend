using Application.Data.Context;
using Application.Data.Repository;
using Application.Services;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MerchDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddScoped<IMerchRepository, MerchRepository>();
builder.Services.AddScoped<IMerchService, MerchService>();

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Api"));
app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyHeader());

app.MapOpenApi();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
