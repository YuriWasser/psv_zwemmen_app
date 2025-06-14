using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using Core.Domain;
using Core.Service;
using Core.Interface;
using Core.Exceptions;

namespace TestLayer;

[TestClass]
public class AfstandenTest
{
    private Mock<IAfstandRepository> _mockRepo;
    private Mock<ILogger<AfstandService>> _mockLogger;
    private AfstandService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IAfstandRepository>();
        _mockLogger = new Mock<ILogger<AfstandService>>();
        _service = new AfstandService(_mockRepo.Object, _mockLogger.Object);
    }
    
    [TestMethod]
    public void GetAll_GeeftLijstMetAfstanden_BijSucces()
    {
        var expected = new List<Afstand>
        {
            new Afstand(1, 50, "50m Vrij"),
            new Afstand(2, 100, "100m Rug")
        };
        _mockRepo.Setup(r => r.GetAll()).Returns(expected);

        var result = _service.GetAll();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("50m Vrij", result[0].Beschrijving);
    }

    [TestMethod]
    public void GetAll_GooitDatabaseException_BijDatabaseFout()
    {
        _mockRepo.Setup(r => r.GetAll()).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de afstanden", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetAll_GooitException_BijOnverwachteFout()
    {
        _mockRepo.Setup(r => r.GetAll()).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de afstanden", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetById_GeeftAfstandTerug_BijSucces()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.GetByID(1)).Returns(afstand);

        var result = _service.GetById(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(afstand, result);
    }

    [TestMethod]
    public void GetById_GooitException_AlsNietGevonden()
    {
        _mockRepo.Setup(r => r.GetByID(1)).Returns((Afstand)null);

        var ex = Assert.ThrowsException<Exception>(() => _service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Afstand niet gevonden", ex.InnerException.Message);
    }
    
    [TestMethod]
    public void GetById_GooitDatabaseException_BijDatabaseFout()
    {
        _mockRepo.Setup(r => r.GetByID(1)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetById_GooitException_BijOnverwachteFout()
    {
        _mockRepo.Setup(r => r.GetByID(1)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Add_GeeftAfstandTerug_BijSucces()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Add(It.IsAny<Afstand>())).Returns(afstand);

        var result = _service.Add(1, 50, "50m Vrij");

        Assert.IsNotNull(result);
        Assert.AreEqual(afstand, result);
    }

    [TestMethod]
    public void Add_GooitArgumentException_BijMetersKleinerOfGelijkAanNul()
    {
        var ex1 = Assert.ThrowsException<Exception>(() => _service.Add(1, 0, "50m Vrij"));
        Assert.IsInstanceOfType(ex1.InnerException, typeof(ArgumentException));

        var ex2 = Assert.ThrowsException<Exception>(() => _service.Add(1, -10, "50m Vrij"));
        Assert.IsInstanceOfType(ex2.InnerException, typeof(ArgumentException));
    }

    [TestMethod]
    public void Add_GooitArgumentException_BijLegeBeschrijving()
    {
        var ex1 = Assert.ThrowsException<Exception>(() => _service.Add(1, 50, ""));
        Assert.IsInstanceOfType(ex1.InnerException, typeof(ArgumentException));

        var ex2 = Assert.ThrowsException<Exception>(() => _service.Add(1, 50, "   "));
        Assert.IsInstanceOfType(ex2.InnerException, typeof(ArgumentException));

        var ex3 = Assert.ThrowsException<Exception>(() => _service.Add(1, 50, null));
        Assert.IsInstanceOfType(ex3.InnerException, typeof(ArgumentException));
    }

    [TestMethod]
    public void Update_GeeftTrueTerug_BijSucces()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Update(afstand)).Returns(true);

        var result = _service.Update(afstand);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Update_GeeftFalseTerug_BijMislukking()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Update(afstand)).Returns(false);

        var result = _service.Update(afstand);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Update_GooitDatabaseException_BijDatabaseFout()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Update(afstand)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.Update(afstand));
        Assert.AreEqual("Er is een fout opgetreden bij het updaten van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Update_GooitException_BijOnverwachteFout()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Update(afstand)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.Update(afstand));
        Assert.AreEqual("Er is een fout opgetreden bij het updaten van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Delete_GeeftTrueTerug_BijSucces()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Delete(afstand)).Returns(true);

        var result = _service.Delete(afstand);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Delete_GeeftFalseTerug_BijMislukking()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Delete(afstand)).Returns(false);

        var result = _service.Delete(afstand);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Delete_GooitDatabaseException_BijDatabaseFout()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Delete(afstand)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.Delete(afstand));
        Assert.AreEqual("Er is een fout opgetreden bij het verwijderen van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Delete_GooitException_BijOnverwachteFout()
    {
        var afstand = new Afstand(1, 50, "50m Vrij");
        _mockRepo.Setup(r => r.Delete(afstand)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.Delete(afstand));
        Assert.AreEqual("Er is een fout opgetreden bij het verwijderen van de afstand", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetByProgrammaId_GeeftLijstMetAfstanden_BijSucces()
    {
        var expected = new List<Afstand>
        {
            new Afstand(1, 50, "50m Vrij"),
            new Afstand(2, 100, "100m Rug")
        };
        _mockRepo.Setup(r => r.GetByProgrammaId(1)).Returns(expected);

        var result = _service.GetByProgrammaId(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("50m Vrij", result[0].Beschrijving);
    }

    [TestMethod]
    public void GetByProgrammaId_GooitArgumentException_BijOngeldigeId()
    {
        var ex1 = Assert.ThrowsException<Exception>(() => _service.GetByProgrammaId(0));
        Assert.IsInstanceOfType(ex1.InnerException, typeof(ArgumentException));

        var ex2 = Assert.ThrowsException<Exception>(() => _service.GetByProgrammaId(-1));
        Assert.IsInstanceOfType(ex2.InnerException, typeof(ArgumentException));
    }

    [TestMethod]
    public void GetByProgrammaId_GooitDatabaseException_BijDatabaseFout()
    {
        _mockRepo.Setup(r => r.GetByProgrammaId(1)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.GetByProgrammaId(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van afstanden voor het programma", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetByProgrammaId_GooitException_BijOnverwachteFout()
    {
        _mockRepo.Setup(r => r.GetByProgrammaId(1)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.GetByProgrammaId(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van afstanden voor het programma", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }
}