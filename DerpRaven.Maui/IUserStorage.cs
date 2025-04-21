namespace DerpRaven.Maui {
    public interface IUserStorage {
        string GetEmail();
        void SetEmail(string email);
    }
}