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
[Route("api/v{version:apiVersion}/PartInBuild")]
public class PartInBuildController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.PartInBuild, App.BLL.DTO.PartInBuild> _mapper;

    public PartInBuildController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<PartInBuild, App.BLL.DTO.PartInBuild>(mapper);
    }
    
    /// <summary>
    /// Returns all part in builds from database
    /// </summary>
    /// <returns>List of keyboard parts</returns>
    [HttpGet]
    [Route("AllBuildParts")]
    [ProducesResponseType(typeof(IEnumerable<PartInBuild>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<PartInBuild>>> GetAll()
    {
        var found = (await _bll.PartInBuilds.GetAllPartInBuildsIncludingAsync())
            .Select(build => _mapper.Map(build));
        return Ok(found);
    }
    
    
    /// <summary>
    /// Returns specific build part from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific build part or not found</returns>
    [HttpGet]
    [Route("GetBuildPart/{id}")]
    [ProducesResponseType(typeof(PartInBuild), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<PartInBuild>> GetBuildPart(Guid id)
    {
        if (id == default) return BadRequest(id);
        var found = (await _bll.PartInBuilds.GetAllPartInBuildsIncludingAsync()).FirstOrDefault(x => x.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Keyboard part with id: {id} not found", Status = HttpStatusCode.NotFound});
        return Ok(_mapper.Map(found));
    }
    
    
    
    /// <summary>
    /// Add new part into the build
    /// </summary>
    /// <returns>part in build</returns>
    [HttpPost]
    [Route("AddBuildPart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartInBuild), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<PartInBuild>> Add([FromBody] PartInBuild model)
    {
        if (model.PartId == default ||
            model.KeyboardBuildId == default)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        
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

        var build = await _bll.KeyboardBuilds.FirstOrDefaultAsync(model.KeyboardBuildId);

        if (build == null)
        {
            return NotFound(new RestApiErrorResponse() { Error = "Build was not found", Status = HttpStatusCode.NotFound});
        }

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null && build.AppUserId != user.Id)
        {
            return Conflict(new RestApiErrorResponse { Error = "JWT does not have required claims and/or is not the owner of the build", Status = HttpStatusCode.NotFound });
        }
        
        if (model.PartId == default ||
            model.KeyboardBuildId == default)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var partInBuild = new App.BLL.DTO.PartInBuild()
            {
                KeyboardBuildId = model.KeyboardBuildId,
                PartId = model.PartId
            };
            var added =_bll.PartInBuilds.Add(partInBuild);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    
    
    /// <summary>
    /// Delete Part from build from the database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeletePartInBuild/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<PartInBuild>> Delete(Guid id)
    {
        var found = (await _bll.PartInBuilds.GetAllPartInBuildsIncludingAsync()).FirstOrDefault(build => build.Id == id);
        if (found == null) return NotFound(false);
        
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


        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null && found.KeyboardBuild!.AppUserId != user.Id)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims and/or is not the owner of the build", Status = HttpStatusCode.NotFound });
        }
        
        if (id == default) return BadRequest(false);
        await _bll.PartInBuilds.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }

    /// <summary>
    /// Update build part
    /// </summary>
    /// <returns>build part and what happened with it</returns>
    [HttpPatch]
    [Route("UpdatePartInBuild")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartInBuild), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<PartInBuild>> Update([FromBody] PartInBuild model)
    {
        var found = (await _bll.PartInBuilds.GetAllPartInBuildsIncludingAsync()).FirstOrDefault(build =>
            build.Id == model.Id); 
        var userEmailClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound});;

        
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
        
        var build = await _bll.KeyboardBuilds.FirstOrDefaultAsync(model.KeyboardBuildId);
        if (build == null)
        {
            return NotFound(new RestApiErrorResponse() { Error = "Build was not found", Status = HttpStatusCode.NotFound});
        }

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null || build.AppUserId != user.Id)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims and/or is not the owner of the build", Status = HttpStatusCode.NotFound });
        }
        
        
        if (model.Id == default ||
            model.KeyboardBuildId == default ||
            model.PartId == default)
        {
            return BadRequest("One or more fields is empty");
        }

        try
        {
            var newBuildPart = new App.BLL.DTO.PartInBuild()
            {
                Id = model.Id,
                KeyboardBuildId = model.KeyboardBuildId,
                PartId = model.PartId
            };
            
            var added =_bll.PartInBuilds.Update(newBuildPart);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
}