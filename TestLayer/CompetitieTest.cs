using System;
using Moq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Domain;
using Core.Service;
using Core.Exceptions;
using Core.Interface;

namespace TestLayer;

[TestClass]
public class CompetitieTest
{
    [TestMethod]
    public void GetAll_ReturnListOfCompetities_WhenNoExceptionThrown()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();  //nepData, dit doen ik zodat ik niet de echte database aanroep
        var mockLogger = new Mock<ILogger<CompetitieService>>(); //nepLogger
        var expected = new List<Competitie>
        {
            new Competitie (1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1),
        };
        
        mockRepo.Setup(repo => repo.GetAll()).Returns(expected); //Als je dit aanroept, dan krijg je de expected terug
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object); //echte service, neppe repo en logger
        
        //Act
        var result = service.GetAll();
        
        //Assert  //controleer of de expected gelijk is aan de result
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("RegionaleCompetitie", result[0].Naam);

    }

    [TestMethod]
    public void GetAll_ThrowsDatabaseException_WhenRepositoryThrowsDatabaseException()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.GetAll()).Throws(new DatabaseException("Fout bij ophalen competities"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act & Assert
        var exception = Assert.ThrowsException<DatabaseException>(() => service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van competities", exception.Message);
        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fout bij ophalen competities")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);        
    }

    [TestMethod]
    public void GetAll_ThrowsGenericException_WhenUnexpectedExceptionOccurs()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.GetAll()).Throws(new Exception("Onverwachte fout"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van competities", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(Exception));
        
        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fout bij ophalen competities")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyList_WhenNoCompetitiesExist()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var expected = new List<Competitie>(); 
        
        mockRepo.Setup(repo => repo.GetAll()).Returns(expected);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act
        var result = service.GetAll();
        
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count); 
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyList_WhenRepositoryReturnsNull()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.GetAll()).Returns((List<Competitie>)null);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van competities", ex.Message);
        
    }
    
    [TestMethod]
    public void GetById_ReturnsCompetitie_WhenCompetitieExists()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var expected = new Competitie(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        
        mockRepo.Setup(repo => repo.GetById(1)).Returns(expected);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act
        var result = service.GetById(1);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("RegionaleCompetitie", result.Naam);
    }

    [TestMethod]
    public void GetById_ThrowsException_WhenCompetitieNotFound()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.GetById(1)).Returns((Competitie)null);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de competitie", exception.Message);
        Assert.IsNotNull(exception.InnerException);
    }

    [TestMethod]
    public void GetById_ThrowsException_WhenRepositoryThrows()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.GetById(1)).Throws(new Exception("Fout bij ophalen competitie"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de competitie", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(Exception));
    }

    [TestMethod]
    public void Add_ReturnsCompetitie_WhenValidInput()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.Add(It.IsAny<Competitie>())).Returns(1);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        //Act
        var result = service.Add(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("RegionaleCompetitie", result.Naam);
    }

    [TestMethod]
    public void Add_ThrowsException_WhenRepositoryThrows()
    {
        // Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        
        mockRepo.Setup(repo => repo.Add(It.IsAny<Competitie>())).Throws(new Exception("Fout bij toevoegen competitie"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        // Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.Add(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1));
        Assert.AreEqual("Er is een fout opgetreden bij het toevoegen van de competitie", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(Exception));
    }

    [TestMethod]
    public void Add_CallsRepositoryWithCorrectCompetitieObject()
    {
        // Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        var competitie = new Competitie(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        
        mockRepo.Setup(r => r.Add(It.IsAny<Competitie>())).Returns(1);
        
        // Act
        service.Add(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        
        // Assert
        mockRepo.Verify(repo => repo.Add(It.Is<Competitie>(c =>
            c.Naam == competitie.Naam &&
            c.StartDatum == competitie.StartDatum &&
            c.EindDatum == competitie.EindDatum &&
            c.ZwembadId == competitie.ZwembadId &&
            c.ProgrammaId == competitie.ProgrammaId)), Times.Once);
    }

    [TestMethod]
    public void Update_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        var competitie = new Competitie(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        mockRepo.Setup(repo => repo.Update(competitie)).Returns(true);
        
        //Act
        var result = service.Update(competitie);
        
        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Update_ThrowsException_WhenRepositoryThrows()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        var competitie = new Competitie(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        mockRepo.Setup(repo => repo.Update(competitie)).Throws(new Exception("Fout bij updaten competitie"));
        
        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.Update(competitie));
        Assert.AreEqual("Er is een fout opgetreden bij het bijwerken van de competitie", exception.Message);
    }

    [TestMethod]
    public void Delete_ReturnsTrue_WhenCompetitieFoundAndDeleted()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        var competitie = new Competitie(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        
        mockRepo.Setup(repo => repo.GetById(1)).Returns(competitie);
        mockRepo.Setup(repo => repo.Delete(competitie)).Returns(true);
        
        //Act
        var result = service.Delete(1);
        
        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Delete_ThrowsException_WhenCompetitieNotFound()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        mockRepo.Setup(repo => repo.GetById(1)).Returns((Competitie)null);
        
        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.Delete(1));
        Assert.AreEqual("Er is een fout opgetreden bij het verwijderen van de competitie", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(Exception));
        Assert.AreEqual("Competitie niet gevonden voor verwijderen", exception.InnerException.Message);
    }

    [TestMethod]
    public void Delete_ThrowsException_WhenRepositoryThrows()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        var competitie = new Competitie(1, "RegionaleCompetitie", new (2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
        
        mockRepo.Setup(repo => repo.GetById(1)).Returns(competitie);
        mockRepo.Setup(repo => repo.Delete(competitie)).Throws(new InvalidOperationException("Fout bij verwijderen competitie"));
        
        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.Delete(1));
        Assert.AreEqual("Er is een fout opgetreden bij het verwijderen van de competitie", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidOperationException));
    }

    [TestMethod]
    public void GetProgrammaVoorCompetitie_ReturnsList_WhenRepositoryReturnsList()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
        
        var competitieId = 1;
        var expected = new List<Programma>
        {
            new Programma(1, 1,"Programma1", new DateTime(2023, 3, 25), new TimeSpan(10,0, 0)),
            new Programma(2, 2,"Programma2", new DateTime(2023, 3, 25), new TimeSpan(10, 0, 0))
        };
        
        mockRepo.Setup(repo => repo.GetProgrammaVoorCompetitie(competitieId)).Returns(expected);
        
        //Act
        var result = service.GetProgrammaVoorCompetitie(competitieId);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Programma1", result[0].Omschrijving);
    }

    // [TestMethod]    !!!!!!!Exception handling toepassen in de service, daarna kan ik verder.
    // public void GetProgrammaVoorCompetitie_ThrowsException_WhenRepositoryThrows()
    // {
    //     //Arrange
    //     var mockRepo = new Mock<ICompetitieRepository>();
    //     var mockLogger = new Mock<ILogger<CompetitieService>>();
    //     var service = new CompetitieService(mockRepo.Object, mockLogger.Object);
    //     
    //     var competitieId = 1;
    //     mockRepo.Setup(repo => repo.GetProgrammaVoorCompetitie(competitieId)).Throws(new InvalidOperationException("Fout bij ophalen programma's voor competitie"));
    //     
    //     //Act & Assert
    //     var exception = Assert.ThrowsException<Exception>(() => service.GetProgrammaVoorCompetitie(competitieId));
    //     Assert.AreEqual("Fout bij ophalen programma's voor competitie", exception.Message);
    // }

}