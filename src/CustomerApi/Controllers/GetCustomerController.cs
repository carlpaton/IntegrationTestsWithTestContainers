using CustomerApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/customer")]
public class GetCustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("{customerId:guid}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public IActionResult Read(Guid customerId)
    {
        var customer = new Customer
        {
            Id = customerId,
            Name = "Customer Name"
        };
        return Ok(customer);
    }
}
