using DataAccess;
using Model;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class OrderService
    {
        private readonly IRepository _repository;

        public OrderService(IRepository repository)
        {
            _repository = repository;
        }

        public void MakeOrder(string name, List<CartLine> lineCollection, User user)
        {
            _repository.MakeOrder(_repository.ReadByName(name), lineCollection, user);
            _repository.UpdateQuantity(lineCollection);
        }
    }
}
