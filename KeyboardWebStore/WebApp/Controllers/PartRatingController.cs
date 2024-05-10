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
[Route("api/v{version:apiVersion}/PartRating")]
public class PartRatingController : Controller
{
    
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.PartRating, App.BLL.DTO.PartRating> _mapper;
    
    public PartRatingController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<App.DTO.v1_0.PartRating, App.BLL.DTO.PartRating>(mapper);
    }
    
    
    /// <summary>
    /// Returns all keyboard ratings from the database
    /// </summary>
    /// <returns>List of keyboard ratings</returns>
    [HttpGet]
    [Route("AllPartRatings")]
    [ProducesResponseType(typeof(IEnumerable<Keyboard>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<Keyboard>>> GetAllPartRatings()
    {
        var found = (await _bll.PartRatings.GetAllPartInBuildsIncludingAsync())
            .Select(rating => _mapper.Map(rating));
      
        return Ok(found);
    }
    
    
    
    /// <summary>
    /// Returns specific part rating from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific part rating</returns>
    [HttpGet]
    [Route("GetPartRating/{id}")]
    [ProducesResponseType(typeof(PartRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Keyboard>> GetPartRating(Guid id)
    {
        
        if (id == default) return BadRequest(id);
        var found = (await _bll.PartRatings.GetAllPartInBuildsIncludingAsync()).Select(rating => _mapper.Map(rating)).FirstOrDefault(kb => kb!.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Rating with id: {id} was not found", Status = HttpStatusCode.NotFound});
        return Ok(found);
    }
    
    
        /// <summary>
    /// Returns speicifc user part ratings
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List of part ratings</returns>
    [HttpGet]
    [Route("GetUserPartRatings/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Keyboard>> GetUserPartRatings(Guid id)
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
        var found = (await _bll.PartRatings.GetAllPartInBuildsIncludingAsync()).Select(rating => _mapper.Map(rating)).Where(kb => kb!.User!.Id == id);
        
        return Ok(found);
    }
        
        
        
    /// <summary>
    /// Returns jwt owners part ratings
    /// </summary>
    /// <param name="id"></param>
    /// <returns>list of part ratings</returns>
    [HttpGet]
    [Route("GetUserOwnPartRatings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<PartRating>>> GetUserOwnPartRatings()
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
        
        
        var found = (await _bll.PartRatings.GetAllPartInBuildsIncludingAsync()).Select(rating => _mapper.Map(rating)).Where(kb => kb!.User!.Id == user.Id);
        
        return Ok(found);
    }
    
    
    /// <summary>
    /// Delete part rating from database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeletePartRating/{id}")]
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
        
        var found = await _bll.PartRatings.FirstOrDefaultAsync(id);

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null && found!.UserId != user.Id)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims and/or is not the owner of this comment", Status = HttpStatusCode.NotFound });
        }
        
        
        if (id == default) return BadRequest(new RestApiErrorResponse() { Error = "Id field not fulfilled", Status = HttpStatusCode.BadRequest});;
        
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});;
        await _bll.PartRatings.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }
    
    
        /// <summary>
    /// Add new part rating
    /// </summary>
    /// <returns>added part rating</returns>
    [HttpPost]
    [Route("AddPartRating")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Keyboard>> AddNewPartRating([FromBody] PartRating model)
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
        
        
        if (string.IsNullOrEmpty(model.Comment) ||
            model.PartId == default)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newKeyboardRating = new App.BLL.DTO.PartRating()
            {
                Comment = model.Comment,
                UserId = user.Id,
                PartId = model.PartId,
                Rating = model.Rating,
                PostedAt = DateTime.UtcNow
            };
            var added =_bll.PartRatings.Add(newKeyboardRating);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.InternalServerError});
        }
    }
        
        
     /// <summary>
    /// Update part rating
    /// </summary>
    /// <returns>Updated part rating</returns>
    [HttpPatch]
    [Route("UpdatePartRating")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(PartRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<PartRating>> UpdatePartRating([FromBody] PartRating model)
    {
        var found = await _bll.PartRatings.FirstOrDefaultAsync(model.Id);
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
        
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {model.Id} was not found", Status = HttpStatusCode.NotFound});;

        if (found.UserId != user.Id)
            return Unauthorized(new RestApiErrorResponse()
                { Error = "You are not the owner of this comment", Status = HttpStatusCode.Unauthorized });        
        
        if (model.Id == default ||
            string.IsNullOrEmpty(model.Comment))
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }

        try
        {

            found.Comment = model.Comment;
            found.Rating = model.Rating;
            found.PostedAt = DateTime.UtcNow;
            
            var added =_bll.PartRatings.Update(found);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    
}