using MongoDB.Driver;
using PokemonAPILAB2.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PokemonAPILAB2.Data
{
    public class MongoDBContext
    {
        private IMongoDatabase db;

        public MongoDBContext(string database)
        {
            var connectionstring = Environment.GetEnvironmentVariable("CosmosDBconnection");
            var client = new MongoClient(connectionstring);
            db = client.GetDatabase(database);
        }

        public async Task<Pokemon> AddPokemon(string table, Pokemon pokemon)
        {
            var collection = db.GetCollection<Pokemon>(table);
            await collection.InsertOneAsync(pokemon);
            return pokemon;
        }

        public Task<List<Pokemon>> GetPokemons(string table)
        {
            var collection = db.GetCollection<Pokemon>(table);
            return collection.AsQueryable().ToListAsync();
        }

        public async Task<Pokemon> GetPokemon(string table, string id)
        {
            var collection = db.GetCollection<Pokemon>(table);

            var Pokemon = await collection
                .Find(Builders<Pokemon>.Filter.Eq("_id", id))
                .FirstOrDefaultAsync();
            return Pokemon;
        }

        public async Task<Pokemon> UpdatePokemon(string table, string id, Pokemon updatePokemon)
        {
            var collection = db.GetCollection<Pokemon>(table);
            var pokemons = await collection.AsQueryable().ToListAsync();
            var pokemon = pokemons.Find(u => u.Id == id);

            if (pokemon != null)
            {
                var filter = Builders<Pokemon>.Filter.Eq(u => u.Id, id);
                await collection.UpdateOneAsync(filter,
                    Builders<Pokemon>.Update.Set(u => u.name, updatePokemon.name).Set(u => u.type, updatePokemon.type));
            }

            return pokemon;
        }

        public async Task<Pokemon> DeletePokemon(string table, string id)
        {
            var collection = db.GetCollection<Pokemon>(table);
            var filter = Builders<Pokemon>.Filter.Eq(e => e.Id, id);

            var pokemonToDelete = await collection.Find(filter).FirstOrDefaultAsync();

            if (pokemonToDelete != null)
            {
                await collection.DeleteOneAsync(filter);
            }

            return pokemonToDelete;

        }
    }
}