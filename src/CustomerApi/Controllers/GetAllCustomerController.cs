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
        var list = new List<Customer>();
        for (int i = 0; i < 5; i++)
        {
            list.Add(new Customer() {
                Id = Guid.NewGuid(),
                Name = $"Customer Name {i}",
            });
        }

        return Ok(list);
    }
}
