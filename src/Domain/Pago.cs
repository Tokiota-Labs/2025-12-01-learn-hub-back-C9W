using System;

public class Pago
{
    public Guid Id { get; set; }
    public Guid InscripcionId { get; set; }
    public Inscripcion Inscripcion { get; set; }
    public string MetodoPago { get; set; } // Tarjeta de crédito, PayPal, etc.
    public string NumeroTarjeta { get; set; }
    public DateTime FechaVencimientoTarjeta { get; set; }
    public string CVV { get; set; }
    public decimal CantidadPago { get; set; }
    public DateTime FechaPago { get; set; }
}