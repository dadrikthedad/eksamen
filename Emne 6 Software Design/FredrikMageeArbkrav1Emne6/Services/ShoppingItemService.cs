using FredrikMageeArbkrav1Emne6.Models;
namespace FredrikMageeArbkrav1Emne6.Services;

public class ShoppingItemService
{
    // ======================================== Felt/Egenskaper ========================================
    // Listene for å sortere mellom elementer som er i handleliste eller i handlekurv
    // De er readonly slik at referansen ikke kan endres, innholdet kan fortsatt endres
    private readonly List<ShoppingItem> _shoppingList =
    [
        new() { Name = "Ost", Amount = 1, Category = "Meieri", EstimatedPrice = 130 },
        new() { Name = "Bolle", Amount = 8, Category = "Bakevarer", EstimatedPrice = 15 },
        new() { Name = "Eple", Amount = 15, Category = "Frukt & Grønt", EstimatedPrice = 5.65m },
        new() { Name = "Svinenakke", Amount = 1, Category = "Kjøtt", EstimatedPrice = 575.75m },
        new() { Name = "Tress is", Amount = 1, Category = "Frysevarer", EstimatedPrice = 89.99m },
        new() { Name = "Yoghurt", Amount = 4, Category = "Meieri", EstimatedPrice = 12.50m },
        new() { Name = "Gulrot", Amount = 10, Category = "Frukt & Grønt", EstimatedPrice = 3.90m },
        new() { Name = "Håndsåpe", Amount = 2, Category = "Hygiene", EstimatedPrice = 24.99m },
        new() { Name = "Kyllingfilet", Amount = 1, Category = "Kjøtt", EstimatedPrice = 189 },
        new() { Name = "Pizza", Amount = 2, Category = "Frysevarer", EstimatedPrice = 39.90m },
        new() { Name = "Brød", Amount = 1, Category = "Bakevarer", EstimatedPrice = 40 },
        new() { Name = "Tomat", Amount = 6, Category = "Frukt & Grønt", EstimatedPrice = 4.20m },
        new() { Name = "Oppvaskbørste", Amount = 1, Category = "Annet", EstimatedPrice = 29.90m },
        new() { Name = "Skinkepålegg", Amount = 1, Category = "Kjøtt", EstimatedPrice = 49.90m },
    ];
    
    private readonly List<ShoppingItem> _cartList = [
        new() { Name = "Agurk", Amount = 2, Category = "Frukt & Grønt", EstimatedPrice = 20 },
        new() { Name = "Rømme", Amount = 1, Category = "Meieri", EstimatedPrice = 22.90m },
        new() { Name = "Bærmiks", Amount = 1, Category = "Frysevarer", EstimatedPrice = 34.90m },
        new() { Name = "Hvetemel", Amount = 1, Category = "Bakevarer", EstimatedPrice = 18.50m },
        new() { Name = "Tannkrem", Amount = 1, Category = "Hygiene", EstimatedPrice = 29.90m },
        new() { Name = "Roastbiff", Amount = 1, Category = "Kjøtt", EstimatedPrice = 64.90m },
        new() { Name = "Plastfolie", Amount = 1, Category = "Annet", EstimatedPrice = 32  },
        new() { Name = "Ost", Amount = 1, Category = "Meieri", EstimatedPrice = 130 },
    ];
    
    // En liste for kategoriene
    private readonly List<string> _categoryList = ["Frukt & Grønt", "Meieri", "Kjøtt", 
        "Bakevarer", "Hygiene", "Frysevarer", "Annet"];
    
    // IReadOnlyList sikrer at det er kun servicen som kan endre listene
    public IReadOnlyList<ShoppingItem> ShoppingList => _shoppingList;
    public IReadOnlyList<ShoppingItem> CartList => _cartList;
    public IReadOnlyList<string> CategoryList  => _categoryList;
    
    
    // Eventen som forteller komponentene/siden at UI-en har endret seg
    public event Action? StateChange;
    
    // Viser en error message i Home-siden hvis noe går galt
    public string? ErrorMessage { get; set; }
    
    // ======================================== Metoder========================================
    // Trigger eventen slik at alle komponenter som lytter til StateChange endrer seg. Home lytter til denne
    private void OnStateChange() => StateChange?.Invoke();
    
    // Når bruker legger til et objekt i handleliste
    public void AddShoppingListItem(ShoppingItem shoppingItem)
    {
        if (!_categoryList.Contains(shoppingItem.Category.Trim()))
            _categoryList.Add(shoppingItem.Category.Trim());
        
        _shoppingList.Insert(0, shoppingItem);
        OnStateChange();
    }
    
    // Når bruker markerer et objekt som kjøpt i handleliste
    public void PurchaseShoppingListItem(ShoppingItem shoppingItem)
    {
        if (!_shoppingList.Remove(shoppingItem))
            ErrorMessage = $"Vare med navn '{shoppingItem.Name}' eksisterer ikke i handlelisten.";
        else
            _cartList.Insert(0, shoppingItem);
        OnStateChange();
    }
    
    // Når bruker sletter et objekt i handleliste
    public void DeleteShoppingListItem(ShoppingItem shoppingItem)
    {
        if (!_shoppingList.Remove(shoppingItem))
            ErrorMessage = $"Vare med navn '{shoppingItem.Name}' eksisterer ikke i handlelisten.";
        
        OnStateChange();
    } 
    
    // Når bruker angrer på markingen av et objekt i handlekurv og sender det tilbake til handleliste
    public void RestoreShoppingListItem(ShoppingItem shoppingItem)
    {
        if (!_cartList.Remove(shoppingItem))
            ErrorMessage = $"Vare med navn '{shoppingItem.Name}' eksisterer ikke i handlekurven.";
        else
            _shoppingList.Insert(0, shoppingItem);
        
        OnStateChange();
    }
    
    // Når bruker sletter et objekt i handlekurv
    public void DeleteCartItem(ShoppingItem shoppingItem)
    {
        if (!_cartList.Remove(shoppingItem))
            ErrorMessage = $"Vare med navn '{shoppingItem.Name}' eksisterer ikke i handlekurven.";
        
        OnStateChange();
    } 
}