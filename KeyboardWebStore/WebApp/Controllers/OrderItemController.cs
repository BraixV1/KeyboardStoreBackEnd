using System.Net;
using System.Security.Claims;
using App.Contracts.BLL;
using App.Domain.Identity;
using App.DTO.v1_0;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;

namespace WebApp.Controllers;


[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/OrderItem")]
public class OrderItemController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.OrderItem, App.BLL.DTO.OrderItem> _mapper;
    
    public OrderItemController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<OrderItem, App.BLL.DTO.OrderItem>(mapper);
    }
    
    
    /// <summary>
    /// Returns all order items from the database
    /// </summary>
    /// <returns>List of order items</returns>
    [HttpGet]
    [Route("AllOrderItems")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(IEnumerable<OrderItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<Keyboard>>> GetAllOrderItems()
    {
        var userEmailClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        if (userEmailClaim == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "JWT does not contain email claim", Status = HttpStatusCode.NotFound});
        }
    
        var userEmail = userEmailClaim.Value;
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "User does not exist that was inside JWT", Status = HttpStatusCode.NotFound});
        }
        
        var rolesClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims", Status = HttpStatusCode.NotFound });
        }

        var found = (await _bll.OrderItems.GetAllOrderItemsIncludingAsync()).Select(item => _mapper.Map(item));
      
        return Ok(found);
    }
    
    
    /// <summary>
    /// Returns specific order item from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific order item or not found</returns>
    [HttpGet]
    [Route("GetOrderItem/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<App.DAL.DTO.OrderItem>> GetOrderItem(Guid id)
    {
        var userEmailClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        if (userEmailClaim == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "JWT does not contain email claim", Status = HttpStatusCode.NotFound});
        }
    
        var userEmail = userEmailClaim.Value;
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "User does not exist that was inside JWT", Status = HttpStatusCode.NotFound});
        }
        
        var rolesClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims", Status = HttpStatusCode.NotFound });
        }
        
        if (id == default) return BadRequest(id);
        var found = (await _bll.OrderItems.GetAllOrderItemsIncludingAsync()).FirstOrDefault(kb => kb.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Keyboard with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(_mapper.Map(found));
    }


    /// <summary>
    /// Add new order item into the database
    /// </summary>
    /// <returns>Order item that was made</returns>
    [HttpPost]
    [Route("AddOrderItem")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.Conflict)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<OrderItem>> AddNewOrderOrderItem([FromBody] OrderItem model)
    {
        try
        {
            var userEmailClaim =
                HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            
            if (userEmailClaim == null)
            {
                return NotFound(new RestApiErrorResponse
                    { Error = "JWT does not contain email claim", Status = HttpStatusCode.NotFound });
            }

            var userEmail = userEmailClaim.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new RestApiErrorResponse
                    { Error = "User does not exist that was inside JWT", Status = HttpStatusCode.NotFound });
            }

            var rolesClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            if (rolesClaim.Count == 0)
            {
                return NotFound(new RestApiErrorResponse
                    { Error = "JWT does not contain any role claims", Status = HttpStatusCode.NotFound });
            }

            if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null)
            {
                return NotFound(new RestApiErrorResponse
                    { Error = "JWT does not have required claims", Status = HttpStatusCode.NotFound });
            }

            if ((model.KeyboardId == null && model.PartId == null) ||
                model.Quantity < 1)
            {
                return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
            
            }
            
            
            // Input Validation
            if (model.Quantity < 1)
            {
                return BadRequest(new RestApiErrorResponse
                {
                    Error = "Quantity must be greater than or equal to 1",
                    Status = HttpStatusCode.BadRequest
                });
            }
            
            if (user == null)
            {
                return NotFound(new RestApiErrorResponse
                {
                    Error = "User does not exist",
                    Status = HttpStatusCode.NotFound
                });
            }

            var price = 0D;
            if (model.KeyboardId != null)
            {
                var keyboard = await _bll.Keyboards.FirstOrDefaultAsync(model.KeyboardId.Value);

                if (keyboard != null) price += keyboard.Price * model.Quantity;
            }
            
            if (model.PartId != null)
            {
                var part = await _bll.Parts.FirstOrDefaultAsync(model.PartId.Value);

                if (part != null) price += part.Price * model.Quantity;
            }
            

            var added =_bll.OrderItems.Add(new App.BLL.DTO.OrderItem()
            {
                AddedToBasket = DateTime.UtcNow,
                KeyboardId = model.KeyboardId,
                PartId = model.PartId,
                OrderId = model.OrderId,
                Price = price,
                Quantity = model.Quantity,
            });
            await _bll.SaveChangesAsync();
            

            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError, new RestApiErrorResponse
            {
                Error = "An error occurred while processing the request please contact development team",
                Status = HttpStatusCode.InternalServerError
            });
        }
    }
    
    
    
    
    /// <summary>
    /// Update Order item in the database
    /// </summary>
    /// <returns>Updated order item</returns>
    [HttpPatch]
    [Route("UpdateOrderItem")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<OrderItem>> UpdateOrderItem([FromBody] OrderItem model)
    {
        var userEmailClaim =
            HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        var found = await _bll.OrderItems.FirstOrDefaultAsync(model.Id);

        if (found == null)
        {
            return NotFound(new RestApiErrorResponse()
                { Error = $"Item with id: {model.Id} does not exist", Status = HttpStatusCode.NotFound });
        }
        if (userEmailClaim == null)
        {
            return NotFound(new RestApiErrorResponse
                { Error = "JWT does not contain email claim", Status = HttpStatusCode.NotFound });
        }

        var userEmail = userEmailClaim.Value;
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse
                { Error = "User does not exist that was inside JWT", Status = HttpStatusCode.NotFound });
        }

        var rolesClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        if (rolesClaim.Count == 0)
        {
            return NotFound(new RestApiErrorResponse
                { Error = "JWT does not contain any role claims", Status = HttpStatusCode.NotFound });
        }

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null)
        {
            return NotFound(new RestApiErrorResponse
                { Error = "JWT does not have required claims", Status = HttpStatusCode.NotFound });
        }

        if (model.Id == default ||
            (model.KeyboardId == null && model.PartId == null) ||
            model.Quantity < 1)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
            
        }
        var price = 0D;
        if (model.KeyboardId != null)
        {
            var keyboard = await _bll.Keyboards.FirstOrDefaultAsync(model.KeyboardId.Value);

            if (keyboard != null) price += keyboard.Price * model.Quantity;
        }
            
        if (model.PartId != null)
        {
            var part = await _bll.Parts.FirstOrDefaultAsync(model.PartId.Value);

            if (part != null) price += part.Price * model.Quantity;
        }

        try
        {
            found.KeyboardId = model.KeyboardId;
            found.PartId = model.PartId;
            found.Quantity = model.Quantity;
            found.AddedToBasket = DateTime.UtcNow;
            found.Price = price;
            
            var added =_bll.OrderItems.Update(found);
            await _bll.SaveChangesAsync();

            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.InternalServerError});
        }
    }
    
    
    /// <summary>
    /// Delete Order item from database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeleteOrderItem/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<OrderItem>> DeleteOrderItem(Guid id)
    {
        var userEmailClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        if (userEmailClaim == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "JWT does not contain email claim", Status = HttpStatusCode.NotFound});
        }
    
        var userEmail = userEmailClaim.Value;
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "User does not exist that was inside JWT", Status = HttpStatusCode.NotFound});
        }
        
        var rolesClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        if (rolesClaim.Count == 0)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not contain any role claims", Status = HttpStatusCode.NotFound });
        }

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims", Status = HttpStatusCode.NotFound });
        }
        
        
        if (id == default) return BadRequest(new RestApiErrorResponse() { Error = "Id field not fulfilled", Status = HttpStatusCode.BadRequest});
        var found = await _bll.OrderItems.FirstOrDefaultAsync(id);
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});
        await _bll.OrderItems.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }
}