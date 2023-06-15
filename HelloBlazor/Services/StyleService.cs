using HelloBlazor.Models;
using System.Net.Http.Json;

namespace HelloBlazor.Services
{
    public interface IStyleService
    {
        Task<List<Couleur>> ObtenirLesCouleurs();
    }

    public class StyleOfflineService : IStyleService
    {
        public Task<List<Couleur>> ObtenirLesCouleurs()
        {
            List<Couleur> couleurs = new List<Couleur>()
            {
                new Couleur(){Libelle ="vert fluo", Code= "green"},
                new Couleur(){Libelle ="rouge vif", Code= "red"},
                new Couleur(){Libelle ="jaune citron", Code= "yellow"},
                new Couleur(){Libelle ="gris souris", Code= "silver"},
                new Couleur(){Libelle ="pluie violette", Code= "purple"}
            };
            return Task.FromResult(couleurs);
        }
    }

    public class StyleOnlineApiService : IStyleService
    {
        public HttpClient Http { get; }
        public StyleOnlineApiService(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<Couleur>> ObtenirLesCouleurs()
        {
            var url = "https://parseapi.back4app.com/classes/Color?limit=1000";
            Http.DefaultRequestHeaders.Add("X-Parse-Application-Id", "vei5uu7QWv5PsN3vS33pfc7MPeOPeZkrOcP24yNX");
            Http.DefaultRequestHeaders.Add("X-Parse-Master-Key", "aImLE6lX86EFpea2nDjq9123qJnG0hxke416U7Je");

            var dtos = await Http.GetFromJsonAsync<ListeCouleurDTO>(url);
            if (dtos == null) throw new Exception("Impossible de récupérer les couleurs !");
            return dtos.Results
                .Select(dto => new Couleur() { 
                Code = dto.HexCode,
                Libelle = dto.Name
                })
                .ToList();
        }
    }
}
