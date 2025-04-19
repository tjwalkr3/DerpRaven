using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerpRaven.Maui;
public static class UserStorage 
{
    public static string GetEmail() {
        return Preferences.Get("Email", string.Empty);
    }

    public static void SetEmail(string email) {
        Preferences.Set("Email", email);
    }
    
}

