using AutoMapper;
using GestionProduit_API;
using GestionProduit_API.Controller;
using GestionProduit_API.Controllers;
using GestionProduit_API.Models.DTO;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    [TestClass]
    public class ProduitTest
    {
        private Mock<ProduitManager> _mockRepository;
        private ProduitsController _produitsController;
        private IMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ProduitManager>();

            // Use the shared AutomapperProfile configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            _mapper = config.CreateMapper();

            _produitsController = new ProduitsController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetProduits_ReturnsListOfProduits()
        {
            // Arrange
            var produits = new List<ProduitDTO>
            {
                new ProduitDTO { Id = 1, Nom = "Produit A", Marque=null, Type=null },
                new ProduitDTO { Id = 2, Nom = "Produit B", Marque=null, Type=null }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(produits);

            // Act
            var actionResult = await _produitsController.GetProduits();

            // Assert
            Assert.IsNotNull(actionResult.Value, "GetProduits: La liste de produits est null.");
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Produit>), "GetProduits: Le type retourné n'est pas une liste de produits.");
            Assert.AreEqual(2, ((IEnumerable<Produit>)actionResult.Value).Count(), "GetProduits: Le nombre de produits retourné est incorrect.");
        }

        [TestMethod]
        public async Task GetProduitById_ExistingId_ReturnsProduit()
        {
            // Arrange
            var produit = new ProduitDetailDTO { Id = 1, Nom = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(produit);

            // Act
            var actionResult = await _produitsController.GetProduitById(1);
            var result = actionResult.Result as OkObjectResult;  // Vérifie si c'est un OkObjectResult

            // Assert
            Assert.IsNotNull(result, "GetProduitById: La réponse n'est pas de type OkObjectResult.");
            var returnedProduit = result.Value as ProduitDetailDTO;
            Assert.IsNotNull(returnedProduit, "GetProduitById: Le produit retourné est null.");
            Assert.AreEqual(produit.Nom, returnedProduit.Nom, "GetProduitById: Les produits ne sont pas égaux.");
        }


        [TestMethod]
        public async Task GetProduitByString_ExistingNom_ReturnsProduit()
        {
            // Arrange
            var produit = new ProduitDetailDTO { Id = 1, Nom = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByStringAsync("Produit A")).ReturnsAsync(produit);

            // Act
            var actionResult = await _produitsController.GetProduitByString("Produit A");
            var result = actionResult.Result as OkObjectResult;  // Vérifie si le résultat est de type OkObjectResult

            // Assert
            Assert.IsNotNull(result, "GetProduitByString: La réponse n'est pas de type OkObjectResult.");
            var returnedProduit = result.Value as ProduitDetailDTO;
            Assert.IsNotNull(returnedProduit, "GetProduitByString: Le produit retourné est null.");
            Assert.AreEqual(produit.Nom, returnedProduit.Nom, "GetProduitByString: Les produits ne sont pas égaux.");
        }


        [TestMethod]
        public async Task PostProduit_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            var produitSN = _mapper.Map<ProduitSansNavigation>(produit); // Map vers ProduitSansNavigation

            var produitDTO = _mapper.Map<ProduitDetailDTO>(produit); // Map vers ProduitDetailDTO pour le retour attendu

            _mockRepository.Setup(repo => repo.PostAsync(It.IsAny<ProduitSansNavigation>()))
                           .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _produitsController.PostProduit(produitSN);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result, "PostProduit: Le produit n'a pas été créé.");

            // Vérifie que le type retourné est bien ProduitDetailDTO
            Assert.IsInstanceOfType(result.Value, typeof(ProduitSansNavigation), "PostProduit: Le produit créé n'est pas du bon type.");

            var returnedProduit = result.Value as ProduitDetailDTO;
            Assert.IsNotNull(returnedProduit, "PostProduit: Le produit retourné est null.");
            Assert.AreEqual(produit.NomProduit, returnedProduit.Nom, "PostProduit: Le nom du produit créé est incorrect.");
            Assert.AreEqual(produit.IdProduit, returnedProduit.Id, "PostProduit: L'ID du produit créé est incorrect.");
        }






        [TestMethod]
        public async Task DeleteProduit_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingProduit = new ProduitDetailDTO { Id = 1, Nom = "Produit A" };

            // Simuler la récupération du produit existant
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduit);

            // Simuler la suppression du produit
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var actionResult = await _produitsController.DeleteProduit(1);

            // Assert
            // Vérifie que le résultat est de type NoContentResult
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "DeleteProduit: Le produit n'a pas été correctement supprimé.");

            // Vérifie que la méthode DeleteAsync a été appelée avec l'ID correct
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once, "DeleteProduit: La méthode DeleteAsync n'a pas été appelée correctement.");
        }

    }
}
