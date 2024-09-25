using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using tprevision.Controller;
using tprevision.Models.DataManager;
using tprevision.Models.ModelTemplate;
using TPRevision.Models.EntityFramework;

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
            var produits = new List<Produit>
            {
                new Produit { IdProduit = 1, NomProduit = "Produit A" },
                new Produit { IdProduit = 2, NomProduit = "Produit B" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(produits);

            var actionResult = await _produitsController.GetProduits();

            Assert.IsNotNull(actionResult.Value);
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Produit>));
            Assert.AreEqual(2, ((IEnumerable<Produit>)actionResult.Value).Count());
        }

        [TestMethod]
        public async Task GetProduitById_ExistingId_ReturnsProduit()
        {
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Produit>(produit));

            var actionResult = await _produitsController.GetProduitById(1);

            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(produit.NomProduit, actionResult.Value.NomProduit);
        }

        [TestMethod]
        public async Task GetProduitById_NonExistingId_ReturnsNotFound()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Produit)null);

            var actionResult = await _produitsController.GetProduitById(999);

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        // Ajout du test pour GetProduitByString
        [TestMethod]
        public async Task GetProduitByString_ExistingNom_ReturnsProduit()
        {
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByStringAsync("Produit A")).ReturnsAsync(new ActionResult<Produit>(produit));

            var actionResult = await _produitsController.GetProduitByString("Produit A");

            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(produit.NomProduit, actionResult.Value.NomProduit);
        }

        [TestMethod]
        public async Task GetProduitByString_NonExistingNom_ReturnsNotFound()
        {
            _mockRepository.Setup(repo => repo.GetByStringAsync(It.IsAny<string>())).ReturnsAsync((Produit)null);

            var actionResult = await _produitsController.GetProduitByString("Produit Inexistant");

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostProduit_ValidModel_ReturnsCreatedAtAction()
        {
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

            var actionResult = await _produitsController.PostProduit(nouveauProduit);

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Produit));
            Assert.AreEqual(produit.NomProduit, ((Produit)result.Value).NomProduit);
        }

        [TestMethod]
        public async Task PutProduit_ValidId_UpdatesProduit()
        {
            var existingProduit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            var updatedProduit = new Produit { IdProduit = 1, NomProduit = "Produit B" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Produit>(existingProduit));
            _mockRepository.Setup(repo => repo.PutAsync(existingProduit, updatedProduit)).Verifiable();

            var actionResult = await _produitsController.PutProduit(1, updatedProduit);

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PutProduit_InvalidId_ReturnsBadRequest()
        {
            var produit = new Produit { IdProduit = 1, NomProduit = "Produit A" };

            var actionResult = await _produitsController.PutProduit(2, produit);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task DeleteProduit_ExistingId_ReturnsNoContent()
        {
            var existingProduit = new Produit { IdProduit = 1, NomProduit = "Produit A" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduit);

            var actionResult = await _produitsController.DeleteProduit(1);

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteProduit_NonExistingId_ReturnsNotFound()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Produit)null);

            var actionResult = await _produitsController.DeleteProduit(999);

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
