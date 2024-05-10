using System.Net;
using System.Security.Claims;
using App.Contracts.BLL;
using App.Domain.Identity;
using App.DTO.v1_0;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/Keyboard")]

public class KeyboardController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;

    public KeyboardController(IAppBLL bll, UserManager<AppUser> userManager)
    {
        _bll = bll;
        _userManager = userManager;
    }
    

    /// <summary>
    /// Returns all keyboard in the database
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("AllKeyboards")]
    [ProducesResponseType(typeof(IEnumerable<Keyboard>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<Keyboard>>> GetAllKeyboards()
    {
        var found = await _bll.Keyboards.GetAllKeyboardIncludingAsync();
      
        return Ok(found);
    }
    
    

    /// <summary>
    /// Returns specific keyboard in the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific keyboard or not found</returns>
    [HttpGet]
    [Route("GetKeyboard/{id}")]
    [ProducesResponseType(typeof(Keyboard), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Keyboard>> GetKeyboard(Guid id)
    {
        if (id == default) return BadRequest(id);
        var found = (await _bll.Keyboards.GetAllKeyboardIncludingAsync()).FirstOrDefault(kb => kb.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Keyboard with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(found);
    }


    /// <summary>
    /// Add new keyboard to database
    /// </summary>
    /// <returns>List of Keyboards</returns>
    [HttpPost]
    [Route("AddKeyboard")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Keyboard), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Keyboard>> AddNewKeyboard([FromBody] Keyboard model)
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
        
        
        if (string.IsNullOrEmpty(model.Name) ||
            string.IsNullOrEmpty(model.Description) ||
            model.Price == 0 ||
            model.Price < 0 ||
            string.IsNullOrEmpty(model.ItemCode))
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newKeyboard = new App.BLL.DTO.Keyboard()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ItemCode = model.ItemCode
            };
            _bll.Keyboards.Add(newKeyboard);
            await _bll.SaveChangesAsync();
            return Ok(newKeyboard);
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }

    /// <summary>
    /// Delete Keyboard from database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeleteKeyboard/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Keyboard>> DeleteKeyboard(Guid id)
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
        
        
        if (id == default) return BadRequest(new RestApiErrorResponse() { Error = "Id field not fulfilled", Status = HttpStatusCode.BadRequest});;
        var found = await _bll.Keyboards.FirstOrDefaultAsync(id);
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});;
        await _bll.Keyboards.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }

    /// <summary>
    /// Update Keyboard in database
    /// </summary>
    /// <returns>Keyboard and what happened to it</returns>
    [HttpPatch]
    [Route("UpdateKeyboard")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Keyboard), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Keyboard>> UpdatePart([FromBody] Keyboard model)
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
            string.IsNullOrEmpty(model.Name) ||
            string.IsNullOrEmpty(model.Description) ||
            model.Price == 0 ||
            string.IsNullOrEmpty(model.ItemCode))
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }

        try
        {
            var newKeyboard = new App.BLL.DTO.Keyboard()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ItemCode = model.ItemCode
            };

            var found = await _bll.Keyboards.FirstOrDefaultAsync(model.Id);

            if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound});;
            
            _bll.Keyboards.Update(newKeyboard);
            await _bll.SaveChangesAsync();
            return Ok(newKeyboard);
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
}