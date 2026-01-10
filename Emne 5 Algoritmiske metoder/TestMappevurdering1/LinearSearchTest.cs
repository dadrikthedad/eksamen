using Mappevurdering1.Algorithms;
namespace TestMappevurdering1;
public class LinearSearchTest
{   
    // Unittester for oppgave 1
    // Testene bygges opp i deler: Arrange, Act, Assert
    // Arrange går ut på å forbrede variabler og det som trengs for å utføre testen
    // I act så bruker vi metodene som skal testet
    // Assert tester at resultatet fra metodene stemmer med resultatet vi ber om i Assert-metoden
    // Assert.Equal() tar inn ønsket resultatet som første parameter, og resultatet den skal teste mot som det andre
    // F.eks. hvis resultatet av en metode i Act gir en int, så må variabelen i første parameter stemme med int-en fra resultatet
    
    /// <summary>
    /// Test for å finne indexen til tallet 34
    /// </summary>
    [Fact] // Fact-attributen kreves av xUnit for at metoden skal være en test
    public void LinearSearchTest1()
    {
        // Arrange
        int[] array = [56, 34, 78, 12, 90, 34, 12];
        int target = 34;
        var expectedResult = 1;
        
        // Act
        var result = LinearSearchMethods.LinearSearch(array, target);

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    /// <summary>
    /// Test for å finne indexen til tallet 90
    /// </summary>
    [Fact]
    public void LinearSearchTest2()
    {
        // Arrange
        int[] array = [56, 34, 78, 12, 90, 34, 12];
        int target = 90;
        var expectedResult = 4;
        
        // Act
        var result = LinearSearchMethods.LinearSearch(array, target);

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    /// <summary>
    /// Test for å finne indexen til tallet 100 som ikke er med i listen
    /// </summary>
    [Fact]
    public void LinearSearchTest3()
    {
        // Arrange
        int[] array = [56, 34, 78, 12, 90, 34, 12];
        int target = 100;
        var expectedResult = -1;
        
        // Act
        var result = LinearSearchMethods.LinearSearch(array, target);

        // Assert
        Assert.Equal(expectedResult, result);
        
    }
    
    /// <summary>
    /// Test for eksamen oppgave 2A
    /// </summary>
    [Fact]
    public void LinearSearchTestExam()
    {
        // Arrange
        int[] array = [4, 8, 2, 9, 1];
        int target = 9;
        var expectedResult = 3;
        
        // Act
        var result = LinearSearchMethods.LinearSearch(array, target);
        
        // Assert
        Assert.Equal(expectedResult, result);

    }
}