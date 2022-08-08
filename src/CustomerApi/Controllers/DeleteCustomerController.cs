using CustomerApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/customer")]
public class DeleteCustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpDelete("{customerId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(Guid customerId)
    {
        _customerRepository.Delete(customerId);
        return NoContent();
    }
}
