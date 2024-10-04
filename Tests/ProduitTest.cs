using GestionProduit_API.Controller;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.ModelTemplate;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    [TestClass]
    public class ProduitTest
    {
        private Mock<ProduitManager> _mockRepository;
        private ProduitsController _produitsController;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ProduitManager>();
            _produitsController = new ProduitsController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetProduits_ReturnsListOfProduits()
        {
            // Arrange
            var produits = new List<Produit>
    {
        new Produit { IdProduit = 1, NomProduit = "Produit A" },
        new Produit { IdProduit = 2, NomProduit = "Produit B" }
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
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Produit>(produit));

            // Act
            var actionResult = await _produitsController.GetProduitById(1);

            // Assert
            Assert.IsNotNull(actionResult.Value, "GetProduitById: Le produit est null.");
            Assert.AreEqual(produit.NomProduit, actionResult.Value.NomProduit, "GetProduitById: Les produits ne sont pas égaux.");
        }

        [TestMethod]
        public async Task GetProduitById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Produit)null);

            // Act
            var actionResult = await _produitsController.GetProduitById(999);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult), "GetProduitById: Produit non trouvé, mais NotFound n'a pas été retourné.");
        }

        [TestMethod]
        public async Task GetProduitByString_ExistingNom_ReturnsProduit()
        {
            // Arrange
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByStringAsync("Produit A")).ReturnsAsync(new ActionResult<Produit>(produit));

            // Act
            var actionResult = await _produitsController.GetProduitByString("Produit A");

            // Assert
            Assert.IsNotNull(actionResult.Value, "GetProduitByString: Le produit est null.");
            Assert.AreEqual(produit.NomProduit, actionResult.Value.NomProduit, "GetProduitByString: Les produits ne sont pas égaux.");
        }

        [TestMethod]
        public async Task GetProduitByString_NonExistingNom_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByStringAsync(It.IsAny<string>())).ReturnsAsync((Produit)null);

            // Act
            var actionResult = await _produitsController.GetProduitByString("Produit Inexistant");

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult), "GetProduitByString: Produit non trouvé, mais NotFound n'a pas été retourné.");
        }

        [TestMethod]
        public async Task PostProduit_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.PostAsync(It.IsAny<Produit>())).Verifiable();

            var nouveauProduit = new ProduitSansNavigation
            {
                IdProduit = produit.IdProduit,
                NomProduit = produit.NomProduit,
                Description = produit.Description,
                NomPhoto = produit.NomPhoto,
                UriPhoto = produit.UriPhoto,
                IdTypeProduit = produit.IdTypeProduit,
                IdMarque = produit.IdMarque,
                StockReel = produit.StockReel,
                StockMin = produit.StockMin,
                StockMax = produit.StockMax
            };

            // Act
            var actionResult = await _produitsController.PostProduit(nouveauProduit);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result, "PostProduit: Le produit n'a pas été créé.");
            Assert.IsInstanceOfType(result.Value, typeof(Produit), "PostProduit: Le produit créé n'est pas du bon type.");
            Assert.AreEqual(produit, ((Produit)result.Value), "PostProduit: Le nom du produit créé est incorrect.");
        }

        [TestMethod]
        public async Task PutProduit_ValidId_UpdatesProduit()
        {
            // Arrange
            var existingProduit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            var updatedProduit = new Produit { IdProduit = 1, NomProduit = "Produit B" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Produit>(existingProduit));
            _mockRepository.Setup(repo => repo.PutAsync(existingProduit, updatedProduit)).Verifiable();

            var produitSansNavigation = new ProduitSansNavigation
            {
                IdProduit = updatedProduit.IdProduit,
                NomProduit = updatedProduit.NomProduit
            };

            // Act
            var actionResult = await _produitsController.PutProduit(1, produitSansNavigation);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "PutProduit: Le produit n'a pas été mis à jour correctement.");
        }

        [TestMethod]
        public async Task PutProduit_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var produit = new ProduitSansNavigation { IdProduit = 1, NomProduit = "Produit A" };

            // Act
            var actionResult = await _produitsController.PutProduit(2, produit);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult), "PutProduit: Un ID non valide a été accepté.");
        }

        [TestMethod]
        public async Task DeleteProduit_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingProduit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduit);

            // Act
            var actionResult = await _produitsController.DeleteProduit(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "DeleteProduit: Le produit n'a pas été correctement supprimé.");
        }

        [TestMethod]
        public async Task DeleteProduit_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Produit)null);

            // Act
            var actionResult = await _produitsController.DeleteProduit(999);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult), "DeleteProduit: Un produit non existant n'a pas retourné NotFound.");
        }

        [TestMethod]
        public async Task AreProduitsEquals_ReturnsTrue()
        {
            // Arrange
            var produit1 = new Produit
            {
                IdProduit = 1,
                NomProduit = "Produit A",
                Description = "Description A",
                NomPhoto = "photoA.png",
                UriPhoto = "http://example.com/photoA.png",
                IdTypeProduit = 2,
                IdMarque = 3,
                StockReel = 10,
                StockMin = 5,
                StockMax = 20
            };

            var produit2 = new Produit
            {
                IdProduit = 2,
                NomProduit = "Produit A",
                Description = "Description A",
                NomPhoto = "photoA.png",
                UriPhoto = "http://example.com/photoA.png",
                IdTypeProduit = 2,
                IdMarque = 3,
                StockReel = 10,
                StockMin = 5,
                StockMax = 20
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(produit1);
            _mockRepository.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(produit2);

            // Act
            var areEqual = produit1.Equals(produit2);

            // Assert
            Assert.IsTrue(areEqual, "EqualsProduit: Les produits (sauf Id) ne sont pas égaux.");
            Assert.IsNotNull(areEqual, "EqualsProduit: Erreur: l'égalité des produits a retourné une valeur null");
        }

        [TestMethod]
        public async Task AreProduitsEquals_ReturnsFalse()
        {
            // Arrange
            var produit1 = new Produit
            {
                IdProduit = 1,
                NomProduit = "Produit Différent",
                Description = "Description A",
                NomPhoto = "photoA.png",
                UriPhoto = "http://example.com/photoA.png",
                IdTypeProduit = 2,
                IdMarque = 3,
                StockReel = 10,
                StockMin = 5,
                StockMax = 20
            };

            var produit2 = new Produit
            {
                IdProduit = 2,
                NomProduit = "Produit A",
                Description = "Description A",
                NomPhoto = "photoA.png",
                UriPhoto = "http://example.com/photoA.png",
                IdTypeProduit = 2,
                IdMarque = 3,
                StockReel = 10,
                StockMin = 5,
                StockMax = 20
            };

            // Act
            var areEqual = produit1.Equals(produit2);

            // Assert
            Assert.IsFalse(areEqual, "NotEqualsProduit: Les produits (sauf Id) sont considérés égaux alors qu'ils ne devraient pas l'être.");
            Assert.IsNotNull(areEqual, "NotEqualsProduit: l'égalité des produits a retourné une valeur null");
        }
    }
}
