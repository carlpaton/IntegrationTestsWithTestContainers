namespace CustomerApi.Infrastructure;

public interface ICustomerRepository
{
    Guid Create(Customer customer);
    Customer Read(Guid customerId);
    void Update(Customer customer);
    void Delete(Guid customerId);
}
