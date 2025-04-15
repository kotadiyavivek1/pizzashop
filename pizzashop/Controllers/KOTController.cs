using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.ImplementationService;
using pizzashop_Repository.interfaceService;
using pizzashop_Repository.ViewModel;

namespace pizzashop.Controllers
{
    public class KOTController : Controller
    {
        private readonly IKotService kotService;
        public KOTController(IKotService kotService)
        {
            this.kotService = kotService;
        }
        public IActionResult Index()
        {
            ViewBag.ActiveNav = "KOT";
            return View(kotService.GetKotDto());
        }
        [HttpGet]
        [Route("/KOT/GetAllCards")]
        public IActionResult GetAllCards(string currentStatus,int currentCategory)
        {   
            List<KotDto> kotDtos = kotService.GetAllCards(currentCategory,currentStatus);
            return PartialView("_cardPartial",kotDtos);
        }
       
       
        [HttpGet]
        [Route("/KOT/GetKotItemPartial")]
        public IActionResult GetKotItemPartial(int orderId,string status)
        {
            ViewData["status"] = status;
            KotDto item = kotService.GetKotItem(orderId,status); 
            return PartialView("_KotModalPartial",item);
        }
        [HttpPost]
        [Route("/KOT/UpdateReadyItems")]
        public IActionResult UpdateReadyItems(List<KotItemUpdateDto> KotItemUpdateDto,string status)
        {
            if(kotService.UpdateItem(KotItemUpdateDto,status))
            {
                return Json(new {success = true,message="item is updated"});
            }
            else
            {
                return Json(new {success=false,message= "something went wrong"});
            }
        }
    }
}