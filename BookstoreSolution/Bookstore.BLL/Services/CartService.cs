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
    /// <summary>
    /// Business logic layer for all Cart operations.
    /// ADO.NET ভিত্তিক repository ব্যবহার করছে।
    /// </summary>
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        /// <summary>
        /// User-এর cart-এ একটি বই যোগ করবে।
        /// </summary>
        public void AddToCart(int userId, int bookId, int quantity)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            if (bookId <= 0) throw new ArgumentException("Invalid book id.");
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

            _cartRepository.AddToCart(userId, bookId, quantity);
        }

        /// <summary>
        /// Cart item এর quantity আপডেট করবে। 
        /// Quantity = 0 বা কম হলে আইটেম রিমুভ করবে।
        /// </summary>
        public void UpdateCartItem(int userId, int bookId, int quantity)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            if (bookId <= 0) throw new ArgumentException("Invalid book id.");

            // Business rule: 0 quantity হলে remove করা হবে
            if (quantity <= 0)
                _cartRepository.RemoveCartItem(userId, bookId);
            else
                _cartRepository.UpdateCartItem(userId, bookId, quantity);
        }

        /// <summary>
        /// Cart থেকে নির্দিষ্ট বই রিমুভ করবে।
        /// </summary>
        public void RemoveCartItem(int userId, int bookId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            if (bookId <= 0) throw new ArgumentException("Invalid book id.");

            _cartRepository.RemoveCartItem(userId, bookId);
        }

        /// <summary>
        /// User এর পুরো cart (books, quantity, price সহ) রিটার্ন করবে।
        /// </summary>
        public IEnumerable<CartItem> GetCartByUser(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user id.");
            return _cartRepository.GetCartByUser(userId);
        }
    }
}
