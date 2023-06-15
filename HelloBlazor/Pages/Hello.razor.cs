using HelloBlazor.Models;
using HelloBlazor.Services;
using Microsoft.AspNetCore.Components;

namespace HelloBlazor.Pages
{
    public partial class Hello
    {
        public string? Prenom { get; set; }
        public string? Message
        {
            get { return EstMessageAffichable ? $"Bonjour {Prenom} !" : ""; }
        }
        public bool EstMessageAffichable
        {
            get { return !string.IsNullOrWhiteSpace(Prenom) && CouleurSelectionnee != null; }
        }
        
        public List<Couleur>? _couleurs;
        public List<Couleur>? Couleurs
        {
            get { return _couleurs; }
            set { _couleurs = value?.OrderBy(c => c.Libelle).ToList(); }
        }

        public Couleur? CouleurSelectionnee { get; set; }

        public void SelectionnerCouleur(ChangeEventArgs e)
        {
            CouleurSelectionnee = Couleurs?.FirstOrDefault(c => c.Code == e.Value?.ToString());
        }

        [Inject]
        public IStyleService StyleService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            Couleurs = await StyleService.ObtenirLesCouleurs();
        }
    }
}
