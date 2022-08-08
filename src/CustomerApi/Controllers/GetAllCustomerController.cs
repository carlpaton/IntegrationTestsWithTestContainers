using CustomerApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/customer")]
public class GetAllCustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public GetAllCustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Customer>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var customers = _customerRepository.SelectAll();
        return Ok(customers);
    }
}
