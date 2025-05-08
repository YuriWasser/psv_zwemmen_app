using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Training
{
    public int Id { get; set; }
    public int ZwembadId { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan StartTijd { get; set; }

    public Training(int zwembadId, DateTime datum, TimeSpan startTijd)
    {
        ZwembadId = zwembadId;
        Datum = datum;
        StartTijd = startTijd;
    }
}