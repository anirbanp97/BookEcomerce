namespace Bookstore.Web.ViewModels
{
    public class OrderItemViewModel
    {
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(x => x.LineTotal);
    }
}
