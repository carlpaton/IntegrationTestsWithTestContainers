using CustomerApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/customer")]
public class UpdateCustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpPut("{customerId:guid}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    public IActionResult Update(Guid customerId, [FromBody] Customer customer)
    {
        _customerRepository.Update(customerId, customer);

        var response = new Customer()
        {
            Id = customerId,
            Name = customer.Name
        };

        return Ok(response);
    }
}
