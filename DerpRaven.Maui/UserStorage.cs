using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerpRaven.Maui;
public class UserStorage : IUserStorage {
    public string GetEmail() {
        return Preferences.Get("Email", string.Empty);
    }

    public void SetEmail(string email) {
        Preferences.Set("Email", email);
    }
}

