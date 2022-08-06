using System.Data;

namespace CustomerApi.Infrastructure;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnection _connection;

    public CustomerRepository(IDbConnection dbConnection)
    {
        _connection = dbConnection;
    }

    public Guid Create(Customer customer)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Customer Read(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public void Update(Customer customer)
    {
        throw new NotImplementedException();
    }
}
