namespace CustomerApi.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _dbContext;

        public CustomerRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(Guid customerId)
        {
            var customer = _dbContext
                .Customers
                .Where(x => x.Id == customerId)
                .FirstOrDefault();

            _dbContext.Remove(customer);
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync(Customer customer)
        {
            _dbContext.Add(customer);
            await _dbContext.SaveChangesAsync();
        }

        public Customer Select(Guid customerId)
        {
            return _dbContext
                .Customers
                .Where(x => x.Id == customerId)
                .FirstOrDefault();
        }

        public List<Customer> SelectAll()
        {
            return _dbContext
                .Customers
                .ToList();
        }

        public void Update(Guid customerId, Customer customer)
        {
            var current = _dbContext
                .Customers
                .Where(x => x.Id == customerId)
                .FirstOrDefault();

            current.Name = customer.Name;

            _dbContext.Update(current);
            _dbContext.SaveChanges();
        }
    }
}
