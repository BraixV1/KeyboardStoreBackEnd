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
[Route("api/v{version:apiVersion}/WarehousePart")]
public class WarehousePartController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.WarehousePart, App.BLL.DTO.WarehousePart> _mapper;
    
    public WarehousePartController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<App.DTO.v1_0.WarehousePart, App.BLL.DTO.WarehousePart>(mapper);
    }

    /// <summary>
    /// Returns all warehouse parts from the database
    /// </summary>
    /// <returns>List of warehouse parts</returns>
    [HttpGet]
    [Route("AllWarehouseParts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(IEnumerable<WarehousePart>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IEnumerable<Warehouse>>> GetAllWarehousesParts()
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
        
        
        var found = (await _bll.WarehouseParts.GetAllWarehousePartsIncludingPartsAsync())
            .Select(warehouse => _mapper.Map(warehouse));
        return Ok(found);
    }

    /// <summary>
    /// Returns specific ware house part
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific warehouse part or not found</returns>
    [HttpGet]
    [Route("GetWarehousePart/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(WarehousePart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<WarehousePart>> GetWarehousePart(Guid id)
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
        var found = (await _bll.WarehouseParts.GetAllWarehousePartsIncludingPartsAsync())
            .Select(warehouse => _mapper.Map(warehouse))
            .FirstOrDefault(warehouse => warehouse!.Id == id);
        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Keyboard with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(found);
        
    }
    
    
    /// <summary>
    /// Add new warehouse part to datbase
    /// </summary>
    /// <returns>added warehouse part or error</returns>
    [HttpPost]
    [Route("AddWarehousePart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(WarehousePart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<WarehousePart>> AddNewWarehousePart([FromBody] WarehousePart model)
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

        if (model.WareHouseId == default ||
            (model.KeyboardId == null && model.PartId == null) ||
            model.Quantity < 1)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newWarehousePart = new App.BLL.DTO.WarehousePart()
            {
                KeyboardId = model.KeyboardId,
                Quantity = model.Quantity,
                PartId = model.PartId,
                WareHouseId = model.WareHouseId
            };
            var added =_bll.WarehouseParts.Add(newWarehousePart);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }


    /// <summary>
    /// Delete warehouse part
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if it managed to delete it</returns>
    [HttpDelete]
    [Route("DeleteWarehousePart/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<bool>> DeleteWarehousePart(Guid id)
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

        var found = await _bll.WarehouseParts.FirstOrDefaultAsync(id);

        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});;
        await _bll.WarehouseParts.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }


    /// <summary>
    /// Update warehouse part
    /// </summary>
    /// <returns>Updated warehouse part</returns>
    [HttpPatch]
    [Route("UpdateWarehousePart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(WarehousePart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Keyboard>> UpdateWarehousePart([FromBody] WarehousePart model)
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
            model.WareHouseId == default ||
            (model.KeyboardId == null && model.PartId == null) || model.Quantity < 0)
        {
            return BadRequest(new RestApiErrorResponse()
                { Error = "One or more fields are empty", Status = HttpStatusCode.BadRequest });
        }

        try
        {

            var found = await _bll.WarehouseParts.FirstOrDefaultAsync(model.Id);

            if (found == null)
                return NotFound(new RestApiErrorResponse()
                    { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound });
            ;

            found.WareHouseId = model.WareHouseId;
            found.KeyboardId = model.KeyboardId;
            found.PartId = model.PartId;
            found.Quantity = model.Quantity;

            _bll.WarehouseParts.Update(found);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(found));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest });
        }

    }
}