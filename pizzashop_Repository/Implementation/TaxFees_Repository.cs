using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Implementation;
public class TaxFees_Repository : ITaxFees_Repository
{
    private readonly PizzashopContext _context;
    public TaxFees_Repository(PizzashopContext context)
    {
        _context = context;
    }

    public bool addTaxFees(TaxFeesDto taxFeesDto, string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        var existingText = _context.Taxesandfees.FirstOrDefault(u=>u.Name ==taxFeesDto.Name);
        if (addTaxFees != null && user != null && existingText==null)
        {
            var taxfee = new Taxesandfee
            {
                Name = taxFeesDto.Name ?? "",
                Taxvalue = taxFeesDto.Type ? null : taxFeesDto.TaxValue,
                Type = taxFeesDto.Type,
                Percentage = taxFeesDto.Type ? taxFeesDto.TaxValue : null,
                Isactive = taxFeesDto.IsEnabled,
                Isdefault = taxFeesDto.IsDefault,
                Isdeleted = false,
                Createdby = user.Id,
            };
            _context.Taxesandfees.Add(taxfee);
            _context.SaveChanges();
            taxFeesDto.Id = taxfee.Id;
            return true;
        }
        return false;
    }
    public TaxFeesDto GetTaxFromId(int id)
    {
        Taxesandfee taxesandfee = _context.Taxesandfees.FirstOrDefault(u=>u.Id == id) ?? new Taxesandfee();
        return new TaxFeesDto()
        {
            Id = taxesandfee.Id,
            Name=taxesandfee.Name,
            Type=taxesandfee.Type ?? true,
            TaxValue = taxesandfee.Type.HasValue && taxesandfee.Type.Value ? taxesandfee.Percentage : taxesandfee.Taxvalue,
            IsEnabled = taxesandfee.Isactive ?? true,
            IsDefault = taxesandfee.Isdefault ?? true
        };
    }
    public bool DeleteTaxFees(int id)
    {
        var taxfee = _context.Taxesandfees.FirstOrDefault(i => i.Id == id);
        if (taxfee != null)
        {
            taxfee.Isdeleted = true;
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool isUpdate(FilterPaginationDto<TaxFeesDto> taxFeesDto)
    {
        if(taxFeesDto.Item == null)
        {
            return false;
        }
        var taxFee = _context.Taxesandfees.FirstOrDefault(i=>i.Id == taxFeesDto.Item.Id);

        if(taxFeesDto.Item!=null && taxFee!=null)
        {
            taxFee.Id=taxFeesDto.Item.Id.Value;
            taxFee.Name=taxFeesDto.Item.Name ?? "";
            taxFee.Createdat=DateTime.Now;
            taxFee.Isactive=taxFeesDto.Item.IsEnabled;
            taxFee.Isdefault=taxFeesDto.Item.IsDefault;
            _context.SaveChanges();
            _context.Update(taxFee);
            return true;
        }
        return false;
    }


    public List<TaxFeesDto> TaxFeesFilter(int pageNumber, int PageSize, string searchString)
    {
        IQueryable<Taxesandfee> taxesAndFees = _context.Taxesandfees.AsQueryable();
        if (String.IsNullOrEmpty(searchString))
        {
            return taxesAndFees.Skip((pageNumber - 1) * PageSize).Take(PageSize).Where(t => t.Isdeleted == false).Select(t => new TaxFeesDto
            {
                Id = t.Id,
                TaxValue = t.Taxvalue,
                Type = t.Type ?? true,
                Percentage = t.Percentage ?? 0,
                Name = t.Name,
                IsEnabled = t.Isactive ?? true,
                IsDefault = t.Isdefault ?? false,
            }).ToList();
        }
        return taxesAndFees.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).Skip((pageNumber - 1) * PageSize).Take(PageSize).Where(t => t.Isdeleted == false).Select(t => new TaxFeesDto
        {
            Id = t.Id,
            TaxValue = t.Taxvalue,
            Type = t.Type ?? true,
            Percentage = t.Percentage ?? 0,
            Name = t.Name,
            IsEnabled = t.Isactive ?? true,
            IsDefault = t.Isdefault ?? false,
        }).ToList();
    }

    public List<TaxFeesDto> GetALLTaxFees(string searchString)
    {

        if (!string.IsNullOrEmpty(searchString))
        {
            return _context.Taxesandfees.Where(t => t.Name.ToLower().Contains(searchString.ToLower()) && t.Isdeleted == false).Select(t => new TaxFeesDto
            {
                Id = t.Id,
                TaxValue = t.Taxvalue,
                Type = t.Type ?? true,
                Percentage = t.Percentage ?? 0,
                Name = t.Name,
                IsEnabled = t.Isactive ?? true,
                IsDefault = t.Isdefault ?? false,
            }).ToList();
        }
        return _context.Taxesandfees.
        Where(t => t.Isdeleted == false).Select(t => new TaxFeesDto
        {
            Id = t.Id,
            TaxValue = t.Taxvalue,
            Type = t.Type ?? true,
            Percentage = t.Percentage ?? 0,
            Name = t.Name,
            IsEnabled = t.Isactive ?? true,
            IsDefault = t.Isdefault ?? false,
        }).ToList();
    }

    public int totalItems(string searchString)
    {
        int count = _context.Taxesandfees.Where(u => u.Name.Contains(searchString)).Count();
        return count;
    }
}
