using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RuhRoh.Samples.WebAPI.Controllers.Models
{
    public class CreateTodoItemModel
    {
        [BindRequired, Required(AllowEmptyStrings = false), StringLength(200)]
        public string Description { get; set; }
    }
}