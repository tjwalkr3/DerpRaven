using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage;
using DerpRaven.Shared.Dtos;
using DerpRaven.Shared.ApiClients;

namespace DerpRaven.Maui
{
    public class CartStorage : ICartStorage
    {
        private const string CartKey = "CartItems";
        public bool CanCheckOut { get; private set; } = false;
        private readonly IOrderedProductClient OrderedProductClient;
        private readonly IProductClient ProductClient;

        public CartStorage(IOrderedProductClient orderedProductClient, IProductClient productClient)
        {
            OrderedProductClient = orderedProductClient;
            ProductClient = productClient;
        }


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
                ProductId = product.Id,
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

        public static List<CartItem> GetCartItems()
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

        public static decimal GetCartTotal()
        {
            var cartItems = GetCartItems();
            return cartItems.Sum(item => item.Quantity * item.Price);
        }

        public async Task CheckOut()
        {
            var cartItems = GetCartItems();
            List<OrderedProductDto> products = new List<OrderedProductDto>();
            foreach (var item in cartItems)
            {
                products.Add(new OrderedProductDto
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
                ProductDto? oldproduct = await ProductClient.GetProductByIdAsync(item.ProductId);
                if (oldproduct != null)
                {
                    oldproduct.Quantity -= item.Quantity;
                    //TODO: Update the product quantity in the database using the ProductClient.
                    //ProductClient.
                }
                await OrderedProductClient.CreateOrderedProducts(products);
            }
            //Checkout page call and response


            //Checkout api call here
            ClearCart();
        }


        //For checking out stuff
        string? nonce;
        public bool IsNonce { get => nonce != null; }
        public void AddNonce(string nonce)
        {
            this.nonce = nonce;
        }

        public void VerifyCanCheckOut()
        {

        }
    }

    public class CartItem
    {
        public string Name { get; set; } = string.Empty;
        public int ProductId { get; set; }
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