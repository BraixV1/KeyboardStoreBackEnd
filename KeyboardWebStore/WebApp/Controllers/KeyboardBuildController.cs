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
[Route("api/v{version:apiVersion}/KeyboardBuild")]
public class KeyboardBuildController : Controller
{
    
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.KeyboardBuild, App.BLL.DTO.KeyboardBuild> _mapper;
    
    public KeyboardBuildController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<KeyboardBuild, App.BLL.DTO.KeyboardBuild>(mapper);
    }
    
    
    /// <summary>
    /// Returns all keyboard builds from the database
    /// </summary>
    /// <returns>List of keyboard build objects</returns>
    [HttpGet]
    [Route("AllKeyboardBuilds")]
    [ProducesResponseType(typeof(IEnumerable<KeyboardBuild>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<KeyboardBuild>>> GetAllKeyboardBuilds()
    {
        var found = (await _bll.KeyboardBuilds.GetAllKeyboardBuildsIncludingAsync())
            .Select(build => _mapper.Map(build));
      
        return Ok(found);
    }
    
    /// <summary>
    /// Returns specific keyboard build from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific keyboard build or not found</returns>
    [HttpGet]
    [Route("GetKeyboardBuild/{id}")]
    [ProducesResponseType(typeof(KeyboardBuild), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<KeyboardBuild>> GetKeyboardBuild(Guid id)
    {
        
        if (id == default) return BadRequest(id);
        var found = (await _bll.KeyboardBuilds.GetAllKeyboardBuildsIncludingAsync()).FirstOrDefault(kb => kb.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"KeyboardBuild with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(_mapper.Map(found));
    }
    
    
    /// <summary>
    /// Users builds
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns all builds this specific user has created</returns>
    [HttpGet]
    [Route("GetUserKeyboardBuilds")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<KeyboardBuild>>> GetUserKeyboardBuilds()
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
        
        
        var found = (await _bll.KeyboardBuilds
            .GetAllKeyboardBuildsIncludingAsync()).Select(build => _mapper.Map(build))
            .Where(build => build!.AppUserId == user.Id);
        
        return Ok(found);
    }
    
    
    /// <summary>
    /// Make new keyboard build in database
    /// </summary>
    /// <returns>keyboard build that was made</returns>
    [HttpPost]
    [Route("AddKeyboardBuild")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardBuild), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<KeyboardBuild>> AddNewKeyboardBuild([FromBody] KeyboardBuild model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.BuildName))
            {
                return BadRequest(new RestApiErrorResponse
                {
                    Error = "Build needs a name Doesn't it (preferably a mighty one) :D",
                    Status = HttpStatusCode.BadRequest
                });
            }
            
            var userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new RestApiErrorResponse
                {
                    Error = "User does not exist",
                    Status = HttpStatusCode.NotFound
                });
            }
            
            var newKeyboardBuild = new App.BLL.DTO.KeyboardBuild()
            {
                AppUserId = user.Id,
                BuildName = model.BuildName,
                Description = model.Description
            };
            var added = _bll.KeyboardBuilds.Add(newKeyboardBuild);
            await _bll.SaveChangesAsync();
            
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new RestApiErrorResponse
            {
                Error = "Server error occured while processing a request please contact development team",
                Status = HttpStatusCode.InternalServerError
            });
        }
    }
    
    
    /// <summary>
    /// Delete keyboard build from database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeleteKeyboardBuild/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<KeyboardBuild>> DeleteKeyboardBuild(Guid id)
    {
        var userEmailClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        var found = await _bll.KeyboardBuilds.FirstOrDefaultAsync(id);
        
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});
        
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

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null && user.Id != found.Id )
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims", Status = HttpStatusCode.NotFound });
        }
        
        if (id == default) return BadRequest(new RestApiErrorResponse() { Error = "Id field not fulfilled", Status = HttpStatusCode.BadRequest});
        
        await _bll.KeyboardBuilds.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }
    
    
    /// <summary>
    /// Update KeyboardBuild in database
    /// </summary>
    /// <returns>KeyboardBuild and what happened to it</returns>
    [HttpPatch]
    [Route("UpdateKeyboardBuild")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardBuild ), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<KeyboardBuild>> UpdateKeyboardBuild([FromBody] KeyboardBuild model)
    {
        var userEmailClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        if (userEmailClaim == null)
        {
            return NotFound(new RestApiErrorResponse {Error = "JWT does not contain email claim", Status = HttpStatusCode.NotFound});
        }
        
        var found = await _bll.KeyboardBuilds.FirstOrDefaultAsync(model.Id);
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound});;
        
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
            string.IsNullOrEmpty(model.BuildName) ||
            string.IsNullOrEmpty(model.Description))
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }

        try
        {

            found.Description = model.Description;
            found.BuildName = model.BuildName;
            
            var added =_bll.KeyboardBuilds.Update(found);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    
    
    
}