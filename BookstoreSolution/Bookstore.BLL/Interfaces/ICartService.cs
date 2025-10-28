using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Interfaces
{
    
    public interface ICartService
    {
        
        void AddToCart(int userId, int bookId, int quantity);

       
        void UpdateCartItem(int userId, int bookId, int quantity);

      
        void RemoveCartItem(int userId, int bookId);

        
        IEnumerable<CartItem> GetCartByUser(int userId);
    }
}
