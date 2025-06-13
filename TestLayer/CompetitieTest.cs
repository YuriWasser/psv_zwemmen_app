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
    public void GetActieveCompetities_ReturnListOfCompetities_WhenNoExceptionThrown()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var expected = new List<Competitie>
        {
            new Competitie(1, "ToekomstigeCompetitie", new(2025, 7, 1), new DateOnly(2025, 12, 31), 1, 1),
        };

        mockRepo.Setup(repo => repo.GetActieveCompetities()).Returns(expected);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        //Act
        var result = service.GetActieveCompetities();

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("ToekomstigeCompetitie", result[0].Naam);
    }

    [TestMethod]
    public void GetActieveCompetities_ThrowsDatabaseException_WhenRepositoryThrowsDatabaseException()
    {
        // Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.GetActieveCompetities()).Throws(new DatabaseException("Fout bij ophalen competities"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        // Act & Assert
        var exception = Assert.ThrowsException<DatabaseException>(() => service.GetActieveCompetities());
    
        // 🔧 Pas hier de foutmelding aan
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van actieve competities", exception.Message);

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fout bij ophalen van actieve competities")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [TestMethod]
    public void GetActieveCompetities_ThrowsGenericException_WhenUnexpectedExceptionOccurs()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.GetActieveCompetities()).Throws(new Exception("Onverwachte fout"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.GetActieveCompetities());
        Assert.AreEqual("Er is een onverwachte fout opgetreden bij het ophalen van actieve competities", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(Exception));

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Onverwachte fout bij ophalen van actieve competities")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [TestMethod]
    public void GetActieveCompetities_ReturnsEmptyList_WhenNoCompetitiesExist()
    {
        // Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var expected = new List<Competitie>(); // geen competities

        mockRepo.Setup(repo => repo.GetActieveCompetities()).Returns(expected);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        // Act
        var result = service.GetActieveCompetities();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetActieveCompetities_ThrowsException_WhenRepositoryReturnsNull()
    {
        // Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.GetActieveCompetities()).Returns((List<Competitie>)null);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => service.GetActieveCompetities());
        Assert.AreEqual("Er is een onverwachte fout opgetreden bij het ophalen van actieve competities", ex.Message);
    }

    [TestMethod]
    public void GetById_ReturnsCompetitie_WhenCompetitieExists()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var expected = new Competitie(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);

        mockRepo.Setup(repo => repo.GetById(1)).Returns(expected);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

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
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.GetById(1)).Returns((Competitie)null);
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

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
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.GetById(1)).Throws(new Exception("Fout bij ophalen competitie"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

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
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.Add(It.IsAny<Competitie>())).Returns(new Competitie(1, "RegionaleCompetitie",
            new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        //Act
        var result = service.Add(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);

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
        var mockProgRepo = new Mock<IProgrammaRepository>();

        mockRepo.Setup(repo => repo.Add(It.IsAny<Competitie>())).Throws(new Exception("Fout bij toevoegen competitie"));
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        // Act & Assert
        var exception = Assert.ThrowsException<Exception>(() =>
            service.Add(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1));
        Assert.AreEqual("Er is een fout opgetreden bij het toevoegen van de competitie", exception.Message);
        Assert.IsInstanceOfType(exception.InnerException, typeof(Exception));
    }

    [TestMethod]
    public void Add_CallsRepositoryWithCorrectCompetitieObject()
    {
        // Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitie = new Competitie(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);

        mockRepo.Setup(r => r.Add(It.IsAny<Competitie>())).Returns(competitie);

        // Act
        service.Add(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);

        // Assert
        mockRepo.Verify(repo => repo.Add(It.Is<Competitie>(c =>
            c.Naam == competitie.Naam &&
            c.StartDatum == competitie.StartDatum &&
            c.EindDatum == competitie.EindDatum &&
            c.ZwembadId == competitie.ZwembadId &&
            c.ProgrammaId == competitie.ProgrammaId)), Times.Once);
    }
    
    [TestMethod]
    public void Add_ThrowsArgumentException_WhenNaamIsNullOrWhitespace()
    {
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, null, new(2023, 1, 1), new(2023, 12, 31), 1, 1));
        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "", new(2023, 1, 1), new(2023, 12, 31), 1, 1));
        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "   ", new(2023, 1, 1), new(2023, 12, 31), 1, 1));
    }
    
    [TestMethod]
    public void Add_ThrowsArgumentException_WhenEindDatumVoorStartDatum()
    {
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "Test", new(2023, 12, 31), new(2023, 1, 1), 1, 1));
    }

    [TestMethod]
    public void Add_ThrowsArgumentException_WhenZwembadIdIsZeroOrNegative()
    {
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "Test", new(2023, 1, 1), new(2023, 12, 31), 0, 1));
        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "Test", new(2023, 1, 1), new(2023, 12, 31), -1, 1));
    }

    [TestMethod]
    public void Add_ThrowsArgumentException_WhenProgrammaIdIsZeroOrNegative()
    {
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "Test", new(2023, 1, 1), new(2023, 12, 31), 1, 0));
        Assert.ThrowsException<ArgumentException>(() =>
            service.Add(1, "Test", new(2023, 1, 1), new(2023, 12, 31), 1, -1));
    }
    
    [TestMethod]
    public void Update_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitie = new Competitie(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
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
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitie = new Competitie(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);
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
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitie = new Competitie(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);

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
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

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
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitie = new Competitie(1, "RegionaleCompetitie", new(2023, 1, 1), new DateOnly(2023, 12, 31), 1, 1);

        mockRepo.Setup(repo => repo.GetById(1)).Returns(competitie);
        mockRepo.Setup(repo => repo.Delete(competitie))
            .Throws(new InvalidOperationException("Fout bij verwijderen competitie"));

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
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitieId = 1;
        var expected = new List<Programma>
        {
            new Programma(1, 1, "Programma1", new DateTime(2023, 3, 25), new TimeSpan(10, 0, 0)),
            new Programma(2, 2, "Programma2", new DateTime(2023, 3, 25), new TimeSpan(10, 0, 0))
        };

        mockRepo.Setup(repo => repo.GetProgrammaVoorCompetitie(competitieId)).Returns(expected);

        //Act
        var result = service.GetProgrammaVoorCompetitie(competitieId);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Programma1", result[0].Omschrijving);
    }

    [TestMethod]
    public void GetProgrammaVoorCompetitie_ThrowsException_WhenRepositoryThrows()
    {
        //Arrange
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        var competitieId = 1;
        mockRepo.Setup(repo => repo.GetProgrammaVoorCompetitie(competitieId))
            .Throws(new InvalidOperationException("Fout bij ophalen programma's voor competitie"));

        //Act & Assert
        var exception = Assert.ThrowsException<Exception>(() => service.GetProgrammaVoorCompetitie(competitieId));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van programma's voor de competitie",
            exception.Message);
    }
    
    [TestMethod]
    public void GetProgrammaVoorCompetitie_ThrowsException_WhenRepositoryReturnsNull()
    {
        var mockRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var service = new CompetitieService(mockRepo.Object, mockLogger.Object, mockProgRepo.Object);

        mockRepo.Setup(r => r.GetProgrammaVoorCompetitie(1)).Returns((List<Programma>)null);

        var ex = Assert.ThrowsException<Exception>(() => service.GetProgrammaVoorCompetitie(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van programma's voor de competitie", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Geen programma's gevonden voor deze competitie", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void GetProgrammaById_ReturnsProgramma_WhenFound()
    {
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var mockCompRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();

        var programma = new Programma(1, 1, "Omschrijving", new DateTime(2023, 3, 25), new TimeSpan(10, 0, 0));
        mockProgRepo.Setup(r => r.GetById(1)).Returns(programma);

        var service = new CompetitieService(mockCompRepo.Object, mockLogger.Object, mockProgRepo.Object);
        typeof(CompetitieService)
            .GetField("_programmaRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(service, mockProgRepo.Object);

        var result = service.GetProgrammaById(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Omschrijving", result.Omschrijving);
    }

    [TestMethod]
    public void GetProgrammaById_ThrowsException_WhenNotFound()
    {
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var mockCompRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();

        mockProgRepo.Setup(r => r.GetById(1)).Returns((Programma)null);

        var service = new CompetitieService(mockCompRepo.Object, mockLogger.Object, mockProgRepo.Object);
        typeof(CompetitieService)
            .GetField("_programmaRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(service, mockProgRepo.Object);

        var ex = Assert.ThrowsException<Exception>(() => service.GetProgrammaById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van het programma", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Programma niet gevonden", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void GetProgrammaById_ThrowsDatabaseException_WhenRepositoryThrowsDatabaseException()
    {
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var mockCompRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();

        mockProgRepo.Setup(r => r.GetById(1)).Throws(new DatabaseException("db fout"));

        var service = new CompetitieService(mockCompRepo.Object, mockLogger.Object, mockProgRepo.Object);
        typeof(CompetitieService)
            .GetField("_programmaRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(service, mockProgRepo.Object);

        var ex = Assert.ThrowsException<DatabaseException>(() => service.GetProgrammaById(1));
        Assert.AreEqual("Er is een databasefout opgetreden bij het ophalen van het programma", ex.Message);
    }

    [TestMethod]
    public void GetProgrammaById_ThrowsException_WhenRepositoryThrowsOtherException()
    {
        var mockProgRepo = new Mock<IProgrammaRepository>();
        var mockCompRepo = new Mock<ICompetitieRepository>();
        var mockLogger = new Mock<ILogger<CompetitieService>>();

        mockProgRepo.Setup(r => r.GetById(1)).Throws(new Exception("andere fout"));

        var service = new CompetitieService(mockCompRepo.Object, mockLogger.Object, mockProgRepo.Object);
        typeof(CompetitieService)
            .GetField("_programmaRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(service, mockProgRepo.Object);

        var ex = Assert.ThrowsException<Exception>(() => service.GetProgrammaById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van het programma", ex.Message);
    }

}