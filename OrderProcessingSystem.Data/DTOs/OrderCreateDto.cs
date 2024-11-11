namespace OrderProcessingSystem.API.DTOs
{
    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
