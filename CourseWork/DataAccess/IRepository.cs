using System.Collections.Generic;
using Model;

namespace DataAccess
{
    public interface IRepository
    {
        void AddClient(User obj);

        void DeleteClient(int id);

        bool SignIn(string login, string password, User user);
        
        bool LoginSearch(string login);

        void MakeOrder(Pharmacy pharmacy, List<CartLine> cart, User user);

        void UpdateQuantity(List<CartLine> lineCollection);

        MedicinalProduct ReadById(int id);

        Pharmacy ReadByName(string name);

        List<string[]> GetMedicine();

        List<string[]> GetPharmacy();
    }
}
