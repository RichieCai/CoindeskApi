using CoindeskApi.Controllers;
using CoindeskApi.Enitties;
using CoindeskApi.Models.MetaData;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;


namespace CoindeskApi.Input
{
    public class ConindeskInput
    {
        [Display(Name = "代碼")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "RequiredField")]
        [StringLength(10, ErrorMessageResourceType =  typeof(SharedResource), ErrorMessageResourceName = "StringLengthExceeded")]
        //[Required(ErrorMessage = "該欄位為必填")]
        //[StringLength(10, ErrorMessage = "code最多輸入10個字")]
        public string code { get; set; }

        [Display(Name = "中文名稱")]
        [StringLength(20, ErrorMessage = "codename最多輸入20個字")]
        public string? codename { get; set; }

        [Display(Name = "匯率")]
        [Precision(11, 4)] 
        [Range(0, 99999.9999, ErrorMessage = "Value must be between 0 and 99999.9999")]
        public decimal ratefloat { get; set; }


        [Display(Name = "備註")]
        [StringLength(200, ErrorMessage = "ratefloat最多輸入200個字")]
        public string? description { get; set; }

        [Display(Name = "符號")]
        [StringLength(10, ErrorMessage = "symbol最多輸入10個字")]
        public string symbol { get; set; }
    }
}
