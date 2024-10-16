using AutoMapper;
using GestionProduit_API.Controller;
using GestionProduit_API.Models.EntityFramework;
using GestionProduit_API.Models.Manager;
using GestionProduit_API.Models.ModelTemplate;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    [TestClass]
    public class TypeProduitTest
    {
        private Mock<TypeProduitManager> _mockRepository;
        private TypeProduitsController _typeProduitsController;
        private IMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<TypeProduitManager>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProduitSansNavigation, Produit>();
                cfg.CreateMap<TypeProduitSansNavigation, TypeProduit>();  // Ajoute cette ligne
            });

            _mapper = config.CreateMapper();

            _typeProduitsController = new TypeProduitsController(_mockRepository.Object, _mapper);
        }

        [TestMethod]
        public async Task GetProduits_ReturnsListOfTypeProduits()
        {
            // Arrange
            var types = new List<TypeProduit>
            {
                new TypeProduit { Idtypeproduit = 1, Nomtypeproduit = "Type A" },
                new TypeProduit { Idtypeproduit = 2, Nomtypeproduit = "Type B" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(types);

            // Act
            var actionResult = await _typeProduitsController.GetTypes();

            // Assert
            Assert.IsNotNull(actionResult.Value, "GetProduits: La liste des types de produits est null.");
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<TypeProduit>), "GetProduits: Le type retourné n'est pas une liste de types de produits.");
            Assert.AreEqual(2, ((IEnumerable<TypeProduit>)actionResult.Value).Count(), "GetProduits: Le nombre de types de produits retourné est incorrect.");
        }

        [TestMethod]
        public async Task GetTypeProduit_ExistingId_ReturnsTypeProduit()
        {
            // Arrange
            var typeProduit = new TypeProduit { Idtypeproduit = 1, Nomtypeproduit = "Type A" };
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<TypeProduit>(typeProduit));

            // Act
            var actionResult = await _typeProduitsController.GetTypeProduitById(1);

            // Assert
            Assert.IsNotNull(actionResult.Value, "GetTypeProduit: Le type de produit est null.");
            Assert.AreEqual(typeProduit.Nomtypeproduit, actionResult.Value.Nomtypeproduit, "GetTypeProduit: Le nom du type de produit ne correspond pas.");
        }

        [TestMethod]
        public async Task GetTypeProduit_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ActionResult<TypeProduit>((TypeProduit)null));

            // Act
            var actionResult = await _typeProduitsController.GetTypeProduitById(999);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult), "GetTypeProduit: Type de produit non trouvé, mais NotFound n'a pas été retourné.");
        }

        [TestMethod]
        public async Task PostTypeProduit_ValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var typeProduitSN = new TypeProduitSansNavigation
            {
                Nomtypeproduit = "Type Nouveau"
            };

            var typeProduit = new TypeProduit
            {
                Idtypeproduit = 1,
                Nomtypeproduit = "Type Nouveau"
            };

            _mockRepository
                .Setup(repo => repo.PostAsync(It.IsAny<TypeProduit>()))
                .Verifiable();

            // Act
            var actionResult = await _typeProduitsController.PostTypeProduit(typeProduitSN);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result, "PostTypeProduit: Le type de produit n'a pas été créé.");
            Assert.IsInstanceOfType(result.Value, typeof(TypeProduit), "PostTypeProduit: Le type de produit créé n'est pas du bon type.");
            Assert.AreEqual(typeProduit.Nomtypeproduit, ((TypeProduit)result.Value).Nomtypeproduit, "PostTypeProduit: Le nom du type de produit créé est incorrect.");
        }

        [TestMethod]
        public async Task PutTypeProduit_ValidId_UpdatesTypeProduit()
        {
            // Arrange
            var existingTypeProduit = new TypeProduit { Idtypeproduit = 1, Nomtypeproduit = "Type A" };
            var updatedTypeProduit = new TypeProduit { Idtypeproduit = 1, Nomtypeproduit = "Type B" };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<TypeProduit>(existingTypeProduit));

            _mockRepository
                .Setup(repo => repo.PutAsync(existingTypeProduit, updatedTypeProduit))
                .Verifiable();

            TypeProduitSansNavigation produitSansNavigation = new TypeProduitSansNavigation
            {
                Idtypeproduit = updatedTypeProduit.Idtypeproduit,
                Nomtypeproduit = updatedTypeProduit.Nomtypeproduit
            };

            // Act
            var actionResult = await _typeProduitsController.PutTypeProduit(1, produitSansNavigation);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "PutTypeProduit: Le type de produit n'a pas été mis à jour correctement.");
        }

        [TestMethod]
        public async Task PutTypeProduit_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var typeProduit = new TypeProduit { Idtypeproduit = 1, Nomtypeproduit = "Type A" };

            TypeProduitSansNavigation produitSansNavigation = new TypeProduitSansNavigation
            {
                Idtypeproduit = typeProduit.Idtypeproduit,
                Nomtypeproduit = typeProduit.Nomtypeproduit
            };

            // Act
            var actionResult = await _typeProduitsController.PutTypeProduit(2, produitSansNavigation);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult), "PutTypeProduit: Un ID non valide a été accepté.");
        }

        [TestMethod]
        public async Task DeleteTypeProduit_ExistingId_ReturnsNoContentAsync()
        {
            // Arrange
            var typeProduit = new TypeProduit { Idtypeproduit = 1, Nomtypeproduit = "Type A" };
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new ActionResult<TypeProduit>(typeProduit));

            // Act
            var actionResult = await _typeProduitsController.DeleteTypeProduit(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "DeleteTypeProduit: Le type de produit n'a pas été supprimé correctement.");
        }

        [TestMethod]
        public async Task DeleteTypeProduit_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ActionResult<TypeProduit>((TypeProduit)null));

            // Act
            var actionResult = await _typeProduitsController.DeleteTypeProduit(999);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult), "DeleteTypeProduit: Type de produit non trouvé, mais NotFound n'a pas été retourné.");
        }
    }
}
