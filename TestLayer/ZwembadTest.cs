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
public class ZwembadTest
{
    private Mock<IZwembadRepository> _mockRepo;
    private Mock<ILogger<ZwembadService>> _mockLogger;
    private ZwembadService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IZwembadRepository>();
        _mockLogger = new Mock<ILogger<ZwembadService>>();
        _service = new ZwembadService(_mockRepo.Object, _mockLogger.Object);
    }

    [TestMethod]
    public void GetAll_GeeftLijstMetZwembaden_BijSucces()
    {
        var expected = new List<Zwembad>
        {
            new Zwembad(1, "De Tongelreep", "Eindhoven"),
            new Zwembad(2, "Pieter van den Hoogenband", "Eindhoven")
        };
        _mockRepo.Setup(r => r.GetAll()).Returns(expected);

        var result = _service.GetAll();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("De Tongelreep", result[0].Naam);
    }

    [TestMethod]
    public void GetAll_GooitDatabaseException_BijDatabaseFout()
    {
        _mockRepo.Setup(r => r.GetAll()).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de zwembaden", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetAll_GooitException_BijOnverwachteFout()
    {
        _mockRepo.Setup(r => r.GetAll()).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.GetAll());
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van de zwembaden", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetById_GeeftZwembadTerug_BijSucces()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.GetById(1)).Returns(zwembad);

        var result = _service.GetById(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(zwembad, result);
    }

    [TestMethod]
    public void GetById_GooitException_AlsNietGevonden()
    {
        _mockRepo.Setup(r => r.GetById(1)).Returns((Zwembad)null);

        var ex = Assert.ThrowsException<Exception>(() => _service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("Zwembad niet gevonden", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetById_GooitDatabaseException_BijDatabaseFout()
    {
        _mockRepo.Setup(r => r.GetById(1)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void GetById_GooitException_BijOnverwachteFout()
    {
        _mockRepo.Setup(r => r.GetById(1)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.GetById(1));
        Assert.AreEqual("Er is een fout opgetreden bij het ophalen van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Add_GeeftZwembadTerug_BijSucces()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Add(It.IsAny<Zwembad>())).Returns(zwembad);

        var result = _service.Add(1, "De Tongelreep", "Eindhoven");

        Assert.IsNotNull(result);
        Assert.AreEqual(zwembad, result);
    }

    [TestMethod]
    public void Add_GooitArgumentException_BijLegeNaam()
    {
        var ex1 = Assert.ThrowsException<Exception>(() => _service.Add(1, "", "Eindhoven"));
        Assert.IsInstanceOfType(ex1.InnerException, typeof(ArgumentException));
        Assert.AreEqual("naam", ((ArgumentException)ex1.InnerException).ParamName, true);

        var ex2 = Assert.ThrowsException<Exception>(() => _service.Add(1, "   ", "Eindhoven"));
        Assert.IsInstanceOfType(ex2.InnerException, typeof(ArgumentException));
        Assert.AreEqual("naam", ((ArgumentException)ex2.InnerException).ParamName, true);

        var ex3 = Assert.ThrowsException<Exception>(() => _service.Add(1, null, "Eindhoven"));
        Assert.IsInstanceOfType(ex3.InnerException, typeof(ArgumentException));
        Assert.AreEqual("naam", ((ArgumentException)ex3.InnerException).ParamName, true);
    }

    [TestMethod]
    public void Add_GooitArgumentException_BijLeegAdres()
    {
        var ex1 = Assert.ThrowsException<Exception>(() => _service.Add(1, "De Tongelreep", ""));
        Assert.IsInstanceOfType(ex1.InnerException, typeof(ArgumentException));
        Assert.AreEqual("adres", ((ArgumentException)ex1.InnerException).ParamName, true);

        var ex2 = Assert.ThrowsException<Exception>(() => _service.Add(1, "De Tongelreep", "   "));
        Assert.IsInstanceOfType(ex2.InnerException, typeof(ArgumentException));
        Assert.AreEqual("adres", ((ArgumentException)ex2.InnerException).ParamName, true);

        var ex3 = Assert.ThrowsException<Exception>(() => _service.Add(1, "De Tongelreep", null));
        Assert.IsInstanceOfType(ex3.InnerException, typeof(ArgumentException));
        Assert.AreEqual("adres", ((ArgumentException)ex3.InnerException).ParamName, true);
    }

    [TestMethod]
    public void Add_GooitDatabaseException_BijDatabaseFout()
    {
        _mockRepo.Setup(r => r.Add(It.IsAny<Zwembad>())).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.Add(1, "De Tongelreep", "Eindhoven"));
        Assert.AreEqual("Er is een fout opgetreden bij het toevoegen van een zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Add_GooitException_BijOnverwachteFout()
    {
        _mockRepo.Setup(r => r.Add(It.IsAny<Zwembad>())).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.Add(1, "De Tongelreep", "Eindhoven"));
        Assert.AreEqual("Er is een fout opgetreden bij het toevoegen van een zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Update_GeeftTrueTerug_BijSucces()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Update(zwembad)).Returns(true);

        var result = _service.Update(zwembad);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Update_GeeftFalseTerug_BijMislukking()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Update(zwembad)).Returns(false);

        var result = _service.Update(zwembad);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Update_GooitDatabaseException_BijDatabaseFout()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Update(zwembad)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.Update(zwembad));
        Assert.AreEqual("Er is een fout opgetreden bij het updaten van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Update_GooitException_BijOnverwachteFout()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Update(zwembad)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.Update(zwembad));
        Assert.AreEqual("Er is een fout opgetreden bij het updaten van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Delete_GeeftTrueTerug_BijSucces()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Delete(zwembad)).Returns(true);

        var result = _service.Delete(zwembad);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Delete_GeeftFalseTerug_BijMislukking()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Delete(zwembad)).Returns(false);

        var result = _service.Delete(zwembad);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Delete_GooitDatabaseException_BijDatabaseFout()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Delete(zwembad)).Throws(new DatabaseException("db fout"));

        var ex = Assert.ThrowsException<DatabaseException>(() => _service.Delete(zwembad));
        Assert.AreEqual("Er is een fout opgetreden bij het verwijderen van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("db fout", ex.InnerException.Message);
    }

    [TestMethod]
    public void Delete_GooitException_BijOnverwachteFout()
    {
        var zwembad = new Zwembad(1, "De Tongelreep", "Eindhoven");
        _mockRepo.Setup(r => r.Delete(zwembad)).Throws(new Exception("onverwachte fout"));

        var ex = Assert.ThrowsException<Exception>(() => _service.Delete(zwembad));
        Assert.AreEqual("Er is een fout opgetreden bij het verwijderen van het zwembad", ex.Message);
        Assert.IsNotNull(ex.InnerException);
        Assert.AreEqual("onverwachte fout", ex.InnerException.Message);
    }
}