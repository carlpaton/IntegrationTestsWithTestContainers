using CustomerApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/customer")]
public class CreateCustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] Customer customer)
    {
        _customerRepository.Save(customer);
        return StatusCode(StatusCodes.Status201Created, customer);
    }
}
