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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApp.Helpers;

namespace WebApp.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/PartCategory")]
public class PartCategoryController: Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.PartCategory, App.BLL.DTO.PartCategory> _mapper;


    public PartCategoryController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<PartCategory, App.BLL.DTO.PartCategory>(mapper);
    }


    /// <summary>
    /// Returns all PartCategories from the database
    /// </summary>
    /// <returns>List of part categories</returns>
    [HttpGet]
    [Route("AllPartCategories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(IEnumerable<PartCategory>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IEnumerable<PartCategory>>> GetAllPartCategories()
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

        var found = (await _bll.PartCategories.GetAllPartInBuildsIncludingAsync())
            .Select(category => _mapper.Map(category));

        return Ok(found);
    }


    /// <summary>
    /// Returns specific part category from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific part category or not found</returns>
    [HttpGet]
    [Route("GetPartCategory/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartCategory), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<PartCategory>> GetPartCategory(Guid id)
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

        
        
        var found = (await _bll.PartCategories.GetAllPartInBuildsIncludingAsync()).FirstOrDefault(category => category.Id == id);

        if (found == null)
        {
            return NotFound(new RestApiErrorResponse {Error = $"Part category with id: {id} was not found", Status = HttpStatusCode.NotFound});
        }

        return Ok(_mapper.Map(found));
    }


    /// <summary>
    /// Add new part category into the database
    /// </summary>
    /// <returns>added part category</returns>
    [HttpPost]
    [Route("AddPartCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.Conflict)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<PartCategory>> AddNewPartCategory([FromBody] PartCategory model)
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

            if ((model.KeyboardId == default && model.PartId == default) ||
                model.CategoryId == default)
            {
                return BadRequest(new RestApiErrorResponse()
                    { Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest });
            }

            var newPartCategory = new App.BLL.DTO.PartCategory()
            {
                PartId = model.PartId,
                KeyboardId = model.KeyboardId,
                CategoryId = model.CategoryId,
            };

            var added = _bll.PartCategories.Add(newPartCategory);

            await _bll.SaveChangesAsync();

            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new RestApiErrorResponse()
            {
                Error = "Something went wrong contact software developer", Status = HttpStatusCode.InternalServerError
            });
        }
    }


    /// <summary>
    /// Delete part category from the database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeletePartCategory/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<bool>> DeletePartCategory(Guid id)
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

        var found = await _bll.PartCategories.FirstOrDefaultAsync(id);
        
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});

        await _bll.PartCategories.RemoveAsync(found);

        await _bll.SaveChangesAsync();

        return Ok(true);
    }

    
    /// <summary>
    /// Update part category in database
    /// </summary>
    /// <returns>Update part category</returns>
    [HttpPatch]
    [Route("UpdatePartCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartCategory), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<PartCategory>> UpdatePartCategory([FromBody] PartCategory model)
    {
        try
        {
            if (model.Id == default || (model.KeyboardId == default && model.PartId == default) ||
                model.CategoryId == default)
            {
                return (BadRequest(new RestApiErrorResponse()
                    { Error = "One or more fields are empty", Status = HttpStatusCode.BadRequest }));
            }

            var found = await _bll.PartCategories.FirstOrDefaultAsync(model.Id);
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

            if (found == null)
            {
                return NotFound(new RestApiErrorResponse()
                    { Error = $"Part category with id: {model.Id} was not found", Status = HttpStatusCode.NotFound });
            }

            found.KeyboardId = model.KeyboardId;
            found.CategoryId = model.CategoryId;
            found.PartId = model.PartId;

            _bll.PartCategories.Update(found);

            await _bll.SaveChangesAsync();

            return Ok(_mapper.Map(found));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    
    
}