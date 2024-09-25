using Microsoft.AspNetCore.Http.HttpResults;
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
    public class MarqueTest
    {
        private Mock<MarqueManager> _mockRepository;
        private MarquesController _marquesController;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<MarqueManager>();
            _marquesController = new MarquesController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetMarques_ReturnsListOfMarques()
        {
            // Arrange
            var marques = new List<Marque>
            {
                new Marque { Idmarque = 1, NomMarque = "Marque A" },
                new Marque { Idmarque = 2, NomMarque = "Marque B" }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(marques);

            // Act
            var actionResult = await _marquesController.GetMarques();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(actionResult.Value);
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Marque>));
            Assert.AreEqual(2, ((IEnumerable<Marque>)actionResult.Value).Count());
        }

        [TestMethod]
        public async Task GetMarqueById_ExistingId_ReturnsMarque()
        {
            // Arrange
            var marque = new Marque { Idmarque = 1, NomMarque = "Marque A" };
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<Marque>(marque));

            // Act
            var actionResult = await _marquesController.GetMarqueById(1);

            // Assert
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(marque.NomMarque, actionResult.Value.NomMarque);
        }

        [TestMethod]
        public async Task GetMarqueById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Marque)null); // Return null directly

            // Act
            var actionResult = await _marquesController.GetMarqueById(999);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostMarque_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var marqueSN = new MarqueSansNavigation
            {
                NomMarque = "Nouvelle Marque"
            };

            var marque = new Marque
            {
                Idmarque = 1,
                NomMarque = "Nouvelle Marque"
            };

            _mockRepository
                .Setup(repo => repo.PostAsync(It.IsAny<Marque>()))
                .Verifiable();

            // Act
            var actionResult = await _marquesController.PostMarque(marqueSN);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(Marque));
            Assert.AreEqual(marque.NomMarque, ((Marque)result.Value).NomMarque);
        }

        [TestMethod]
        public async Task PutMarque_ValidId_UpdatesMarque()
        {
            // Arrange
            var existingMarque = new Marque { Idmarque = 1, NomMarque = "Marque A" };
            var updatedMarque = new Marque { Idmarque = 1, NomMarque = "Marque B" };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<Marque>(existingMarque));

            _mockRepository
                .Setup(repo => repo.PutAsync(existingMarque, updatedMarque))
                .Verifiable();

            // Act
            var actionResult = await _marquesController.PutMarque(1, updatedMarque);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PutMarque_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var marque = new Marque { Idmarque = 1, NomMarque = "Marque A" };

            // Act
            var actionResult = await _marquesController.PutMarque(2, marque);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task DeleteMarque_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingMarque = new Marque { Idmarque = 1, NomMarque = "Marque A" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingMarque);

            // Act
            var actionResult = await _marquesController.DeleteMarque(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult)); // Check for No Content
        }


        [TestMethod]
        public async Task DeleteMarque_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(9999)).ReturnsAsync((Marque)null);

            // Act
            var actionResult = await _marquesController.DeleteMarque(9999); // Await the method call

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

    }
}
