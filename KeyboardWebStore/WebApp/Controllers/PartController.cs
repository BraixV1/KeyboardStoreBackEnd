using System.Net;
using System.Security.Claims;
using App.Contracts.BLL;
using App.Domain.Identity;
using App.DTO.v1_0;
using App.DTO.v1_0.Part;
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
[Route("api/v{version:apiVersion}/Part")]
public class PartController : Controller
{
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.Part.Part, App.BLL.DTO.Part> _mapper;

    public PartController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<Part, App.BLL.DTO.Part>(mapper);
    }
    
    /// <summary>
    /// Returns all parts from database
    /// </summary>
    /// <returns>List of parts</returns>
    [HttpGet]
    [Route("AllParts")]
    [ProducesResponseType(typeof(IEnumerable<Part>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<Part>>> GetAllParts()
    {
        var found = (await _bll.Parts.GetAllPartsIncludingAsync()).Select(part => _mapper.Map(part));
        return Ok(found);
    }

    /// <summary>
    /// Returns specific Part in the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific part or not found</returns>
    [HttpGet]
    [Route("GetPart/{id}")]
    [ProducesResponseType(typeof(Part), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Part>> GetPart(Guid id)
    {
        var found = (await _bll.Parts.GetAllPartsIncludingAsync()).FirstOrDefault(p => p.Id == id);

        if (found == null)
            return NotFound(new RestApiErrorResponse
                { Error = $"Part with id: {id} not found", Status = HttpStatusCode.NotFound });
        return Ok(_mapper.Map(found));
    }
    
    
    
    /// <summary>
    /// Add new Part to datbase
    /// </summary>
    /// <returns>added part or error</returns>
    [HttpPost]
    [Route("AddPart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Part), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Part>> AddNewPart([FromBody] Part model)
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
        
        if (string.IsNullOrEmpty(model.PartName) ||
            string.IsNullOrEmpty(model.PartCode) ||
            model.Price == 0 ||
            model.Quantity == null)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newPart = new App.BLL.DTO.Part()
            {
                PartName = model.PartName,
                PartCode = model.PartCode,
                Price = model.Price,
                Quantity = model.Quantity
            };
            var added =_bll.Parts.Add(newPart);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    /// <summary>
    /// Delete Part from database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeletePart/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Keyboard>> DeletePart(Guid id)
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
        var found = await _bll.Parts.FirstOrDefaultAsync(id);
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});;
        await _bll.Parts.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }
    
    /// <summary>
    /// Update Part in database
    /// </summary>
    /// <returns>Keyboard and what happened with it</returns>
    [HttpPatch]
    [Route("UpdatePart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(Part), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<Keyboard>> UpdatePart([FromBody] Part model)
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
        
        var found = await _bll.Parts.FirstOrDefaultAsync(model.Id);
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound});;
        if (model.Id == default ||
            string.IsNullOrEmpty(model.PartName) ||
            model.Price == 0 ||
            string.IsNullOrEmpty(model.PartCode))
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }

        try
        {

            found.PartName = model.PartName;
            found.Price = model.Price;
            found.PartCode = model.PartCode;
            found.Quantity = model.Quantity;


            
            
            
            _bll.Parts.Update(found);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(found));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
}