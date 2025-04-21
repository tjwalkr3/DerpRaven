using System.Text.Json;
using DerpRaven.Shared.Dtos;
using DerpRaven.Shared.ApiClients;

namespace DerpRaven.Maui;

public class CartStorage : ICartStorage
{
    private const string CartKey = "CartItems";
    public bool CanCheckOut { get; private set; } = false;
    private readonly IOrderedProductClient _orderedProductClient;
    private readonly IOrderClient _orderClient;
    private readonly IProductClient _productClient;
    private readonly IUserClient _userClient;
    private readonly IUserStorage _userStorage;

    public CartStorage(IOrderedProductClient orderedProductClient, IProductClient productClient, IOrderClient orderClient, IUserClient userClient, IUserStorage userStorage)
    {
        _userStorage = userStorage;
        _userClient = userClient;
        _orderedProductClient = orderedProductClient;
        _productClient = productClient;
        _orderClient = orderClient;
    }

    public void SaveCartItems(List<CartItem> items)
    {
        var json = JsonSerializer.Serialize(items);
        Preferences.Clear(CartKey);
        Preferences.Set(CartKey, json);
    }

    public void AddCartItem(ProductDto product, int SelectedQuantity)
    {
        var item = new CartItem
        {
            Name = product.Name,
            ProductId = product.Id,
            ImageUrl = Path.Combine(FileSystem.CacheDirectory, $"{product.ImageIds[0]}.png"),
            Quantity = SelectedQuantity,
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

    public decimal GetCartTotal()
    {
        var cartItems = GetCartItems();
        return cartItems.Sum(item => item.Quantity * item.Price);
    }

    public async Task<bool> CheckOut(string address, string email)
    {
        var cartItems = GetCartItems();
        List<OrderedProductDto> products = new List<OrderedProductDto>();

        if (cartItems.Count > 0 && await CheckAndUpdateCartItemQuantities())
        {
            string? userEmail = _userStorage.GetEmail();
            UserDto userdto = await _userClient.GetUserByEmailAsync(userEmail);
            // Create order
            OrderDto order = new OrderDto
            {
                Address = address,
                Email = email,
                OrderDate = DateTime.Now,
                UserId = userdto.Id
            };
            int orderId = await _orderClient.CreateOrderAsync(order);

            foreach (var item in cartItems)
            {
                products.Add(new OrderedProductDto
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderID = orderId
                });

                ProductDto? oldproduct = await _productClient.GetProductByIdAsync(item.ProductId);
                if (oldproduct != null)
                {
                    oldproduct.Quantity -= item.Quantity;
                    await _productClient.UpdateProductAsync(oldproduct);
                }
            }

            //Checkout page call and response
            await _orderedProductClient.CreateOrderedProducts(products);
            ClearCart();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CheckAndUpdateCartItemQuantities()
    {
        bool quantitiesHaveNotChanged = true;
        var cartItems = GetCartItems();
        List<OrderedProductDto> products = new List<OrderedProductDto>();
        foreach (var item in cartItems)
        {
            ProductDto? oldproduct = await _productClient.GetProductByIdAsync(item.ProductId);
            if (oldproduct != null && oldproduct.Quantity < item.Quantity)
            {
                item.Quantity = oldproduct.Quantity;
                quantitiesHaveNotChanged = false;
            }
        }
        SaveCartItems(cartItems);
        return quantitiesHaveNotChanged;
    }

    //For checking out stuff
    string? nonce;
    public bool IsNonce { get => nonce != null; }
    public void AddNonce(string nonce)
    {
        this.nonce = nonce;
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