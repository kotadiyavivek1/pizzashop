using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.VisualBasic;
using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;

namespace pizzashop.Controllers
{
    public class SectionController : Controller
    {
        private readonly ITableSection_Service tableSectionService;
        public SectionController(ITableSection_Service _tableSectionService)
        {
            tableSectionService = _tableSectionService;
        }
        public IActionResult TableSection()
        {
            TableSectionDto tableSectionDto = tableSectionService.getAllItem();
            return View(tableSectionDto);
        }
        public IActionResult FilteredTablesBySection(int sectionId, int PageNumber, int PageSize, string searchString)
        {
            FilterPaginationDto<TableDto> filterPaginationDto = tableSectionService.FilterTable(sectionId, PageNumber, PageSize, searchString);
            return PartialView("_TablePartial", filterPaginationDto);
        }
        [Route("Section/AddSection")]
        [HttpPost]
        public IActionResult AddSection(TableSectionDto tableSectionDto)
        {
            ModelState.Clear();
            bool isSectionValid = TryValidateModel(tableSectionDto.SectionDto ?? new SectionDto(), nameof(SectionDto));
            if (!isSectionValid)
            {
                return BadRequest(new { success = false, message = "Something went wrong..." });
            }
            if (isSectionValid)
            {
                string email = Request.Cookies["email"] ?? "";
                tableSectionService.AddSection(tableSectionDto, email);
                return Ok(new { success = true, message = "Section added successfully." });
            }

            return RedirectToAction("TableSection");
        }


        [HttpGet]
        public IActionResult GetSectionById(int sectionId)
        {
            SectionDto sectionDto = tableSectionService.GetSectionById(sectionId);
            return PartialView("_EditSectionPartial", sectionDto);
        }

        [HttpPost]
        [Route("/Section/EditSection")]
        public IActionResult EditSection(int Id, SectionDto SectionDto)
        {
            string email = Request.Cookies["email"] ?? "";
            if (!ModelState.IsValid)
            {
                List<string> errormessage = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, message = errormessage });
            }

            if (tableSectionService.EditSection(Id, SectionDto, email))
            {
                return Json(new { success = true, message = "section update successfully" });
            }
            return Json(new { success = false, message = "Something went wrong..." });

        }


        [Route("/Section/DeleteSection")]
        [HttpPost]
        public IActionResult DeleteSection(int sectionId)
        {

            if (tableSectionService.DeleteSection(sectionId))
            {
                TempData["Success"] = "Section Deleted Successfully";
                return RedirectToAction("TableSection");
            }
            TempData["Error"] = "Section Not Deleted";
            return RedirectToAction("TableSection");
        }

        [Route("/Section/addTable")]
        
        public IActionResult addTable(TableSectionDto tableSectionDto)
{
    // First check if the main DTO is null
    if (tableSectionDto == null)
    {
        ModelState.AddModelError("", "Invalid request data");
    }
    else if (tableSectionDto.TableDto == null)
    {
        ModelState.AddModelError("TableDto", "Table data is required");
    }

    if (!ModelState.IsValid)
    {
        var errorMessages = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return Json(new { 
            success = false, 
            errors = errorMessages 
        });
    }

    string email = Request.Cookies["email"] ?? "";
    if (tableSectionService.AddTable(tableSectionDto, email))
    {
        return Json(new { success = true, message = "Table Added Successfully" });
    }
    
    return Json(new { 
        success = false, 
        message = "Table Not Added",
        errors = new List<string> { "Failed to add table. Please try again." } 
    });
}

        public IActionResult GetTableById(int id)
        {
            TableSectionDto tableDto = tableSectionService.GetTableById(id);
            return PartialView("_EditTablePartial", tableDto);
        }

        [HttpPost]
        public IActionResult EditTable(TableSectionDto tableSectionDto)
        {
            string email = Request.Cookies["email"] ?? "";

            if (ModelState.IsValid)
            {
                if (tableSectionDto.TableDto != null)
                {
                    bool isUpdated = tableSectionService.EditTable(tableSectionDto.TableDto.Id, tableSectionDto.TableDto, email);

                    if (isUpdated)
                    {
                        return Json(new { success = true, message = "Table updated successfully." });
                    }
                    return Json(new { success = false, message = "Failed to update table." });
                }
                return Json(new { success = false, message = "Invalid table data." });
            }
            return Json(new { success = false, message = "Model state is invalid." });
        }


        [HttpPost]
        public IActionResult DeleteTable(int id)
        {
            if (tableSectionService.DeleteTable(id))
            {
                return RedirectToAction("TableSection");
            }
            return RedirectToAction("TableSection");
        }


        [HttpPost]
        public IActionResult DeleteTables(List<int> tableIds)
        {
            if (tableSectionService.DeleteTables(tableIds))
            {
                return RedirectToAction("TableSection");
            }
            return RedirectToAction("TableSection");
        }
    }
}