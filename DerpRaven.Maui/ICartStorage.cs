using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui
{
    public interface ICartStorage
    {
        bool CanCheckOut { get; }
        bool IsNonce { get; }

        void AddCartItem(ProductDto product);
        void AddNonce(string nonce);
        Task CheckOut();
        void ClearCart();
        void RemoveCartItem(CartItem item);
        void SaveCartItems(List<CartItem> items);
        void UpdateCartItem(CartItem item);
        void VerifyCanCheckOut();
    }
}