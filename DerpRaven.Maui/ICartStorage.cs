using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui
{
    public interface ICartStorage
    {
        void AddCartItem(ProductDto product);
        Task CheckOut();
        void ClearCart();
        void RemoveCartItem(CartItem item);
        void SaveCartItems(List<CartItem> items);
        void UpdateCartItem(CartItem item);
    }
}