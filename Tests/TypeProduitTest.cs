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
    public class TypeProduitTest
    {
        private Mock<TypeProduitManager> _mockRepository;
        private TypeProduitsController _typeProduitsController;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<TypeProduitManager>();
            _typeProduitsController = new TypeProduitsController(_mockRepository.Object);
        }

        [TestMethod]
        public void GetTypes_ReturnsListOfTypeProduits()
        {
            // Arrange
            var types = new List<TypeProduit>
            {
                new TypeProduit { Idtypeproduit = 1, nomtypeproduit = "Type A" },
                new TypeProduit { Idtypeproduit = 2, nomtypeproduit = "Type B" }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(types);

            // Act
            var actionResult = _typeProduitsController.GetTypes();

            // Assert
            var result = actionResult.Result.Value as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<TypeProduit>));
            Assert.AreEqual(2, ((IEnumerable<TypeProduit>)result.Value).Count());
        }

        [TestMethod]
        public void GetTypeProduit_ExistingId_ReturnsTypeProduit()
        {
            // Arrange
            var typeProduit = new TypeProduit { Idtypeproduit = 1, nomtypeproduit = "Type A" };
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<TypeProduit>(typeProduit));

            // Act
            var actionResult = _typeProduitsController.GetTypeProduit(1);

            // Assert
            Assert.IsNotNull(actionResult.Result.Value);
            Assert.AreEqual(typeProduit.nomtypeproduit, actionResult.Result.Value.nomtypeproduit);
        }

        [TestMethod]
        public void GetTypeProduit_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ActionResult<TypeProduit>((TypeProduit)null));

            // Act
            var actionResult = _typeProduitsController.GetTypeProduit(999);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostTypeProduit_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var typeProduitSN = new TypeProduitSansNavigation
            {
                nomtypeproduit = "Type Nouveau"
            };

            var typeProduit = new TypeProduit
            {
                Idtypeproduit = 1,
                nomtypeproduit = "Type Nouveau"
            };

            _mockRepository
                .Setup(repo => repo.PostAsync(It.IsAny<TypeProduit>()))
                .Verifiable();

            // Act
            var actionResult = await _typeProduitsController.PostTypeProduit(typeProduitSN);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(TypeProduit));
            Assert.AreEqual(typeProduit.nomtypeproduit, ((TypeProduit)result.Value).nomtypeproduit);
        }

        [TestMethod]
        public void PutTypeProduit_ValidId_UpdatesTypeProduit()
        {
            // Arrange
            var existingTypeProduit = new TypeProduit { Idtypeproduit = 1, nomtypeproduit = "Type A" };
            var updatedTypeProduit = new TypeProduit { Idtypeproduit = 1, nomtypeproduit = "Type B" };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<TypeProduit>(existingTypeProduit));

            _mockRepository
                .Setup(repo => repo.PutAsync(existingTypeProduit, updatedTypeProduit))
                .Verifiable();

            // Act
            var actionResult = _typeProduitsController.PutTypeProduit(1, updatedTypeProduit);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public void PutTypeProduit_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var typeProduit = new TypeProduit { Idtypeproduit = 1, nomtypeproduit = "Type A" };

            // Act
            var actionResult = _typeProduitsController.PutTypeProduit(2, typeProduit);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteTypeProduit_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var typeProduit = new TypeProduit { Idtypeproduit = 1, nomtypeproduit = "Type A" };
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<TypeProduit>(typeProduit));

            // Act
            var actionResult = _typeProduitsController.DeleteTypeProduit(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteTypeProduit_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ActionResult<TypeProduit>((TypeProduit)null));

            // Act
            var actionResult = _typeProduitsController.DeleteTypeProduit(999);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
