using System;

namespace Meos_API.Authentication;

 public class UserAccountService
 {
     private List<UserAccount> _userAccountList;

     public UserAccountService()
     {
        //soon database integratie
         _userAccountList = new List<UserAccount>
         {
             new UserAccount{ UserName = "noah", Password = "noah", Role = "Administrator" },
             new UserAccount{ UserName = "user", Password = "user", Role = "User" }
         };
     }

     public UserAccount? GetUserAccountByUserName(string userName)
     {
         return _userAccountList.FirstOrDefault(x => x.UserName == userName);
     }
 }
