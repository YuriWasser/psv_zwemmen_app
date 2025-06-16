using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Training
{
    public int Id { get; private set; }
    public int ZwembadId { get; private set; }
    public DateTime Datum { get; private set; }
    public TimeSpan Tijd { get; private set; }

    public Training(int id, int zwembadId, DateTime datum, TimeSpan tijd)
    {
        Id = id;
        ZwembadId = zwembadId;
        Datum = datum;
        Tijd = tijd;
    }
}