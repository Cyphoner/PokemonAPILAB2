using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PokemonAPILAB2.Models;
using PokemonAPILAB2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<HttpClient>();

builder.Services.AddSingleton(_ => new PokemonAPILAB2.Data.MongoDBContext("PokemonLab"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



/*app.MapPost("Pokemon", async (HttpClient client, Pokemon pokemon) =>
{
    StringContent request = new StringContent(JsonConvert.SerializeObject(pokemon));


   var result = await client.PostAsync("http://localhost:3000/pokemon", request);

   if (result.StatusCode == HttpStatusCode.OK)
   {
       return Results.Ok();
   }
   else
   {
       return Results.Problem();
   }
});*/


app.MapPost("/pokemon", async (Pokemon pokemon, MongoDBContext db) =>
{
    var newPokemon = await db.AddPokemon("Pokemon", pokemon);
    return Results.Ok(newPokemon);
});


app.MapGet("/pokemon", async (MongoDBContext db) =>
{
    var pokemon = await db.GetPokemons("Pokemon");
    return Results.Ok(pokemon);
});


app.MapGet("/pokemon/{id}", async (string id, MongoDBContext db) => {
    var pokemon = await db.GetPokemon("Pokemon", id);
    return Results.Ok(pokemon);
});


app.MapPut("/pokemon/{id}", async (string id, Pokemon pokemon, MongoDBContext db) =>
{
    var updatePokemon = await db.UpdatePokemon("Pokemon", id, pokemon);
    return Results.Ok(updatePokemon);
});


app.MapDelete("/pokemon/{id}", async (string id, MongoDBContext db) =>
{
    var pokemon = await db.DeletePokemon("Pokemon", id);
    return Results.Ok(pokemon);
});


app.UseHttpsRedirection();

app.Run();
