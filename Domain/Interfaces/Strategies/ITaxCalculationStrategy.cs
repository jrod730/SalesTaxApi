using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Strategies
{
    public interface ITaxCalculationStrategy
    {
        TaxType TaxType { get; }
        decimal Calculate(decimal price);
    }
}
