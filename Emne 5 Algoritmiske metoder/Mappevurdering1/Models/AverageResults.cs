namespace Mappevurdering1.Models;

/// <summary>
/// En klasse som lagrer alle gjennomsnittene vi trenger til analyse-delen. Gjør det mer organisert og enklere å lese
/// </summary>
public class AverageResults
{
    public double LinearArrayAvg { get; set; }
    public double BinaryArrayAvg { get; set; }
    public double LinearListAvg { get; set; }
    public double BinaryListAvg { get; set; }
    public double LinkedListAvg { get; set; }
    
    public double NotFoundLinearArrayAvg { get; set; }
    public double NotFoundBinaryArrayAvg { get; set; }
    public double NotFoundLinearListAvg { get; set; }
    public double NotFoundBinaryListAvg { get; set; }
    public double NotFoundLinkedListAvg { get; set; }
    
    public double SortArrayAvg { get; set; }
    public double SortListAvg { get; set; }
}