using Mappevurdering1.Experiments;
using Mappevurdering1.Models;

namespace Mappevurdering1.Analysis;

/// <summary>
/// Klassen som brukes til å lagre resultatene fra eksperimentene til fil
/// </summary>
public static class FileWriter
{
      /// <summary>
    /// Oppretter en fil til hver datatype og en felles fil, og lagrer resultatene i tilhørende fil
    /// </summary>
    /// <param name="result">En rad i resultattabellen med resultatene fra søket</param>
    /// <param name="dataType">Typen liste resultatet kommer fra</param>
    public static void SaveResultToFile(string result, TypeEnum dataType)
    {
        try 
        {
            // Hvis ikke filene allerede eksisterer, så oppretter vi filene med headeren
            if (!File.Exists(ConstStrings.AllResultsFilePath) || !File.Exists(ConstStrings.ArrayResultsFilePath) 
                                                              || !File.Exists(ConstStrings.ListResultsFilePath)
                                                              || !File.Exists(ConstStrings.LinkedListResultsFilePath))
            {
                CreateFiles();
            }
            
            // Lagrer resultatet til hver tilhørende fil
            if (dataType == TypeEnum.Array)
                File.AppendAllText(ConstStrings.ArrayResultsFilePath, "\n" + result);
            else if (dataType == TypeEnum.List)
                File.AppendAllText(ConstStrings.ListResultsFilePath, "\n" + result);
            else
                File.AppendAllText(ConstStrings.LinkedListResultsFilePath, "\n" + result);

            File.AppendAllText(ConstStrings.AllResultsFilePath, "\n" + result);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while saving to file: {e}");
        }
    }

    private static void CreateFiles()
    {
        // Headeren er øverste linje i Consolen og i filene hvis de opprettes
        var header = ResultTableBuilder.GetHeader();
        Console.WriteLine(header + ConstStrings.Separator);
           
        File.AppendAllText(ConstStrings.AllResultsFilePath, header+ConstStrings.Separator);
        File.AppendAllText(ConstStrings.ArrayResultsFilePath, header+ConstStrings.Separator);
        File.AppendAllText(ConstStrings.ListResultsFilePath, header+ConstStrings.Separator);
        File.AppendAllText(ConstStrings.LinkedListResultsFilePath, header+ConstStrings.Separator);
    }
}