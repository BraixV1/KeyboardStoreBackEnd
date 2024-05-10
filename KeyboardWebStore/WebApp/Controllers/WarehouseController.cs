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
[Route("api/v{version:apiVersion}/Warehouse")]
public class WarehouseController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.Warehouse, App.BLL.DTO.Warehouse> _mapper;
    
    public WarehouseController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Warehouse, App.BLL.DTO.Warehouse>(mapper);
    }

    /// <summary>
    /// Returns all warehouses from the database
    /// </summary>
    /// <returns>List of warehouses</returns>
    [HttpGet]
    [Route("AllWarehouses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(IEnumerable<Warehouse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IEnumerable<Warehouse>>> GetAllWarehouses()
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
        
        
        var found = (await _bll.Warehouses.GetAllWarehousesIncludingAsync())
            .Select(warehouse => _mapper.Map(warehouse));
        return Ok(found);
    }

    /// <summary>
    /// Returns specific warehouse from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific warehouse or not found</returns>
    [HttpGet]
    [Route("GetWarehouse/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Warehouse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Warehouse>> GetWarehouse(Guid id)
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
        
        
        if (id == default) return BadRequest(new RestApiErrorResponse() {Error = "Id field is empty or does not meet to standards", Status = HttpStatusCode.BadRequest});
        var found = (await _bll.Warehouses.GetAllWarehousesIncludingAsync())
            .Select(warehouse => _mapper.Map(warehouse))
            .FirstOrDefault(warehouse => warehouse!.Id == id);
        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Keyboard with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(found);
        
    }
    
    
    /// <summary>
    /// Add new Warehouse to datbase
    /// </summary>
    /// <returns>added warehouse or error</returns>
    [HttpPost]
    [Route("AddWarehouse")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Warehouse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Warehouse>> AddNewWarehouse([FromBody] Warehouse model)
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
        
        if (string.IsNullOrEmpty(model.WarehouseName))
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newWarehouse = new App.BLL.DTO.Warehouse()
            {
                WarehouseName = model.WarehouseName
            };
            var added =_bll.Warehouses.Add(newWarehouse);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }


    /// <summary>
    /// Delete warehouse from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true if it did it</returns>
    [HttpDelete]
    [Route("DeleteWarehouse/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<bool>> DeleteWarehouse(Guid id)
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
        
        if (id == default) return BadRequest(new RestApiErrorResponse() {Error = "Id field is empty or does not meet to standards", Status = HttpStatusCode.BadRequest});

        var found = await _bll.Warehouses.FirstOrDefaultAsync(id);

        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});;
        await _bll.Warehouses.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }


    /// <summary>
    /// Update warehouse
    /// </summary>
    /// <returns>updated warehouse</returns>
    [HttpPatch]
    [Route("UpdateWarehouse")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Warehouse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Keyboard>> UpdateWarehouse([FromBody] Warehouse model)
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


        if (model.Id == default ||
            string.IsNullOrEmpty(model.WarehouseName))
        {
            return BadRequest(new RestApiErrorResponse()
                { Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest });
        }

        try
        {

            var found = await _bll.Warehouses.FirstOrDefaultAsync(model.Id);

            if (found == null)
                return NotFound(new RestApiErrorResponse()
                    { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound });
            ;

            found.WarehouseName = model.WarehouseName;

            _bll.Warehouses.Update(found);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(found));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest });
        }

    }
}