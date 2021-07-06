using System.Collections.Generic;

namespace AYN.Web.Validators
{
    public interface IValidator<in T>
        where T : class
    {
        string Validate(T entity);
    }
}
