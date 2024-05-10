using System.Collections;
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
[Route("api/v{version:apiVersion}/KeyboardRating")]
public class KeyboardRatingController : Controller
{
    
    private readonly IAppBLL _bll;
    private readonly UserManager<AppUser> _userManager;
    private readonly PublicDTOBllMapper<App.DTO.v1_0.KeyboardRating, App.BLL.DTO.KeyboardRating> _mapper;
    
    public KeyboardRatingController(IAppBLL bll, UserManager<AppUser> userManager, IMapper mapper)
    {
        _bll = bll;
        _userManager = userManager;
        _mapper = new PublicDTOBllMapper<KeyboardRating, App.BLL.DTO.KeyboardRating>(mapper);
    }
    
    
    /// <summary>
    /// Returns all keyboard ratings from the database
    /// </summary>
    /// <returns>list of keyboard ratings</returns>
    [HttpGet]
    [Route("AllKeyboardRatings")]
    [ProducesResponseType(typeof(IEnumerable<KeyboardRating>), (int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<KeyboardRating>>> GetAllKeyboards()
    {
        var found = (await _bll.KeyboardRatings.GetAllRatingsIncludingAsync())
            .Select(rating => _mapper.Map(rating));
      
        return Ok(found);
    }
    
    
    
    /// <summary>
    /// Returns specific keyboard rating from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specific keyboard or not found</returns>
    [HttpGet]
    [Route("GetKeyboardRating/{id}")]
    [ProducesResponseType(typeof(KeyboardRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<KeyboardRating>> GetKeyboardRating(Guid id)
    {
        if (id == default) return BadRequest(id);
        var found = (await _bll.KeyboardRatings.GetAllRatingsIncludingAsync()).FirstOrDefault(kb => kb.Id == id);

        if (found == null) return NotFound(new RestApiErrorResponse {Error = $"Rating with id: {id} was not found", Status = HttpStatusCode.NotFound});
        return Ok(_mapper.Map(found));
    }
    
    
        /// <summary>
    /// Returns speicific user keyboard ratings
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List of keyboard ratings</returns>
    [HttpGet]
    [Route("GetUserKeyboardRatings/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<KeyboardRating>>> GetUserKeyboardRatings(Guid id)
    {
        if (id == default) return BadRequest(id);
        var found = (await _bll.KeyboardRatings.GetAllRatingsIncludingAsync())
            .Where(kb => kb.User!.Id == id)
            .Select(rating => _mapper.Map(rating));
        
        return Ok(found);
    }
        
        
        
    /// <summary>
    /// Returns jwt owners keyboard ratings
    /// </summary>
    /// <param name="id"></param>
    /// <returns>List of keyboard ratings</returns>
    [HttpGet]
    [Route("GetUserOwnKeyboardRatings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<IEnumerable<KeyboardRating>>> GetUserOwnKeyboardRatings()
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
        
        
        var found = (await _bll.KeyboardRatings.GetAllRatingsIncludingAsync())
            .Where(kb => kb.User!.Id == user.Id)
            .Select(rating => _mapper.Map(rating));
        
        return Ok(found);
    }
    
    
    /// <summary>
    /// Delete keyboard rating from the database
    /// </summary>
    /// <returns>true or false depending on if it did its job</returns>
    [HttpDelete]
    [Route("DeletekeyboardRating/{id}")]
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
        
        var found = await _bll.KeyboardRatings.FirstOrDefaultAsync(id);

        if (rolesClaim.FirstOrDefault(claim => claim.Value == "Admin") == null && found!.UserId != user.Id)
        {
            return NotFound(new RestApiErrorResponse { Error = "JWT does not have required claims and/or is not the owner of this comment", Status = HttpStatusCode.NotFound });
        }
        
        
        if (id == default) return BadRequest(new RestApiErrorResponse() { Error = "Id field not fulfilled", Status = HttpStatusCode.BadRequest});;
        
        if (found == null) return NotFound(new RestApiErrorResponse() { Error = $"item with id: {id} was not found", Status = HttpStatusCode.NotFound});;
        await _bll.KeyboardRatings.RemoveAsync(found);
        await _bll.SaveChangesAsync();
        return Ok(true);
    }
    
    
    /// <summary>
    /// Add new rating the keyboard
    /// </summary>
    /// <returns>keyboard rating</returns>
    [HttpPost]
    [Route("AddKeyboardRating")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]

    public async Task<ActionResult<Keyboard>> AddNewKeyboardRating([FromBody] KeyboardRating model)
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
            model.KeyboardId == default || model.Rating < 0)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }
        try
        {
            var newKeyboardRating = new App.BLL.DTO.KeyboardRating()
            {
                Comment = model.Comment,
                UserId = user.Id,
                KeyboardId = model.KeyboardId,
                Rating = model.Rating,
                postedAtDt = DateTime.UtcNow
            };
            var added =_bll.KeyboardRatings.Add(newKeyboardRating);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.InternalServerError});
        }
    }
        
        
     /// <summary>
    /// Update Keyboard rating in the database
    /// </summary>
    /// <returns>Updated keyboard rating</returns>
    [HttpPatch]
    [Route("UpdateKeyboardRating")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(KeyboardRating), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<KeyboardRating>> UpdateKeyboardRating([FromBody] KeyboardRating model)
    {
        var found = await _bll.KeyboardRatings.FirstOrDefaultAsync(model.Id);
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
            string.IsNullOrEmpty(model.Comment) || model.Rating < 0)
        {
            return BadRequest(new RestApiErrorResponse() {Error = "One or more fields is empty", Status = HttpStatusCode.BadRequest});
        }

        try
        {
            found.Comment = model.Comment;
            found.Rating = model.Rating;
            found.postedAtDt = DateTime.UtcNow;
            
            var added =_bll.KeyboardRatings.Update(found);
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(added));
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse() { Error = e.Message, Status = HttpStatusCode.BadRequest});
        }
    }
    
    
    
}