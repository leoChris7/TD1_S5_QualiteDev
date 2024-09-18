using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.EntityFrameworkCore;
using Moq;
using tprevision.Controllers;
using tprevision.Models.Repository;
using TPRevision.Models.EntityFramework;

namespace Tests
{   
    [TestClass]
    public class ProduitTest
    {
        [TestMethod]
        public ActionResult<Produit> GetProductById_RightIdPassed(int id)
        {
            // Arrange
            // Création de l'objet Produit avec initialisation partielle des propriétés
            Produit product = new Produit
            {
                Description = "La nouvelle Javel",
                NomProduit = "Produit Nettoyant",
                NomPhoto = "javel.jpg",
                UriPhoto = "/images/javel.jpg",
                IdTypeProduit = 1,
                IdMarque = 2,
                StockReel = 100,
                StockMin = 10,
                StockMax = 200
            };
             
            var mockRepository = new Mock<IDataRepository<Produit>>();


            mockRepository.Setup(x => x.GetById(1).Result).Returns(product);

            var produitsController = new ProduitsController(mockRepository.Object);
            // Act
            var actionResult = produitsController.GetProduit(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(product, actionResult.Value as Produit);
        }

        [TestMethod]
        public void Postproduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var produitController = new ProduitsController(mockRepository.Object);
            Produit product = new Produit
            {
                Description = "La nouvelle Javel",
                NomProduit = "Produit Nettoyant",
                NomPhoto = "javel.jpg",
                UriPhoto = "/images/javel.jpg",
                IdTypeProduit = 1,
                IdMarque = 2,
                StockReel = 100,
                StockMin = 10,
                StockMax = 200
            };

            // Act
            var actionResult = produitController.PostProduit(product).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Produit>), "Pas un ActionResult<Produit>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Produit), "Pas un produit");
            product.IdProduit = ((Produit)result.Value).IdProduit;
            Assert.AreEqual(product, (Produit)result.Value, "Produits pas identiques");
        }
    }
}