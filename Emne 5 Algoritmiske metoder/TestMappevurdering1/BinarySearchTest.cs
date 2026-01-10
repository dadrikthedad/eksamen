using Mappevurdering1.Algorithms;

namespace TestMappevurdering1;

public class BinarySearchTest
{
    /// <summary>
    /// Test for å finne indexen til tallet 47
    /// </summary>
    [Fact]
    public void BinarySearchTest1()
    {
        // Arrange
        int[] array = [72, 35, 22, 89, 59, 10, 68, 47];
        MergeSortMethods.Sort(array);
        
        int target = 47;
        var expectedResult = 3;
        
        // Act
        var result = BinarySearchMethods.BinarySearch(array, target);

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    /// <summary>
    /// Test for å finne indexen til tallet 90
    /// </summary>
    [Fact]
    public void BinarySearchTest2()
    {
        // Arrange
        int[] array = [72, 35, 22, 89, 59, 10, 68, 47];
        MergeSortMethods.Sort(array);
        
        int target = 89;
        var expectedResult = 7;
        
        // Act
        var result = BinarySearchMethods.BinarySearch(array, target);

        // Assert
        Assert.Equal(expectedResult, result);
        
    }
    
    /// <summary>
    /// Test for å finne indexen til tallet 100 som ikke er med i listen
    /// </summary>
    [Fact]
    public void BinarySearchTest3()
    {
        // Arrange
        int[] array = [72, 35, 22, 89, 59, 10, 68, 47];
        MergeSortMethods.Sort(array);
        
        int target = 100;
        var expectedResult = -1;
        
        // Act
        var result = BinarySearchMethods.BinarySearch(array, target);

        // Assert
        Assert.Equal(expectedResult, result);
        
    }
    
    /// <summary>
    /// Bonus test for å sjekke merge sort
    /// </summary>
    [Fact]
    public void MergeSortTest()
    {
        // Arrange
        int[] array = [72, 35, 22, 89, 59, 10, 68, 47];
        int[] arraySorted = [10, 22, 35, 47, 59, 68, 72, 89];
        
        // Act
        MergeSortMethods.Sort(array);
        
        // Assert
        Assert.Equal(arraySorted, array);
    }
}