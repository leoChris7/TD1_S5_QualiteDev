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
                .Setup(repo => repo.GetAll())
                .Returns(marques);

            // Act
            var actionResult = await _marquesController.GetMarques();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<Marque>));
            Assert.AreEqual(2, ((IEnumerable<Marque>)result.Value).Count());
        }

        [TestMethod]
        public async Task GetMarqueById_ExistingId_ReturnsMarque()
        {
            // Arrange
            var marque = new Marque { Idmarque = 1, NomMarque = "Marque A" };
            _mockRepository
                .Setup(repo => repo.GetById(1))
                .Returns(new ActionResult<Marque>(marque));

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
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new ActionResult<Marque>((Marque)null));

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
                .Setup(repo => repo.Post(It.IsAny<Marque>()))
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
                .Setup(repo => repo.GetById(1))
                .Returns(new ActionResult<Marque>(existingMarque));

            _mockRepository
                .Setup(repo => repo.Put(existingMarque, updatedMarque))
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
            var marque = new Marque { Idmarque = 1, NomMarque = "Marque A" };
            _mockRepository
                .Setup(repo => repo.GetById(1))
                .Returns(new ActionResult<Marque>(marque));

            // Act
            var actionResult = await _marquesController.DeleteMarque(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteMarque_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new ActionResult<Marque>((Marque)null));

            // Act
            var actionResult = await _marquesController.DeleteMarque(999);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
