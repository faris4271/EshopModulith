using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Exeption
{
    public sealed record ValidationError(string PropertyName, string ErrorMessage);
}
