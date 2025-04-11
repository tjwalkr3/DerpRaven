using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Maui.Storage;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui;
internal class CartStorage {
    private const string CartKey = "CartItems";

    public static void SaveCartItems(List<CartItem> items) {
        var json = JsonSerializer.Serialize(items);
        Preferences.Set(CartKey, json);
    }

    public static void AddCartItem(ProductDto product, string imageurl) {
        var item = new CartItem {
            Name = product.Name,
            ImageUrl = imageurl,
            //ImageUrl = GetImageUrl(product.ImageIds),
            Quantity = product.Quantity,
            Price = product.Price,
            ProductTypeId = product.ProductTypeId
        };
        //if the item is already in the cart, increase the quantity
        var existingItem = GetCartItems().FirstOrDefault(i => i.Name == item.Name);
        if (existingItem != null) {
            existingItem.Quantity += item.Quantity;
        } else {
            var cartItems = GetCartItems();
            cartItems.Add(item);
        }
        SaveCartItems(GetCartItems());
    }

    //private static string GetImageUrl(List<int> ids) {
    //    string imageUrl = string.Empty;
    //    // Assuming you have a method to get the image URL by ID
    //    imageUrl = GetImageUrlById(ids[0]);
    //    return imageUrl;
    //}

    public static void RemoveCartItem(CartItem item) {
        var cartItems = GetCartItems();
        cartItems.Remove(item);
        SaveCartItems(cartItems);
    }

    public static List<CartItem> GetCartItems() {
        var json = Preferences.Get(CartKey, string.Empty);
        if (string.IsNullOrEmpty(json)) {
            return new List<CartItem>();
        }
        return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
    }

    public static void ClearCart() {
        Preferences.Remove(CartKey);
    }

    public static void UpdateCartItem(CartItem item) {
        var cartItems = GetCartItems();
        var existingItem = cartItems.FirstOrDefault(i => i.Name == item.Name);
        if (existingItem != null) {
            existingItem.Quantity = item.Quantity;
            existingItem.Price = item.Price;
        }
        SaveCartItems(cartItems);
    }


}

public class CartItem {
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int ProductTypeId { get; set; }
}