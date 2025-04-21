using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui
{
    public interface ICartStorage
    {
        bool CanCheckOut { get; }
        bool IsNonce { get; }

        void AddCartItem(ProductDto product, int SelectedQuantity);
        void AddNonce(string nonce);
        Task<bool> CheckAndUpdateCartItemQuantities();
        Task<bool> CheckOut(string address, string email);
        void ClearCart();
        public List<CartItem> GetCartItems();
        public decimal GetCartTotal();
        void RemoveCartItem(CartItem item);
        void SaveCartItems(List<CartItem> items);
        void UpdateCartItem(CartItem item);
    }
}