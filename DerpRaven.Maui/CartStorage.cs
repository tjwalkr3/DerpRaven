using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui
{
    public class CartStorage : ICartStorage
    {
        private const string CartKey = "CartItems";

        public void SaveCartItems(List<CartItem> items)
        {
            var json = JsonSerializer.Serialize(items);
            Preferences.Clear(CartKey);
            Preferences.Set(CartKey, json);
        }

        public void AddCartItem(ProductDto product)
        {
            var item = new CartItem
            {
                Name = product.Name,
                ImageUrl = Path.Combine(FileSystem.CacheDirectory, $"{product.ImageIds[0]}.png"),
                Quantity = product.Quantity,
                Price = product.Price,
                ProductTypeId = product.ProductTypeId
            };

            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cartItems.Add(item);
            }
            SaveCartItems(cartItems);
        }

        public void RemoveCartItem(CartItem item)
        {
            var cartItems = GetCartItems();
            cartItems.Remove(item);
            SaveCartItems(cartItems);
        }

        public List<CartItem> GetCartItems()
        {
            var json = Preferences.Get(CartKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return new List<CartItem>();
            }
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public void ClearCart()
        {
            Preferences.Remove(CartKey);
        }

        public void UpdateCartItem(CartItem item)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                existingItem.Price = item.Price;
            }
            SaveCartItems(cartItems);
        }

        public void CheckOut() {
            var cartItems = GetCartItems();
            List<OrderedProductDto> products = new List<OrderedProductDto>();
            foreach (var item in cartItems) {
                products.Add(new OrderedProductDto {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            //Checkout api call here
            ClearCart();
        }
    }

    public class CartItem
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is CartItem item)
            {
                return Name == item.Name &&
                       ImageUrl == item.ImageUrl &&
                       Quantity == item.Quantity &&
                       Price == item.Price &&
                       ProductTypeId == item.ProductTypeId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ImageUrl, Quantity, Price, ProductTypeId);
        }
    }
}