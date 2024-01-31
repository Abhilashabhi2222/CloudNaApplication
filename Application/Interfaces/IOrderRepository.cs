namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        OrderDetailsResponse GetMostRecentOrder(string email, string customerId);
        //
    }
}
