using CoindeskApi.Enitties;
using CoindeskApi.Input;
using CoindeskApi.Interface.Service;
using CoindeskApi.Models.MetaData;
using CoindeskApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CoindeskApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CoindeskController : Controller
    {
        private readonly IStringLocalizer<CoindeskController> _localizer;
        private readonly ICoindeskService _icoindeskService;
        private readonly IStringLocalizer<SharedResource> _sharelocalizer;
        public CoindeskController(ICoindeskService icoindeskService, IStringLocalizer<CoindeskController> localizer)
        {
            _icoindeskService = icoindeskService;
            _localizer = localizer;
        }

        [HttpGet("error")]
        public IActionResult ThrowError()
        {
            throw new InvalidOperationException("This is a test exception.");
        }

        [HttpGet("Regin")]
        public IActionResult Regin()
        {
            return Ok(_localizer["PleaseCurrenyType"]);
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> CallApi()
        {
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var result = await _icoindeskService.CallApi(url);
            return Ok(result);
        }

        [HttpGet("GetAssign")]
        public async Task<IActionResult> GetAssign(string code)
        {
            if (code == null)
            {
                return BadRequest(_localizer["PleaseCurrenyType"]);
            }
            var result = await _icoindeskService.GetAssign(code);
            return Ok(result);
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _icoindeskService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ConindeskInput input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                return BadRequest(new ApiResponse<Coindesk>
                {
                    Success = false,
                    Message = _localizer["ValidationFailed"],
                    Errors = errors
                });
            }

            var result = _icoindeskService.Add(input);
            return Ok(result);
        }

        [HttpPatch]
        public IActionResult Update(ConindeskInput input)
        {
            if (input.code == null)
            {
                return BadRequest(new ApiResponse<Coindesk>
                {
                    Success = false,
                    Message = _localizer["PleaseCurrenyType"]
                });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                return BadRequest(new ApiResponse<Coindesk>
                {
                    Success = false,
                    Message = _localizer["ValidationFailed"],
                    Errors = errors
                });
            }
            var result = _icoindeskService.Update(input);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(string code)
        {
            if (code == null)
            {
                return BadRequest(_localizer["PleaseCurrenyType"]);
            }
            var result = _icoindeskService.Delete(code);
            return Ok(result);
        }

    }

}
