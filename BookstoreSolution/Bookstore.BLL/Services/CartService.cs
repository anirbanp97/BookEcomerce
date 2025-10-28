using Bookstore.BLL.Interfaces;
using Bookstore.DAL.Interfaces;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Services
{
    
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        
        public void AddToCart(int userId, int bookId, int quantity)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            if (bookId <= 0) throw new ArgumentException("Invalid book id.");
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

            _cartRepository.AddToCart(userId, bookId, quantity);
        }

       
        public void UpdateCartItem(int userId, int bookId, int quantity)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            if (bookId <= 0) throw new ArgumentException("Invalid book id.");

           
            if (quantity <= 0)
                _cartRepository.RemoveCartItem(userId, bookId);
            else
                _cartRepository.UpdateCartItem(userId, bookId, quantity);
        }

        
        public void RemoveCartItem(int userId, int bookId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            if (bookId <= 0) throw new ArgumentException("Invalid book id.");

            _cartRepository.RemoveCartItem(userId, bookId);
        }

        
        public IEnumerable<CartItem> GetCartByUser(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            return _cartRepository.GetCartByUser(userId);
        }
    }
}
