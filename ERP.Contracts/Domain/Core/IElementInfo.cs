using System;

namespace ERP.Contracts.Domain.Core
{
    public interface IElementInfo
    {
        int ElementInfoId { get; set; }
        int EmployeeId { get; set; }
        DateTime Time { get; set; }
        double Amount { get; set; }
    }
}
