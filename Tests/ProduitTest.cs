using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using tprevision.Controller;
using tprevision.Models.Manager.tprevision.Models.DataManager;
using tprevision.Models.ModelTemplate;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

namespace Tests
{
    [TestClass]
    public class ProduitTest
    {
        private Mock<ProduitManager> _mockRepository; // Assurez-vous que c'est la bonne interface
        private ProduitsController _produitsController;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ProduitManager>();
            _produitsController = new ProduitsController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task PostProduitSansNavigation_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var produitSN = new ProduitSansNavigation
            {
                NomProduit = "Produit Test",
                Description = "Description Test",
                StockMax = 100,
                StockMin = 10,
                StockReel = 50,
                UriPhoto = "http://example.com/photo.jpg",
                NomPhoto = "photo.jpg",
                IdTypeProduit = 1,
                IdMarque = 1
            };

            var produit = new Produit
            {
                NomProduit = produitSN.NomProduit,
                Description = produitSN.Description,
                StockMax = produitSN.StockMax,
                StockMin = produitSN.StockMin,
                StockReel = produitSN.StockReel,
                UriPhoto = produitSN.UriPhoto,
                NomPhoto = produitSN.NomPhoto,
                IdTypeProduit = produitSN.IdTypeProduit,
                IdMarque = produitSN.IdMarque
            };

            _mockRepository
                .Setup(manager => manager.GetById(It.IsAny<int>()))
                .Returns(new ActionResult<Produit>(produit));

            // Act
            var actionResult = await _produitsController.PostProduit(produitSN);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Produit));
            Assert.AreEqual(produit.NomProduit, ((Produit)result.Value).NomProduit);
        }

        [TestMethod]
        public async Task GetProduitById_ExistingId_ReturnsProduct()
        {
            // Arrange
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit Test" };
            _mockRepository
                .Setup(manager => manager.GetById(It.IsAny<int>()))
                .Returns(new ActionResult<Produit>(produit));

            // Act
            var actionResult = _produitsController.GetProduit(1);

            // Assert
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(produit.NomProduit, actionResult.Value.NomProduit);
        }

        [TestMethod]
        public async Task GetProduitById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(manager => manager.GetById(9999))
                .Returns(new ActionResult<Produit>((Produit)null));
            
            // Act
            var actionResult = _produitsController.GetProduit(99999);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteProduit_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var produitTest = new Produit
            {
                NomProduit = "Produit Test",
                Description = "Description Test",
                StockMax = 100,
                StockMin = 10,
                StockReel = 50,
                UriPhoto = "http://example.com/photo.jpg",
                NomPhoto = "photo.jpg",
                IdTypeProduit = 1,
                IdMarque = 1
            };


            _mockRepository
                .Setup(manager => manager.GetById(1))
                .Returns(new ActionResult<Produit>(produitTest));

            // Act
            var actionResult = _produitsController.DeleteProduit(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteProduit_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(9999)).Returns((Produit)null);

            // Act
            var actionResult = _produitsController.DeleteProduit(99);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PutProduit_ValidId_UpdatesProduct()
        {
            // Arrange
            var existingProduit = new Produit { IdProduit = 1, NomProduit = "Produit Test" };
            var updatedProduit = new Produit { IdProduit = 1, NomProduit = "Produit Mis à Jour" };

            _mockRepository
                .Setup(manager => manager.GetById(1))
                .Returns(new ActionResult<Produit>(existingProduit));

            _mockRepository
                .Setup(manager => manager.Put(existingProduit, updatedProduit));

            // Act
            var actionResult = _produitsController.PutProduit(1, updatedProduit);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PutProduit_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit Test" };

            // Act
            var actionResult = _produitsController.PutProduit(2, produit);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }
    }
}
