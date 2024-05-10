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
[Route("api/v{version:apiVersion}/KeyboardPart")]
public class KeyboardPartController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.KeyboardPart, App.BLL.DTO.KeyboardPart> _mapper;

    public KeyboardPartController(IAppBLL bll, UserManager<AppUser> userManager, IMapper automapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<KeyboardPart, App.BLL.DTO.KeyboardPart>(automapper);
    }
    
    /// <summary>
    /// Returns all keyboard parts from the database
    /// </summary>
    /// <returns>List of keyboard parts</returns>
    [HttpGet]
    [Route("AllKeyboardParts")]
    [ProducesResponseType(typeof(IEnumerable<KeyboardPart>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<KeyboardPartWithKeyboard>>> GetAll()
    {
        var found = (await _bll.KeyboardParts.GetAllKeyboardPartsIncludingAsync())
            .Select(part => _mapper.Map(part));
        return Ok(found);
    }
    
    
    /// <summary>
    /// Returns specific keyboard part field from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific keyboard or not found</returns>
    [HttpGet]
    [Route("GetKeyboardPart/{id}")]
    [ProducesResponseType(typeof(KeyboardPart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<KeyboardPart>> GetKeyboardPart(Guid id)
    {
        if (id == default) return BadRequest(id);
        var found = (await _bll.KeyboardParts.GetAllKeyboardPartsIncludingAsync())
            .Select(e => _mapper.Map(e))
            .FirstOrDefault(x => x!.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Keyboard part with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(found);
    }
    
    
    
    /// <summary>
    /// Add new keyboard part to the database
    /// </summary>
    /// <returns>Keyboard part and what happened to it</returns>
    [HttpPost]
    [Route("AddKeyboardPart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardPart), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<KeyboardPart>> Add([FromBody] KeyboardPart model)
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
        
        if (model.KeyboardId == default ||
            model.PartId == default)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newKeyboardPart = new App.BLL.DTO.KeyboardPart
            {
                KeyboardId = model.KeyboardId,
                PartId = model.PartId
            };
            var added = _bll.KeyboardParts.Add(newKeyboardPart);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    
    
        /// <summary>
    /// Delete Keyboard part from database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeleteKeyboardPart/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<KeyboardPartWithKeyboard>> DeleteKeyboard(Guid id)
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
        
        if (id == default) return BadRequest(false);
        var found = await _bll.KeyboardParts.FirstOrDefaultAsync(id);
        if (found == null) return NotFound(false);
        await _bll.KeyboardParts.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }

    /// <summary>
    /// Update Keyboard part in database
    /// </summary>
    /// <returns>updated keyboard part</returns>
    [HttpPatch]
    [Route("UpdateKeyboardPart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardPartWithKeyboard), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<KeyboardPartWithKeyboard>> Update([FromBody] KeyboardPartWithKeyboard model)
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
        
        
        if (model.Id == default ||
            model.KeyboardId == default ||
            model.PartId == default)
        {
            return BadRequest("One or more fields is empty");
        }

        try
        {
            var newKeyboardPart = new App.BLL.DTO.KeyboardPart
            {
                Id = model.Id,
                KeyboardId = model.KeyboardId,
                PartId = model.PartId
            };

            var found = await _bll.KeyboardParts.FirstOrDefaultAsync(model.Id);

            if (found == null) return NotFound(newKeyboardPart);
            
            var added =_bll.KeyboardParts.Update(newKeyboardPart);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
}