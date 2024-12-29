using CoindeskApi.Input;
using CoindeskApi.Interface.Service;
using CoindeskApi.Models.MetaData;
using CoindeskApi.Resources;
using CoindeskApi.ViewModels;
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
        public CoindeskController(ICoindeskService icoindeskService, IStringLocalizer<CoindeskController> localizer, IStringLocalizer<SharedResource> sharelocalizer)
        {
            _icoindeskService = icoindeskService;
            _localizer = localizer;
            _sharelocalizer = sharelocalizer;
        }       


        [HttpGet("fetch")]
        public async Task<IActionResult> CallApi()
        {
            var requiredFieldMessage = _sharelocalizer["RequiredField"];
            Console.WriteLine($"RequiredField Message: {requiredFieldMessage}");

            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var result= await _icoindeskService.CallApi(url);
            return Ok(result);
        }

        [HttpGet("GetAssign")]
        public async Task<IActionResult> GetAssign(string code)
        {
            if (code == null)
            {
                return BadRequest(_localizer["PleaseCurrenyType"]);
            }
            var result= await  _icoindeskService.GetAssign(code);
            return Ok(result);
        }
      
      
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            //throw new Exception();
            var result = await _icoindeskService.GetAll();
            return Ok(result);
        }
      
        [HttpPost]
        public IActionResult Add(ConindeskInput input)
        {
            if (input.code == null)
            {
                return BadRequest(new ResultVM<Coindesk>
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
                return BadRequest(new ResultVM<Coindesk>
                {
                    Success = false,
                    Message = _localizer["ValidationFailed"],
                    Errors = errors
                });
            }
            var result =  _icoindeskService.Add(input);
            return Ok(result);
        }
      
        [HttpPatch]
        public IActionResult Update(ConindeskInput input)
        {
            if (input.code==null)
            {
                return BadRequest(new ResultVM<Coindesk>
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
                return BadRequest(new ResultVM<Coindesk>
                {
                    Success = false,
                    Message = _localizer["ValidationFailed"],
                    Errors = errors
                });
            }
            var result =  _icoindeskService.Update(input);
            return Ok(result);
        }
      
        [HttpDelete]
        public IActionResult Delete(string code)
        {
            if (code == null)
            {
                return BadRequest(_localizer["PleaseCurrenyType"]);
            }
            var result =  _icoindeskService.Delete(code);
            return Ok(result);
        }

    }

}
