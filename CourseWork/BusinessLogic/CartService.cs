using DataAccess;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class CartService
    {
        private readonly IRepository _repository;

        public CartService(IRepository repository)
        {
            _repository = repository;
        }

        public void AddItemToCart(int id, int quantity, List<CartLine> lineCollection)
        {
            CartLine line = lineCollection.Where(g => g.MedicinalProduct.Id == id).FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    MedicinalProduct = _repository.ReadById(id),
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
                line.MedicinalProduct.Price *= line.Quantity;
            }
        }

        public void ClearCart(List<CartLine> lineCollection)
        {
            lineCollection.Clear();
        }

        public void DeleteFromCart(int index, List<CartLine> lineCollection)
        {
            if (index >= 0 && index < lineCollection.Count())
                lineCollection.RemoveAt(index);
        }

        public void UpdateQuantityFromCart(int index, int quantity, List<CartLine> lineCollection)
        {
            var element = lineCollection.ElementAt(index);
            element.Quantity = quantity;
            element.MedicinalProduct.Price *= quantity;
        }

        public List<string[]> GetCartOfMedicines(List<CartLine> lineCollection)
        {
            List<string[]> strings = new List<string[]>();
            for (int i = 0; i < lineCollection.Count; i++)
            {
                string[] array = new string[]
                {
                    lineCollection[i].MedicinalProduct.Id.ToString(),
                    lineCollection[i].MedicinalProduct.Category,
                    lineCollection[i].MedicinalProduct.Name,
                    lineCollection[i].MedicinalProduct.Brand,
                    lineCollection[i].MedicinalProduct.Price.ToString(),
                    lineCollection[i].MedicinalProduct.Quantity.ToString(),
                    lineCollection[i].Quantity.ToString(),
                };
                strings.Add(array);
            }
            return strings;
        }
    }
}
