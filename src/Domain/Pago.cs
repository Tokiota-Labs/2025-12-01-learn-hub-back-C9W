using System;

public class Pago
{
    public Guid Id { get; set; }
    public Guid InscripcionId { get; set; }
    public Inscripcion Inscripcion { get; set; }
    public string MetodoPago { get; set; } // Tarjeta de crédito, PayPal, etc.
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
}