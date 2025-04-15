using Microsoft.AspNetCore.Mvc;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;

namespace pizzashop.Controllers
{
    public class TaxFeesController : Controller
    {
        private readonly ITaxFees_Service taxFeesService;

        public TaxFeesController(ITaxFees_Service _taxFeesService)
        {
            taxFeesService = _taxFeesService;
        }

        public IActionResult TaxFees()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTax(FilterPaginationDto<TaxFeesDto> taxFeesDto)
        {
            if (ModelState.IsValid)
        {
            if (taxFeesService.updateTax(taxFeesDto))
            {
                return Json(new { success = true, message = "Tax updated successfully" });
            }

            return Json(new { success = false, message = "Update failed" });
        }

    
        var errors = ModelState
        .Where(ms => ms.Value.Errors.Count > 0)
        .Select(ms => new
        {
            field = ms.Key,
            errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
        }).ToList();

        return Json(new
        {
            success = false,
            message = "Validation failed",
            errors
        });
        }


        [HttpGet]
        [Route("/TaxFees/FilterTaxes")]
        public IActionResult FilterTaxes(int pageNumber = 1, int pageSize = 5, string searchString = "")
        {
            var filterPaginationDto = taxFeesService.filterPaginationDto(pageNumber, pageSize, searchString);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TaxFeesTable", filterPaginationDto);
            }
            return View("TaxFees", filterPaginationDto);
        }

        [HttpGet]
        public IActionResult GetAllTaxFees(string searchString = "")
        {
            List<TaxFeesDto> TaxFees = taxFeesService.GetALLTaxFees(searchString);
            return PartialView("_TaxFeesTable", TaxFees);
        }
        
        [HttpGet]
        [Route("/TaxFees/GetTaxDetail")]
        public IActionResult GetTaxDetail(int taxId)
        {
            return Json(taxFeesService.taxFees(taxId));
        }

        [HttpPost]
        [Route("/TaxFees/DeleteTaxFees")]
        public IActionResult DeleteTaxFees(int id)
        {
            if (taxFeesService.DeleteTaxFees(id))
            {
                return RedirectToAction("TaxFees");
            }
            return RedirectToAction("TaxFees");
        }
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult AddTaxFees(FilterPaginationDto<TaxFeesDto> filterPaginationDto)
{
    // Validate null item
    if (filterPaginationDto?.Item == null)
    {
        return BadRequest(new { success = false, message = "Tax data is missing." });
    }

    // Try to validate the Item model manually
    if (!TryValidateModel(filterPaginationDto.Item))
    {
        var errorList = ModelState
            .Where(kvp => kvp.Value.Errors.Count > 0)
            .Select(kvp => new
            {
                Field = kvp.Key,
                Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
            }).ToList();

        return BadRequest(new { success = false, errors = errorList });
    }

    // Get user email from cookie
    string email = Request.Cookies["email"] ?? string.Empty;

    // Try adding tax
    bool success = taxFeesService.addTaxFees(filterPaginationDto.Item, email);

    if (success)
    {
        return Json(new { success = true });
    }

    // Optional: log input if saving failed
    // _logger.LogWarning("Failed to add tax. Data: {@Item}, Email: {Email}", filterPaginationDto.Item, email);

    return BadRequest(new { success = false, message = "Failed to add tax." });
}



    }
}