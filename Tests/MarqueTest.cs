using GestionProduit_API.Controller;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.ModelTemplate;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            Assert.IsNotNull(actionResult.Value, "GetMarques: La liste de marques est null.");
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Marque>), "GetMarques: Le type de la liste de marques n'est pas correcte.");
            Assert.AreEqual(2, ((IEnumerable<Marque>)actionResult.Value).Count(), "GetMarques: Un nombre incohérent de produits a été récupéré.");
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
            Assert.IsNotNull(actionResult.Value, "GetMarqueById: La marque est null.");
            Assert.AreEqual(marque.NomMarque, actionResult.Value.NomMarque, "GetMarqueById: Les marques ne sont pas égales.");
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
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult), "GetMarqueById: Une marque non trouvée n'a pas retourné NotFound.");
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
            Assert.IsNotNull(result, "PostMarque: La marque n'a pas été créée, est null.");
            Assert.IsInstanceOfType(result.Value, typeof(Marque), "PostMarque: La marque créée n'est pas de type Marque.");
            Assert.AreEqual(marque.NomMarque, ((Marque)result.Value).NomMarque, "La marque créée ne correspond pas à la marque voulue.");
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

            MarqueSansNavigation marqueSansNavigation = new()
            {
                Idmarque = updatedMarque.Idmarque,
                NomMarque = updatedMarque.NomMarque
            };

            // Act
            var actionResult = await _marquesController.PutMarque(1, marqueSansNavigation);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "PutMarque: La marque n'a pas été mis à jour correctement.");
        }

        [TestMethod]
        public async Task PutMarque_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var marque = new Marque { Idmarque = 1, NomMarque = "Marque A" };

            MarqueSansNavigation marqueSansNavigation = new()
            {
                Idmarque = marque.Idmarque,
                NomMarque = marque.NomMarque
            };

            // Act
            var actionResult = await _marquesController.PutMarque(2, marqueSansNavigation);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult), "PutMarque: L'id a été validé alors qu'il n'existe pas. BadRequest aurait dû être retourné.");
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

        [TestMethod]
        public async Task AreMarquesEquals_ReturnsTrue()
        {
            // Arrange
            var marque1 = new Marque
            {
                Idmarque=1,
                NomMarque="Javel Lessive"
            };

            var marque2 = new Marque
            {
                Idmarque = 2,
                NomMarque = "Javel Lessive"
            };

            // Act
            var areEqual = marque1.Equals(marque2);

            // Assert
            Assert.IsTrue(areEqual, "EqualsMarque: Les marques (sauf Id) ne sont pas égales.");
            Assert.IsNotNull(areEqual, "EqualsMarque: l'égalité des marques a retourné une valeur null");
        }

        [TestMethod]
        public async Task AreMarquesEquals_ReturnsFalse()
        {
            // Arrange
            var marque1 = new Marque
            {
                Idmarque = 1,
                NomMarque = "Javel Lessive"
            };

            var marque2 = new Marque
            {
                Idmarque = 2,
                NomMarque = "Javel Lessive Pas Efficace"
            };

            // Act
            var areEqual = marque1.Equals(marque2);

            // Assert
            Assert.IsFalse(areEqual, "EqualsMarque: Les marques (sauf Id) sont égales.");
            Assert.IsNotNull(areEqual, "EqualsMarque: l'égalité des marques a retourné une valeur null");
        }

    }
}
