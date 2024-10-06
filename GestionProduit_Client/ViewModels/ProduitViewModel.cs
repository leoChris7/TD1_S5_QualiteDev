using GestionProduit_Client.Models;
using GestionProduit_Client.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionProduit_Client.ViewModels
{
    public class ProduitViewModel : INotifyPropertyChanged
    {
        private readonly IService _service;
        private Produit _selectedProduit;
        private ObservableCollection<Produit> _lesProduits;
        private bool _isEditMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProduitViewModel(IService service)
        {
            _service = service;
            _selectedProduit = new Produit();
            _lesProduits = new ObservableCollection<Produit>();
            _isEditMode = false;
        }

        public ObservableCollection<Produit> LesProduits
        {
            get => _lesProduits;
            set
            {
                _lesProduits = value;
                OnPropertyChanged();
            }
        }

        public Produit SelectedProduit
        {
            get => _selectedProduit;
            set
            {
                _selectedProduit = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadProduitsAsync()
        {
            var produits = await _service.GetProduitsAsync("produits");
            LesProduits = new ObservableCollection<Produit>(produits);
        }

        public async Task SaveProduitAsync()
        {
            if (_isEditMode)
            {
                await _service.PutProduitAsync($"produits/{SelectedProduit.IdProduit}", SelectedProduit);
            }
            else
            {
                await _service.PostProduitAsync("produits", SelectedProduit);
            }

            await LoadProduitsAsync();
            ResetProduit();
        }

        public void EditProduit(Produit produit)
        {
            SelectedProduit = new Produit
            {
                IdProduit = produit.IdProduit,
                NomProduit = produit.NomProduit,
                Description = produit.Description,
                IdMarque = produit.IdMarque,
                IdTypeProduit = produit.IdTypeProduit
            };
            IsEditMode = true;
        }

        public void ResetProduit()
        {
            SelectedProduit = new Produit();
            IsEditMode = false;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
