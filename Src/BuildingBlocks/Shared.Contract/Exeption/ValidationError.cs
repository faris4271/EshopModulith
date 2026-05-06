using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contract.Exeption
{
    public sealed record ValidationError(string PropertyName, string ErrorMessage);
}
