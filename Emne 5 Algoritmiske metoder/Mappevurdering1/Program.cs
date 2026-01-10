using Mappevurdering1.Analysis;
using Mappevurdering1.Experiments;
using Mappevurdering1.Models;

namespace Mappevurdering1;

class Program
{
    static void Main()
    {
        Console.WriteLine("\n" + "\n" + ConstStrings.DoubleSeparator);
        Console.WriteLine("Fredrik Magee sin Mappeinnlevering 1 for Eksamen i Emne 5 Algoritmiske metoder");
        Console.WriteLine(ConstStrings.DoubleSeparator + "\n" + "\n");
        Console.WriteLine("Oppgave 1 og oppgave 2 i mappeinnlevering 1 i emne 5 tar for seg lineære og binære søk, og " +
                          "ber oss å teste metodene vi har implimentert med et spesifikt datasett. Dette gjøres i" +
                          "unittester i TestMappevurdering1. \n" +
                          "Se readme.md med detaljer for kjøring av testene.");

        Console.WriteLine("Trykk på en tast for å gå videre til oppgave 3");
        Console.ReadKey();
        
        Console.WriteLine("\nOppgave 3");
        Console.WriteLine("Vi tester lineære søk med 3 forskjellige datatyper, array, list og LinkedList, " +
                          "og vi tester binære søk med array og list. Vi tar tiden på hvert søk for å måle ytelsen.\n");
        Console.WriteLine("Trykk på en tast for å se reusultatet fra eksperimentene");
        Console.ReadKey();
        Console.WriteLine("Dette er resultatet fra eksperimentene:");
        Console.WriteLine(ConstStrings.Separator);
        
        // Setter opp og utfører eksperimentet
        Experiment.ArrangeExperiment();
        
        Console.WriteLine("\nResultater er lagret. Trykk en tast for å se analysen.");
        Console.ReadLine();
        Console.WriteLine(ConstStrings.DoubleSeparator);
        
        
        ResultsAnalyzer.RunAnalyze();
        
        
        Console.WriteLine(ConstStrings.DoubleSeparator);
        Console.WriteLine("Analyse fullført. Se i readme.md for teori relatert til mappeinnlevering og eksamen.");
    }
}