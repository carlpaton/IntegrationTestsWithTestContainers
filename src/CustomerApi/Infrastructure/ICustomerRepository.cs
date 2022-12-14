namespace CustomerApi.Infrastructure
{
    public interface ICustomerRepository
    {
        Task SaveAsync(Customer customer);
        List<Customer> SelectAll();
        Customer Select(Guid customerId);
        void Update(Guid customerId, Customer customer);
        void Delete(Guid customerId);
    }
}