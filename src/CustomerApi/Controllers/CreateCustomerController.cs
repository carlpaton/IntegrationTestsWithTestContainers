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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        try
        {

            await _customerRepository.SaveAsync(customer);

            if (customer.Id == Guid.Empty ||
                string.IsNullOrEmpty(customer.Name))
            {
                return BadRequest();
            }

            return CreatedAtAction("Create", new { id = customer.Id }, customer);
        }
        catch (Exception ex) 
        { 
            return Ok(ex);
        }
    }
}
