using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui
{
    public interface ICartStorage
    {
        void AddCartItem(ProductDto product);
        void ClearCart();
        List<CartItem> GetCartItems();
        void RemoveCartItem(CartItem item);
        void SaveCartItems(List<CartItem> items);
        void UpdateCartItem(CartItem item);
    }
}