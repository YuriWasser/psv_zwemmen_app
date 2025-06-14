using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Core.Domain;
using Core.Service;
using Core.Interface;
using Core.Dto;
using Core.Exceptions;

namespace TestLayer;

[TestClass]
public class GebruikerTest
{
    private Mock<IGebruikerRepository> _mockRepo;
    private Mock<ILogger<GebruikerService>> _mockLogger;
    private Mock<IConfiguration> _mockConfig;
    private GebruikerService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IGebruikerRepository>();
        _mockLogger = new Mock<ILogger<GebruikerService>>();
        _mockConfig = new Mock<IConfiguration>();
        _service = new GebruikerService(_mockRepo.Object, _mockLogger.Object, _mockConfig.Object);
    }
    
    [TestMethod]
    public void GetById_GeeftGebruiker_TerugAlsGevonden()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "test", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.GetById(1)).Returns(gebruiker);

        // Act
        var result = _service.GetById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(gebruiker, result);
    }
    
    [TestMethod]
    public void GetById_GooitException_AlsNietGevonden()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetById(1)).Returns((Gebruiker)null);

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.GetById(1));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het ophalen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Gebruiker niet gevonden", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void GetById_GooitException_AlsRepositoryFoutGeeft()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetById(1)).Throws(new Exception("db error"));

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.GetById(1));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het ophalen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db error", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void Add_GooitArgumentException_BijLegeVelden()
    {
        // Gebruikersnaam leeg
        var ex1 = Assert.ThrowsException<Exception>(() => _service.Add(1, "", "pw", "mail", "voor", "achter", "SWIM"));
        Assert.IsInstanceOfType(ex1.InnerException, typeof(ArgumentException));
        Assert.AreEqual("gebruikersnaam", ((ArgumentException)ex1.InnerException).ParamName, true);

        // Wachtwoord leeg
        var ex2 = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "", "mail", "voor", "achter", "SWIM"));
        Assert.IsInstanceOfType(ex2.InnerException, typeof(ArgumentException));
        Assert.AreEqual("wachtwoord", ((ArgumentException)ex2.InnerException).ParamName, true);

        // Email leeg
        var ex3 = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "pw", "", "voor", "achter", "SWIM"));
        Assert.IsInstanceOfType(ex3.InnerException, typeof(ArgumentException));
        Assert.AreEqual("email", ((ArgumentException)ex3.InnerException).ParamName, true);

        // Voornaam leeg
        var ex4 = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "pw", "mail", "", "achter", "SWIM"));
        Assert.IsInstanceOfType(ex4.InnerException, typeof(ArgumentException));
        Assert.AreEqual("voornaam", ((ArgumentException)ex4.InnerException).ParamName, true);

        // Achternaam leeg
        var ex5 = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "pw", "mail", "voor", "", "SWIM"));
        Assert.IsInstanceOfType(ex5.InnerException, typeof(ArgumentException));
        Assert.AreEqual("achternaam", ((ArgumentException)ex5.InnerException).ParamName, true);

        // FunctieCode leeg
        var ex6 = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "pw", "mail", "voor", "achter", ""));
        Assert.IsInstanceOfType(ex6.InnerException, typeof(ArgumentException));
        Assert.AreEqual("functieCode", ((ArgumentException)ex6.InnerException).ParamName, true);
    }

    [TestMethod]
    public void Add_GeeftGebruikerTerug_BijSucces()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "hashed", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.Add(It.IsAny<Gebruiker>())).Returns(gebruiker);

        // Act
        var result = _service.Add(1, "user", "pw", "mail", "voor", "achter", "SWIM");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(gebruiker, result);
    }
    
    [TestMethod]
    public void Add_GooitException_AlsRepositoryNullTeruggeeft()
    {
        // Arrange
        _mockRepo.Setup(r => r.Add(It.IsAny<Gebruiker>())).Returns((Gebruiker)null);

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "pw", "mail", "voor", "achter", "SWIM"));
        Assert.AreEqual("Er is een fout opgetreden bij het toevoegen van de gebruiker", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.IsTrue(ex.InnerException.Message.Contains("Toevoegen van gebruiker is mislukt"));
    }

    [TestMethod]
    public void Add_GooitException_AlsRepositoryFoutGeeft()
    {
        // Arrange
        _mockRepo.Setup(r => r.Add(It.IsAny<Gebruiker>())).Throws(new Exception("db error"));

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.Add(1, "user", "pw", "mail", "voor", "achter", "SWIM"));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het toevoegen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db error", ex.InnerException.Message);
    }

    [TestMethod]
    public void Update_GeeftTrueTerug_BijSucces()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.Update(gebruiker)).Returns(true);

        // Act
        var result = _service.Update(gebruiker);

        // Assert
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void Update_GeeftFalseTerug_BijMislukking()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.Update(gebruiker)).Returns(false);

        // Act
        var result = _service.Update(gebruiker);

        // Assert
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void Update_GooitException_AlsRepositoryFoutGeeft()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.Update(gebruiker)).Throws(new Exception("db error"));

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.Update(gebruiker));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het updaten van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db error", ex.InnerException.Message);
    }

    [TestMethod]
    public void Delete_GeeftTrueTerug_BijSucces()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.GetById(1)).Returns(gebruiker);
        _mockRepo.Setup(r => r.Delete(gebruiker)).Returns(true);

        // Act
        var result = _service.Delete(1);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Delete_GooitException_AlsGebruikerNietGevonden()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetById(1)).Returns((Gebruiker)null);

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.Delete(1));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het verwijderen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Gebruiker niet gevonden", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void Delete_GooitException_AlsRepositoryFoutGeeft()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.GetById(1)).Returns(gebruiker);
        _mockRepo.Setup(r => r.Delete(gebruiker)).Throws(new Exception("db error"));

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.Delete(1));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het verwijderen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db error", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetByGebruikersnaam_GeeftGebruikerTerug_BijSucces()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", "pw", "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Returns(gebruiker);

        // Act
        var result = _service.GetByGebruikersnaam("user");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(gebruiker, result);
    }

    [TestMethod]
    public void GetByGebruikersnaam_GooitArgumentException_BijLegeGebruikersnaam()
    {
        var ex = Assert.ThrowsException<Exception>(() => _service.GetByGebruikersnaam(""));
        Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException));
        Assert.AreEqual("gebruikersnaam", ((ArgumentException)ex.InnerException).ParamName, true);
    }

    [TestMethod]
    public void GetByGebruikersnaam_GooitException_AlsNietGevonden()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Returns((Gebruiker)null);

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.GetByGebruikersnaam("user"));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het ophalen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Gebruiker niet gevonden", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void GetByGebruikersnaam_GooitException_AlsRepositoryFoutGeeft()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Throws(new Exception("db error"));

        // Act & Assert
        var ex = Assert.ThrowsException<Exception>(() => _service.GetByGebruikersnaam("user"));
        Assert.IsTrue(ex.Message.Contains("Er is een fout opgetreden bij het ophalen van de gebruiker"));
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db error", ex.InnerException.Message);
    }

    [TestMethod]
    public void Login_GeeftLoginDtoTerug_BijSucces()
    {
        // Arrange
        var wachtwoord = "pw";
        var hashed = PasswordHasher.HashPassword(wachtwoord);
        var gebruiker = new Gebruiker(1, "user", hashed, "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Returns(gebruiker);

        // Act
        var dto = _service.Login("user", "pw", "supersecretkey1234567890", "issuer");

        // Assert
        Assert.IsNotNull(dto);
        Assert.AreEqual(gebruiker.Id, dto.Id);
        Assert.AreEqual(gebruiker.Gebruikersnaam, dto.Gebruikersnaam);
        Assert.IsFalse(string.IsNullOrWhiteSpace(dto.TokenString));
        Assert.AreEqual("Zwemmer", dto.FunctieCode);
    }
    
    [TestMethod]
    public void Login_GooitInvalidDataException_BijLegeArgumenten()
    {
        Assert.ThrowsException<InvalidDataException>(() => _service.Login("", "pw", "key", "issuer"));
        Assert.ThrowsException<InvalidDataException>(() => _service.Login("user", "", "key", "issuer"));
        Assert.ThrowsException<InvalidDataException>(() => _service.Login("user", "pw", "", "issuer"));
        Assert.ThrowsException<InvalidDataException>(() => _service.Login("user", "pw", "key", ""));
    }

    [TestMethod]
    public void Login_GooitGebruikerNotFoundException_AlsGebruikerNietBestaat()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Returns((Gebruiker)null);

        // Act & Assert
        Assert.ThrowsException<GebruikerNotFoundException>(() => _service.Login("user", "pw", "key", "issuer"));
    }
    
    [TestMethod]
    public void Login_GooitGebruikerNotFoundException_AlsWachtwoordOnjuist()
    {
        // Arrange
        var gebruiker = new Gebruiker(1, "user", PasswordHasher.HashPassword("pw"), "mail", "voor", "achter", "SWIM");
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Returns(gebruiker);

        // Act & Assert
        Assert.ThrowsException<GebruikerNotFoundException>(() => _service.Login("user", "wrongpw", "key", "issuer"));
    }
    
    [TestMethod]
    public void Login_GooitException_AlsRepositoryFoutGeeft()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByGebruikersnaam("user")).Throws(new Exception("db error"));

        // Act & Assert
        Assert.ThrowsException<Exception>(() => _service.Login("user", "pw", "key", "issuer"));
    }
    
    [TestMethod]
    [DataRow(null, "Gebruiker")]
    [DataRow("", "Gebruiker")]
    [DataRow("TRAIN", "Trainer")]
    [DataRow("train", "Trainer")]
    [DataRow("SWIM", "Zwemmer")]
    [DataRow("swim", "Zwemmer")]
    [DataRow("EXSWIM", "OudZwemmer")]
    [DataRow("exswim", "OudZwemmer")]
    [DataRow("ANDERS", "Gebruiker")]
    public void NormalizeFunctieCode_GeeftJuisteWaardeTerug(string input, string expected)
    {
        var result = _service.NormalizeFunctieCode(input);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Logout_LogtInformation()
    {
        _service.Logout();
        _mockLogger.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Gebruiker uitgelogd")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}